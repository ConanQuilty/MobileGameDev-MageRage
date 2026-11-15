using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    public int Coins { get; private set; }
    public int Level { get; private set; }
    public event System.Action Changed;
    public int NextCost => (int)(100 * Mathf.Pow(1.5f, Level)); // Using the 1.5^level creates a smoother slope than standard multiplication

    void Awake() => Instance = this;

    public void Earn(int n)
    {
        if (n <= 0) return; // a 0 coin run won't trigger a save
        Coins += n;
        Changed?.Invoke();
    }

    public void Restore(int c, int l) { Coins = c; Level = l; Changed?.Invoke(); }

    public bool TryUpgrade()
    {
        int cost = NextCost;
        if (Coins < NextCost) return false;
        Coins -= NextCost;
        Level++;
        Changed?.Invoke();
        Telemetry.Log("upgrade_purchased", ("upgrade_id", "arcane_power"),
            ("level", Level), ("cost", cost), ("coins_left", Coins));
        return true;
    }
}