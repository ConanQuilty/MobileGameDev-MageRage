using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] LifecycleGuard guard;

    void OnEnable()  { LifecycleGuard.PausedChanged += Show; Show(LifecycleGuard.IsPaused); }
    void OnDisable() { LifecycleGuard.PausedChanged -= Show; }
    void Show(bool paused) {
        panel.SetActive(paused);
        if (!paused) settingsPanel.SetActive(false);
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null || !kb.escapeKey.wasPressedThisFrame) return; //Was the escape key (back motion on mobile) pressed?

        if (settingsPanel.activeSelf) { CloseSettings(); return; }
        guard.SetPaused(!LifecycleGuard.IsPaused);
    }

    public void OnResumePressed() => guard.SetPaused(false);

    public GameObject settingsPanel;  
 
    public void OpenSettings()  => settingsPanel.SetActive(true);
    public void CloseSettings() => settingsPanel.SetActive(false);

    public void SetTextScale(float factor) => TextScale.Factor = factor;

}