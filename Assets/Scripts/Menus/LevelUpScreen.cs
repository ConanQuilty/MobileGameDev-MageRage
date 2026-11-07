using TMPro;
using UnityEngine;


public class LevelUpScreen : MonoBehaviour
{
    [SerializeField] PlayerXP xp;
    [SerializeField] PlayerStats stats;
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text title;
    [SerializeField] SpellUnlocks unlocks;
    int _pending;

    void OnEnable()  { xp.LeveledUp += OnLeveledUp; panel.SetActive(false); }
    void OnDisable() { xp.LeveledUp -= OnLeveledUp; }

    void OnLeveledUp(int level)
    {
        _pending++;
        if (panel.activeSelf) return;
        LifecycleGuard.AddHold();
        panel.SetActive(true);
        ShowTitle();
    }

    void ShowTitle()
    {
        int lvl = xp.Level - _pending + 1;       // the oldest level-up still waiting
        if      (unlocks.IsOrbsLevel(lvl)) title.SetText("Level 3! Fire Orbs unlocked");
        else if (unlocks.IsNovaLevel(lvl)) title.SetText("Level 5! Frost Nova unlocked");
        else                               title.SetText("Level {0:0}! Choose one", (float)lvl);
    }

    // Button OnClick: 0 = HP, 1 = Def, 2 = Atk, 3 = Speed (the order of the Stat enum)
    public void Choose(int stat)
    {
        if (_pending == 0) return; // avoid double tap
        stats.Apply((Stat)stat);
        _pending--;
        if (_pending > 0) { ShowTitle(); return; }
        panel.SetActive(false);
        LifecycleGuard.RemoveHold();
    }
}