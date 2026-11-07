using UnityEngine;

public class FireOrbs : MonoBehaviour
{
    [SerializeField] Transform[] orbs; // 2 child circles, inactive in the scene
    [SerializeField] PlayerStats stats;
    [SerializeField] float radius = 1.6f; // distance from the mage
    [SerializeField] float degreesPerSecond = 180f;
    [SerializeField] float hitRadius = 0.35f; // orb size
    [SerializeField] float hitEvery = 0.5f;
    [SerializeField] float baseDamage = 1f;
    float _angle;

void OnEnable()
{
    // Make each orb look exactly as big as its hit area (hitRadius × 2 wide)
    float d = hitRadius * 2f;
    for (int i = 0; i < orbs.Length; i++)
    {
        orbs[i].localScale = Vector3.one * (hitRadius * 2f);
        orbs[i].gameObject.SetActive(true);
    }
}
    void OnDisable() { for (int i = 0; i < orbs.Length; i++) if (orbs[i]) orbs[i].gameObject.SetActive(false); }

    void Update()
    {
        _angle += degreesPerSecond * stats.AttackSpeedMult * Time.deltaTime;   // Atk spins orbs faster
        Vector2 centre = transform.position;
        var list = Enemy.Active;

        for (int k = 0; k < orbs.Length; k++)
        {
            float a = (_angle + k * 360f / orbs.Length) * Mathf.Deg2Rad;   // spread evenly
            Vector2 p = centre + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius;
            orbs[k].position = p;

            //Backwards, because a kill removes that enemy from the list mid-loop.
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var e = list[i];
                float r = hitRadius + e.Radius;
                if (((Vector2)e.transform.position - p).sqrMagnitude > r * r) continue;
                if (Time.time < e.LastOrbHit + stats.Cooldown(hitEvery)) continue;
                e.LastOrbHit = Time.time;
                e.Hit(stats.Damage(baseDamage));
            }
        }
    }
}