using UnityEngine;

public class StressSpawner : MonoBehaviour
{
    [SerializeField] Rigidbody2D enemyPrefab;
    [SerializeField] Transform player;
    [SerializeField] int count = 200;
    [SerializeField] float radius = 10f;
    [SerializeField] float speed = 2f;

    Rigidbody2D[] _bodies;

    public void Spawn()
    {
        if (_bodies != null) return;  // If exists already skip spawn()
        _bodies = new Rigidbody2D[count];

        // Spawn in 5 rings around player
        for (int i = 0; i < count; i++)
        {
            float a = i * Mathf.PI * 2f / count;
            float r = radius + (i % 5);
            Vector2 pos = (Vector2)player.position + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * r;
            _bodies[i] = Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
        }
        Debug.Log($"[Stress] spawned {count}");
    }

    void FixedUpdate()
    {
        if (_bodies == null || LifecycleGuard.IsPaused) return;
        Vector2 target = player.position;
        for (int i = 0; i < _bodies.Length; i++)
        {
            Vector2 dir = (target - _bodies[i].position).normalized;
            _bodies[i].linearVelocity = dir * speed;
        }
    }
}