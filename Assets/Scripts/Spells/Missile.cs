using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float lifetime = 1.5f;

    MissilePool _pool;
    Enemy _target;
    int _targetLife; // which "life" of the pooled enemy we aimed at
    Vector2 _dir;
    float _damage;
    float _dieAt;

    public Missile Init(MissilePool pool) { _pool = pool; return this; }

    public void Launch(Vector2 from, Enemy target, float damage)
    {
        transform.position = from;
        _target = target;
        _targetLife = target.Life;
        _damage = damage;
        _dieAt = Time.time + lifetime;
        _dir = ((Vector2)target.transform.position - from).normalized;
    }

    void Update()
    {
        if (Time.time >= _dieAt) { _pool.Release(this); return; }

        Vector2 pos = transform.position;
        // Same object AND same life: it hasn't died and been reused at the arena edge.
        if (_target.isActiveAndEnabled && _target.Life == _targetLife)
        {
            Vector2 to = (Vector2)_target.transform.position - pos;
            float reach = _target.Radius + 0.15f;
            if (to.sqrMagnitude <= reach * reach)
            {
                _target.Hit(_damage);
                _pool.Release(this);
                return;
            }
            _dir = to.normalized;                    // keep homing
        }
        transform.position = pos + _dir * (speed * Time.deltaTime);
    }
}