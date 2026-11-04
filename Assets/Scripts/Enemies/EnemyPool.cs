using System.Collections.Generic;
using UnityEngine;

// One pool per enemy type. Prewarms in Awake so play never calls Instantiate,
// unless a wave is bigger than `size` (then it grows once and you raise `size`).
public class EnemyPool : MonoBehaviour
{
    [SerializeField] Enemy prefab;
    [SerializeField] int size = 60;
    [SerializeField] Transform player;

    readonly Stack<Enemy> _free = new Stack<Enemy>();
    public int ActiveCount { get; private set; }

    void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            var e = Create();
            e.gameObject.SetActive(false);
            _free.Push(e);
        }
    }

    Enemy Create() => Instantiate(prefab, transform).Init(this, player);

    public Enemy Spawn(Vector2 position)
    {
        var e = _free.Count > 0 ? _free.Pop() : Create(); // grow only if prewarm was too small
        e.transform.position = position;
        e.gameObject.SetActive(true); // OnEnable resets health and velocity
        ActiveCount++;
        return e;
    }

    public void Release(Enemy e)
    {
        if (!e.gameObject.activeSelf) return; // already released
        e.gameObject.SetActive(false);
        _free.Push(e);
        ActiveCount--;
    }
}