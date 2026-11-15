using UnityEngine;

public enum Stat { HP, Def, Atk, Speed, AtkSpeed }   // order = the level-up buttons, 0 to 3

public class PlayerStats : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    public float DamageMult { get; private set; } = 1f;
    public float AttackSpeed { get; private set; } = 1f; //1 = base atk speed + 10% attack
    public float SpeedMult  { get; private set; } = 1f;
    public float ArcaneMult => Wallet.Instance != null ? 1f + 0.15f * Wallet.Instance.Level : 1f; //Upgrade arcane power by shop between runs

    public float Damage(float baseDamage) => baseDamage * DamageMult * ArcaneMult;
    public float Cooldown(float baseSeconds) => baseSeconds / AttackSpeed;
    public float AttackSpeedMult { get; private set; } = 1f; 

    public void Apply(Stat s)
    {
        switch (s)
        {
            case Stat.HP:    health.AddMax(2); break;
            case Stat.Def:   health.AddSafety(0.2f); break;
            case Stat.Atk:   DamageMult += 0.10f; break;
            case Stat.Speed: SpeedMult  += 0.15f; break;
            case Stat.AtkSpeed: AttackSpeedMult += 0.05f; break; 
        }
    }
}