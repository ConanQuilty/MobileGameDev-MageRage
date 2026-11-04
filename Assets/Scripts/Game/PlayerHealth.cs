using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] float invulnerableTime = 0.6f; 
    [SerializeField] float flashTime = 0.1f;
    [SerializeField] SpriteRenderer sprite; 
    [SerializeField] Color flashColor = Color.red;

    public int Max => maxHealth;
    public int Current { get; private set; }
    public event System.Action<int, int> Changed; // (current, max)

    Color _baseColor;
    float _invulnUntil, _flashUntil;

    void Awake() { Current = maxHealth; _baseColor = sprite.color; }
    void Start() => Changed?.Invoke(Current, maxHealth);

    void OnCollisionStay2D(Collision2D c)
    {
        if (Time.time < _invulnUntil) return;
        if (!c.collider.TryGetComponent(out Enemy enemy)) return; 
        TakeDamage(enemy.Type.contactDamage);
    }

    public void TakeDamage(int amount)
    {
        if (Current <= 0) return;
        Current = Mathf.Max(0, Current - amount);
        _invulnUntil = Time.time + invulnerableTime;
        _flashUntil = Time.time + flashTime;
        sprite.color = flashColor;
        Changed?.Invoke(Current, maxHealth);
        if (Current == 0) GameManager.Instance.Lose("hp");
    }

    void Update()   // ends the flash; a timer check instead of a coroutine
    {
        if (_flashUntil > 0f && Time.time >= _flashUntil)
        {
            sprite.color = _baseColor;
            _flashUntil = 0f;
        }
    }
}