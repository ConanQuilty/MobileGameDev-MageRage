using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType type;
    public EnemyType Type => type;

    EnemyPool _pool;
    Transform _target;
    Rigidbody2D _body;
    int _health;

    public Enemy Init(EnemyPool pool, Transform target)
    {
        _pool = pool;
        _target = target;
        _body = GetComponent<Rigidbody2D>();
        transform.localScale = Vector3.one * type.scale;
        GetComponent<SpriteRenderer>().color = type.color;
        return this;
    }

    void OnEnable()
    {
        _health = type.maxHealth;
        if (_body) _body.linearVelocity = Vector2.zero;   // forget the last life's movement
    }

    void FixedUpdate()
    {
        if (_target == null) return;
        Vector2 dir = ((Vector2)_target.position - _body.position).normalized;
        _body.linearVelocity = dir * type.speed;
    }

    public void Hit(int damage) 
    {
        _health -= damage;
        if (_health <= 0) Despawn();
    }

    public void Despawn() => _pool.Release(this);
}