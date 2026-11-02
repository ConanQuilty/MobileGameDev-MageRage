using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Sits on the Settings panel. Shows each toggle's saved value without firing
// its event, saves changes straight to PlayerPrefs (save on change), and
// prints an About line so the device model is visible in screenshots.
public class SettingsPanel : MonoBehaviour
{
    const string ShakeKey = "screenShake";

    // Read by camera shake later (W5B feedback). Default on.
    public static bool ShakeEnabled => PlayerPrefs.GetInt(ShakeKey, 1) == 1;

    [SerializeField] Toggle hapticsToggle;
    [SerializeField] Toggle shakeToggle;
    [SerializeField] TMP_Text aboutLabel;

    void Awake()
    {
        aboutLabel.text = $"{Application.productName} {Application.version} · " +
                          $"{SystemInfo.deviceModel} · {SystemInfo.operatingSystem}";
    }

    void OnEnable()
    {
        // SetIsOnWithoutNotify: show the stored value without calling the
        // handlers (otherwise opening Settings would buzz every time).
        hapticsToggle.SetIsOnWithoutNotify(Haptics.Enabled);
        shakeToggle.SetIsOnWithoutNotify(ShakeEnabled);
    }

    public void OnHapticsChanged(bool on)
    {
        Haptics.Enabled = on;   // Haptics saves to PlayerPrefs itself
        if (on) Haptics.Pulse(); // one preview buzz so the player knows it works
    }

    public void OnShakeChanged(bool on)
    {
        PlayerPrefs.SetInt(ShakeKey, on ? 1 : 0);
        PlayerPrefs.Save();
    }
}