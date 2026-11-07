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

    void Start()
    {
        var r = GameManager.LastResult;
        title.SetText(r.won ? "You survived!" : "Defeated");
        float k = r.kills;

        int s = Mathf.FloorToInt(r.timeSurvived);
        int m = s / 60, sec = s % 60; 

        if (sec < 10) detail.SetText("Time {0:0}:0{1:0}   Kills {2:0}", (float)m, (float)sec, k);
        else          detail.SetText("Time {0:0}:{1:0}   Kills {2:0}",  (float)m, (float)sec, k);
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
        _ = SceneLoader.Instance.Load(scene, Application.exitCancellationToken);
    }
}