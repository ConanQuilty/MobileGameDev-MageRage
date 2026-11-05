using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{

    public static readonly List<Enemy> Active = new List<Enemy>(256);   // active/alive enemies
    public static event System.Action<Enemy> Killed;
    public float Radius => 0.5f * type.scale; // the Circle sprite is 1 unit wide
    public int Life { get; private set; } // +1 every time the pool reuses this object >
                                          // allows the differenciation of the same object between "lives" or instances
                                          // eg. Missile targets enemy, they die from other reasons and spawnpool reuses them
                                          //     The missile would then target the respawned enemy far away
    [HideInInspector] public float LastOrbHit;
   
    [SerializeField] EnemyType type;
    public EnemyType Type => type;

    EnemyPool _pool;
    Transform _target;
    Rigidbody2D _body;
    SpriteRenderer _sprite;
    int _health;
    float _slowUntil, _slowFactor = 1f;

    public Enemy Init(EnemyPool pool, Transform target)
    {
        _pool = pool;
        _target = target;
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.one * type.scale;
        _sprite.color = type.color;
        return this;
    }

    void OnEnable()
    {
        Life++; // Same reused object but need to clarify diff enemy
        _health = type.maxHealth;
        _slowUntil = 0f;
        LastOrbHit = -1f;
        if (_body) _body.linearVelocity = Vector2.zero;   // forget the last life's movement
        if (_sprite) _sprite.color = type.color;
        Active.Add(this);
    }

    void OnDisable() => Active.Remove(this);

    void FixedUpdate()
    {
        if (_target == null) return;
        float speed = type.speed;
        if (_slowUntil > 0f)
        {
            if (Time.time < _slowUntil) speed *= _slowFactor;
            else { _slowUntil = 0f; _sprite.color = type.color; }   // slow wore off
        }
        Vector2 dir = ((Vector2)_target.position - _body.position).normalized;
        _body.linearVelocity = dir * speed;
    }


    public void Hit(int damage)
    {
        if (_health <= 0) return;
        _health -= damage;
        if (_health > 0) return;
        Killed?.Invoke(this); // before Despawn, so listeners can read the position
        Despawn();
    }

    public void Slow(float factor, float duration)  //for a slowing status effect
    {
        _slowFactor = factor;
        _slowUntil = Time.time + duration;
        _sprite.color = Color.Lerp(type.color, Color.cyan, 0.6f);
    }


    public void Despawn() => _pool.Release(this);
}