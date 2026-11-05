 using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpHud : MonoBehaviour
{
    [SerializeField] PlayerXP xp;
    [SerializeField] Image fill;
    [SerializeField] TMP_Text level;

    void OnEnable()  { xp.XpChanged += Refresh; Refresh(xp.Xp, xp.Next); }
    void OnDisable() { xp.XpChanged -= Refresh; }

    void Refresh(int current, int next)
    {
        fill.fillAmount = next > 0 ? (float)current / next : 0f;
        level.SetText("Lv {0:0}", (float)xp.Level);
    }
}