using TMPro;
using UnityEngine;

// Kill counter: the lane's "score change" feedback. Same pattern as RunTimerHud: touch the text only on change.
public class KillCounterHud : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    int _shown = -1;

    void LateUpdate()
    {
        var gm = GameManager.Instance;
        if (gm == null || gm.Kills == _shown) return;
        _shown = gm.Kills;
        label.SetText("Kills {0:0}", (float)_shown);
    }
}