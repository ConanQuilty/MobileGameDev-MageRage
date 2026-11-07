using UnityEngine;

public class SpellUnlocks : MonoBehaviour
{
    [SerializeField] PlayerXP xp;
    [SerializeField] FireOrbs orbs;
    [SerializeField] FrostNova nova;
    [SerializeField] int orbsLevel = 3;
    [SerializeField] int novaLevel = 5;

    void OnEnable()  => xp.LeveledUp += OnLevel;
    void OnDisable() => xp.LeveledUp -= OnLevel;

    void OnLevel(int level)
    {
        if (level >= orbsLevel) orbs.enabled = true;
        if (level >= novaLevel) nova.enabled = true;
    }

    public bool IsOrbsLevel(int level) => level == orbsLevel;
    public bool IsNovaLevel(int level) => level == novaLevel;

}