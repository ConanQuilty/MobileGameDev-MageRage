using System.Collections.Generic;
using UnityEngine;

using Unity.Profiling;   // top of EnemyPool.cs


// One pool per enemy type. Prewarms in Awake so play never calls Instantiate,
// unless a wave is bigger than `size` (then it grows once and you raise `size`).
public class EnemyPool : MonoBehaviour
{
    static readonly ProfilerMarker s_Spawn = new ProfilerMarker("EnemyPool.Spawn");
    [SerializeField] Enemy prefab;
    [SerializeField] int size = 60;
    [SerializeField] Transform player;

    public int Peak { get; private set; }

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
        using (s_Spawn.Auto()) //For debugging
        {
        var e = _free.Count > 0 ? _free.Pop() : Create(); // grow only if prewarm was too small
        e.transform.position = position;
        e.gameObject.SetActive(true); // OnEnable resets health and velocity
        ActiveCount++;
        if (ActiveCount > Peak) Peak = ActiveCount;
        return e;
        }
    }

    public void Release(Enemy e)
    {
        if (!e.gameObject.activeSelf) return; // already released
        e.gameObject.SetActive(false);
        _free.Push(e);
        ActiveCount--;
    }

    void OnDestroy() => Debug.Log($"[Pool] {name} peak {Peak} of {size}");
}