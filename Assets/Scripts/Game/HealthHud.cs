using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthHud : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    [SerializeField] Image fill;
    [SerializeField] TMP_Text label;

    void OnEnable()  { health.Changed += Refresh; Refresh(health.Current, health.Max); }
    void OnDisable() { health.Changed -= Refresh; }

    void Refresh(int current, int max)
    {
        fill.fillAmount = max > 0 ? (float)current / max : 0f;
        label.SetText("{0:0}/{1:0}", (float)current, (float)max);
    }
}