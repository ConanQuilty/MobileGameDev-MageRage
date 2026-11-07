using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject settingsPanel;  

    void OnEnable()  { LifecycleGuard.PausedChanged += Show; Show(LifecycleGuard.IsPaused); }
    void OnDisable() { LifecycleGuard.PausedChanged -= Show; }
    void Show(bool paused) {
        panel.SetActive(paused);
        if (!paused) settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (SceneLoader.IsLoading) return; //Don't do mid load
        var kb = Keyboard.current;
        if (kb == null || !kb.escapeKey.wasPressedThisFrame) return; //Was the escape key (back motion on mobile) pressed?

        if (settingsPanel.activeSelf) { CloseSettings(); return; }
        LifecycleGuard.Instance.SetPaused(!LifecycleGuard.IsPaused);
    }

    public void OnResumePressed() => LifecycleGuard.Instance.SetPaused(false);

 
    public void OpenSettings()  => settingsPanel.SetActive(true);
    public void CloseSettings() => settingsPanel.SetActive(false);

    public void SetTextScale(float factor) => TextScale.Factor = factor;

}