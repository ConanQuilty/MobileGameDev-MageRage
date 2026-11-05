using UnityEngine;


public class PlayerXP : MonoBehaviour
{
    [SerializeField] int xpPerLevel = 5;
    public int Level { get; private set; } = 1;
    public int Xp { get; private set; }
    public int Next => Level * xpPerLevel;

    public event System.Action<int, int> XpChanged; 
    public event System.Action<int> LeveledUp;  

    void Start() => XpChanged?.Invoke(Xp, Next);

    public void Add(int amount)
    {
        Xp += amount;
        while (Xp >= Next) // a Tank gem can be worth more than one level
        {
            Xp -= Next;
            Level++;
            LeveledUp?.Invoke(Level);
        }
        XpChanged?.Invoke(Xp, Next);
    }
}