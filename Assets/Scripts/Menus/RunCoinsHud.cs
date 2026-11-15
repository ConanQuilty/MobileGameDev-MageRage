using TMPro;
using UnityEngine;

public class RunCoinsHud : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    int _shown = -1;

    void LateUpdate()
    {
        var gm = GameManager.Instance;
        if (gm == null || gm.RunCoins == _shown) return;
        _shown = gm.RunCoins;
        label.SetText("{0:0} coins earned", (float)_shown);
    }
}