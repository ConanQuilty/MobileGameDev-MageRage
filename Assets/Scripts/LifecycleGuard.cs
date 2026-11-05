using UnityEngine;
using UnityEngine.SceneManagement;
public class LifecycleGuard : MonoBehaviour
{
    public static LifecycleGuard Instance { get; private set; }
    public static bool IsPaused { get; private set; }
    public static event System.Action<bool> PausedChanged;

    static int _holds;   // things (like the level-up panel) that need time frozen

    void Awake() => Instance = this;
    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    //If scene load make sure it's unpaused
    void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        _holds = 0;
        SetPaused(false);
    }

    // Home, app switch, incoming call, screen off: save HERE.
    void OnApplicationPause(bool paused)
    {
        if (!paused) return;           // resuming is a player choice
        PlayerPrefs.Save();            // swap for your save system
        SetPaused(true);
    }

    // Notification shade, permission dialog, on-screen keyboard.
    void OnApplicationFocus(bool f) { if (!f) SetPaused(true); }

    public void SetPaused(bool value)  // Resume button passes false
    {
        IsPaused = value;
        ApplyTime();  
        AudioListener.pause = value;
        PausedChanged?.Invoke(value);
    }

    public static void AddHold()    { _holds++; ApplyTime(); }
    public static void RemoveHold()
    {
        if (_holds > 0) _holds--; //never below 0
        ApplyTime();
    }
    static void ApplyTime() => Time.timeScale = (IsPaused || _holds > 0) ? 0f : 1f;
}