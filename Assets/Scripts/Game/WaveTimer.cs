using System;
using System.Threading;
using UnityEngine;

// One spawn rule: which pool, how often (seconds).
[Serializable] public struct SpawnRule { public EnemyPool pool; public float interval; }

// A wave starts at `startTime` seconds into the run and uses its rules until the next wave.
[Serializable] public struct Wave { public float startTime; public SpawnRule[] rules; }

// Spawns enemies just off-screen around the player, wave by wave, following GameManager's clock.
// Awaitable loop instead of a coroutine; the token cancels it when the object is disabled or the app quits.
public class WaveTimer : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    [SerializeField] Transform player;
    [SerializeField] float spawnRadius = 20f; // a little outside the camera's view

    const float Tick = 0.25f; // how often rules are checked
    readonly float[] _timers = new float[8]; //Just a timer for each rule
    CancellationTokenSource _cts;
    public int CurrentWave { get; private set; } = -1;

    void OnEnable()
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken); // cancels on quit
        _ = RunAsync(_cts.Token); // was StartCoroutine(Loop())
    }

    void OnDisable() { _cts.Cancel(); _cts.Dispose(); }

    async Awaitable RunAsync(CancellationToken ct)
    {
        try
        {
            while (true)
            {
                var gm = GameManager.Instance;
                if (gm != null && gm.Current == GameManager.State.Playing)
                {
                    int w = WaveAt(gm.Elapsed);
                    if (w != CurrentWave)
                    {
                        CurrentWave = w;
                        Array.Clear(_timers, 0, _timers.Length);
                        Debug.Log($"[Wave] {w + 1} at {gm.Elapsed:F0}s");   // once per wave
                    }
                    var rules = waves[w].rules;
                    for (int i = 0; i < rules.Length; i++)
                    {
                        _timers[i] += Tick;
                        if (_timers[i] < rules[i].interval) continue;
                        _timers[i] -= rules[i].interval;
                        rules[i].pool.Spawn(EdgePoint());
                    }
                }
                // Scaled time: while paused (timeScale 0) this simply doesn't finish.
                await Awaitable.WaitForSecondsAsync(Tick, ct);
            }
        }
        catch (OperationCanceledException) { }  // normal on disable or quit
    }

    int WaveAt(float t)
    {
        int w = 0;
        for (int i = 0; i < waves.Length; i++) if (t >= waves[i].startTime) w = i;
        return w;
    }

    Vector2 EdgePoint()
    {
        float a = UnityEngine.Random.value * Mathf.PI * 2f;
        return (Vector2)player.position + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * spawnRadius;
    }
}