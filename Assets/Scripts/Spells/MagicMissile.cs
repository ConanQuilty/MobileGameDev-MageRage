using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    [SerializeField] MissilePool pool;
    [SerializeField] float cooldown = 0.5f;
    [SerializeField] float range = 9f;
    [SerializeField] float baseDamage = 2; 
    [SerializeField] PlayerStats stats;
    float _next;

    void Update()
    {
        if (Time.time < _next) return;
        Enemy target = Nearest();
        if (target == null) return; // nothing in range: try again next frame
        _next = Time.time + stats.Cooldown(cooldown);
        pool.Spawn().Launch(transform.position, target, stats.Damage(baseDamage)); //stats.Damage(x) Will multiply dmg by the stats multiplyer
    }

    Enemy Nearest()
    {
        Enemy best = null;
        float bestSq = range * range;
        Vector2 player = transform.position;
        var list = Enemy.Active;
        for (int i = 0; i < list.Count; i++)
        {
            float d = ((Vector2)list[i].transform.position - player).sqrMagnitude;
            if (d < bestSq) { bestSq = d; best = list[i]; }
        }
        return best;
    }
}