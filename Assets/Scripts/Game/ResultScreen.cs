using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Shows how the run ended. Retry replays, Menu (or Android back) goes up one level.
// No Quit button: mobile apps don't have one.
public class ResultScreen : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text detail;
    bool _leaving;

    void Start()
    {
        var r = GameManager.LastResult;
        title.SetText(r.won ? "You survived!" : "Defeated");
        int s = Mathf.FloorToInt(r.timeSurvived);
        if (s % 60 < 10) detail.SetText("Time {0}:0{1}", (float)(s / 60), (float)(s % 60));
        else             detail.SetText("Time {0}:{1}", (float)(s / 60), (float)(s % 60));
    }

    void Update()
    {
        var kb = Keyboard.current; 
        if (kb != null && kb.escapeKey.wasPressedThisFrame) OnMenu();
    }

    public void OnRetry() => Go("Game");
    public void OnMenu()  => Go("Menu");

    void Go(string scene)
    {
        if (_leaving) return;                      // ignore double taps
        _leaving = true;
        SceneManager.LoadSceneAsync(scene);
    }
}