using TMPro;
using UnityEngine;

// Shows the time left as m:ss. Touches the text only when the second changes,
// and uses SetText with numbers, so it never builds a string (0 B per frame).
public class RunTimerHud : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    int _shown = -1;

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;
        int secs = Mathf.CeilToInt(gm.Remaining);
        if (secs == _shown) return;
        _shown = secs;


        int m = secs / 60;   // whole minutes (int division: 125 / 60 = 2)
        int s = secs % 60;   // seconds left over (125 % 60 = 5)
        // {0:0} = "0 decimal places". TMP can't zero-pad, so the "0" before {1} is typed by hand.
        if (s < 10) label.SetText("{0:0}:0{1:0}", (float)m, (float)s);
        else        label.SetText("{0:0}:{1:0}",  (float)m, (float)s);
    }
}