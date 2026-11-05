using UnityEngine;

public class XpGem : MonoBehaviour
{
    [SerializeField] float magnetRadius = 2.5f;
    [SerializeField] float pickupRadius = 0.5f;
    [SerializeField] float speed = 14f;          // faster than the mage, so it always catches up
    [SerializeField] float lifetime = 30f;

    GemPool _pool;
    Transform _player;
    PlayerXP _xp;
    int _value;
    float _dieAt;
    bool _pulled;

    public XpGem Init(GemPool pool, Transform player, PlayerXP xp)
    {
        _pool = pool; _player = player; _xp = xp;
        return this;
    }

    public void Drop(Vector2 at, int value)
    {
        transform.position = at;
        _value = value;
        _dieAt = Time.time + lifetime;
        _pulled = false;
        transform.localScale = Vector3.one * (value >= 4 ? 0.45f : 0.3f);   // Valuable gems look bigger
    }

    void Update()
    {
        if (Time.time >= _dieAt) { _pool.Release(this); return; }

        Vector2 pos = transform.position;
        Vector2 to = (Vector2)_player.position - pos;
        float d2 = to.sqrMagnitude;
        if (d2 <= pickupRadius * pickupRadius)
        {
            _xp.Add(_value);
            _pool.Release(this);
            return;
        }
        if (_pulled || d2 <= magnetRadius * magnetRadius)
        {
            _pulled = true;   // once pulled, it keeps coming
            transform.position = pos + to.normalized * (speed * Time.deltaTime);
        }
    }
}