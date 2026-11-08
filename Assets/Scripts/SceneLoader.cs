using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public static bool IsLoading { get; private set; }

    [SerializeField] Slider bar;
    [SerializeField] GameObject loadingScreen;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        loadingScreen.SetActive(false);
    }

    public async Awaitable Load(string scene, CancellationToken ct)
    {
        if (IsLoading) return;
        IsLoading = true;
        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            loadingScreen.SetActive(true);
            bar.value = 0f;
            var op = SceneManager.LoadSceneAsync(scene);
            op.allowSceneActivation = false; // progress now stops at 0.9
            while (op.progress < 0.9f)
            {
                bar.value = op.progress / 0.9f;
                await Awaitable.NextFrameAsync(ct);
            }
            bar.value = 1f;
            op.allowSceneActivation = true;
            await op; //completes on activation (pools prewarm in Awake here)
        }
        finally
        {
            loadingScreen.SetActive(false); //even if cancelled so it's never a stuck loading screen
            IsLoading = false;
        }
        Debug.Log($"[perf] scene {scene} loaded in {sw.ElapsedMilliseconds} ms");
    }
}