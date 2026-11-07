using UnityEngine;

// Every `every` s: a cold pulse around the mage. Enemies inside take damage and move at half speed for 2 s.
// The ring is a separate scene object that grows and fades for 0.3 s, so you can see the pulse.
public class FrostNova : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] Transform ring;
    [SerializeField] float every = 4f;
    [SerializeField] float radius = 3.5f;
    [SerializeField] float slowFactor = 0.5f;
    [SerializeField] float slowFor = 2f;
    [SerializeField] float ringTime = 0.3f;
    [SerializeField] float baseDamage = 1f;

    SpriteRenderer _ringSprite;
    float _next, _ringUntil;

    void Awake()    => _ringSprite = ring.GetComponent<SpriteRenderer>(); 
    void OnEnable() => _next = Time.time + 0.5f; // first pulse soon after unlocking

    void Update()
    {
        if (_ringUntil > 0f) AnimateRing();
        if (Time.time < _next) return;
        _next = Time.time + stats.Cooldown(every);
        Pulse();
    }

    void Pulse()
    {
        Vector2 centre = transform.position;
        float dmg = stats.Damage(baseDamage);
        var list = Enemy.Active;
        for (int i = list.Count - 1; i >= 0; i--) //backwards: kills remove from the list
        {
            var e = list[i];
            float r = radius + e.Radius;
            if (((Vector2)e.transform.position - centre).sqrMagnitude > r * r) continue;
            e.Slow(slowFactor, slowFor);
            e.Hit(dmg);
        }
        ring.position = centre;
        ring.localScale = Vector3.zero;
        ring.gameObject.SetActive(true);
        _ringUntil = Time.time + ringTime;
    }

    void AnimateRing()
    {
        float t = 1f - (_ringUntil - Time.time) / ringTime;    // 0 to 1 over ringTime
        if (t >= 1f) { ring.gameObject.SetActive(false); _ringUntil = 0f; return; }
        ring.localScale = Vector3.one * (radius * 2f * t); // circle sprite: scale = diameter
        var c = _ringSprite.color;
        c.a = 0.5f * (1f - t);
        _ringSprite.color = c;
    }
}