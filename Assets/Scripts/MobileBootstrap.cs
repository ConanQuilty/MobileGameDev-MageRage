using UnityEngine;
using UnityEngine.SceneManagement;
public class MobileBootstrap : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;            // Android defaults to 30 without this
        QualitySettings.vSyncCount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(gameObject);

        Debug.Log($"[Boot] {SystemInfo.deviceModel} | " +
                  $"{SystemInfo.operatingSystem} | " +
                  $"{SystemInfo.graphicsDeviceType} | " +
                  $"{Screen.width}x{Screen.height} @ {Screen.dpi} dpi | " +
                  $"RAM {SystemInfo.systemMemorySize} MB | " +
                  $"target {Application.targetFrameRate} fps");

        Telemetry.Log("session_start",
            ("app_version", Application.version),
            ("device_model", SystemInfo.deviceModel),
            ("android", SystemInfo.operatingSystem));
    }

    async Awaitable Start() => await SceneLoader.Instance.Load("Menu", Application.exitCancellationToken);
}
