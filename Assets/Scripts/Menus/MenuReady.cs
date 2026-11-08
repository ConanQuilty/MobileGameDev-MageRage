using UnityEngine;

public class MenuReady : MonoBehaviour
{
    static bool s_logged;   //static: returning to Menu later doesn't log again

    void Update()
    {
        if (!s_logged)
        {
            s_logged = true;
            Debug.Log($"[perf] interactive {Time.realtimeSinceStartup:F2}s");
        }
        enabled = false;
    }
}