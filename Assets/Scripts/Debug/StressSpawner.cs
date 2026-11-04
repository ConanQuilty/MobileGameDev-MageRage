using UnityEngine;

// Debug harness: spawns `count` real Grunts from the pool in rings around the player,
// so the Thu 12 before/after uses the actual enemy code. Enemies steer themselves.
public class StressSpawner : MonoBehaviour
{
    [SerializeField] EnemyPool pool;
    [SerializeField] Transform player;
    [SerializeField] int count = 200;
    [SerializeField] float radius = 10f;
    bool _spawned;

    public void Spawn()                 // debug "Stress" button
    {
        Debug.Log("[Stress] Spawn() Called");
        if (_spawned) return;
        _spawned = true;
        for (int i = 0; i < count; i++)
        {
            float a = i * Mathf.PI * 2f / count;
            float r = radius + (i % 5);
            pool.Spawn((Vector2)player.position + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * r);
        }
        Debug.Log($"[Stress] spawned {count} from pool");
    }
}
