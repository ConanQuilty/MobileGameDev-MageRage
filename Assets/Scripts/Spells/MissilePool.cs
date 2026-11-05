using System.Collections.Generic;
using UnityEngine;

public class MissilePool : MonoBehaviour
{
    [SerializeField] Missile prefab;
    [SerializeField] int size = 10; // 0.5s cooldown × 1.5s lifetime = ~3 alive at a time
    readonly Stack<Missile> _free = new Stack<Missile>();

    void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            var m = Create();
            m.gameObject.SetActive(false);
            _free.Push(m);
        }
    }

    Missile Create() => Instantiate(prefab, transform).Init(this);

    public Missile Spawn()
    {
        var m = _free.Count > 0 ? _free.Pop() : Create();
        m.gameObject.SetActive(true);
        return m;
    }

    public void Release(Missile m)
    {
        if (!m.gameObject.activeSelf) return;
        m.gameObject.SetActive(false);
        _free.Push(m);
    }
}