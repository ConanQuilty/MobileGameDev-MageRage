using System.Collections.Generic;
using UnityEngine;

public class GemPool : MonoBehaviour
{
    [SerializeField] XpGem prefab;
    [SerializeField] int size = 150;
    [SerializeField] Transform player;
    [SerializeField] PlayerXP xp;
    readonly Stack<XpGem> _free = new Stack<XpGem>();

    void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            var g = Create();
            g.gameObject.SetActive(false);
            _free.Push(g);
        }
    }

    void OnEnable()  => Enemy.Killed += OnKilled;
    void OnDisable() => Enemy.Killed -= OnKilled;

    XpGem Create() => Instantiate(prefab, transform).Init(this, player, xp);

    void OnKilled(Enemy e)
    {
        var g = _free.Count > 0 ? _free.Pop() : Create();
        g.gameObject.SetActive(true);
        g.Drop(e.transform.position, e.Type.xp);
    }

    public void Release(XpGem g)
    {
        if (!g.gameObject.activeSelf) return;
        g.gameObject.SetActive(false);
        _free.Push(g);
    }
}