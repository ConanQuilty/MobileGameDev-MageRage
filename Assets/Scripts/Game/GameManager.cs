using UnityEngine;
using UnityEngine.SceneManagement;

// Way of sending
public struct RunResult
{
    public bool won;
    public float timeSurvived;
    public string cause;
    public int kills;
    public int coins;
}

public class GameManager : MonoBehaviour
{
    public enum State { Ready, Playing, Paused, Won, Lost }

    public static GameManager Instance { get; private set; }
    public static RunResult LastResult;            // read by the Result scene

    [SerializeField] float runLength = 180f;       // survive this long to win

    public State Current { get; private set; } = State.Ready;
    public float Elapsed { get; private set; }
    public float Remaining => Mathf.Max(0f, runLength - Elapsed);
    public event System.Action<State> StateChanged;
    public int Kills { get; private set; }
    public int RunCoins { get; private set; }
    static int _attempt = 0;

    void Awake() => Instance = this;
    void OnEnable() 
    { 
        LifecycleGuard.PausedChanged += OnPausedChanged;
        Enemy.Killed += OnKilled;
    }

    void OnDisable()
    {
        LifecycleGuard.PausedChanged -= OnPausedChanged;
        Enemy.Killed -= OnKilled;
    }
    void Start() 
    {
        SetState(State.Playing);
        _attempt++;
        Telemetry.Log("level_start", ("level_id", 1), ("attempt", _attempt),
            ("coins", Wallet.Instance != null ? Wallet.Instance.Coins : 0));
    }

    void Update()
    {
        if (Current != State.Playing) return; //Not playing, skip
        Elapsed += Time.deltaTime;           
        if (Elapsed >= runLength) Win();
    }


    void OnPausedChanged(bool paused)
    {
        if (paused && Current == State.Playing) SetState(State.Paused);
        else if (!paused && Current == State.Paused) SetState(State.Playing);
    }
    

    public void Win() => End(State.Won, "none");
    public void Lose(string cause) => End(State.Lost, cause);   // cause: short id, e.g. "hp"

    void End(State result, string cause)
    {
        if (Current == State.Won || Current == State.Lost) return;   // end only once
        LastResult = new RunResult { won = result == State.Won, timeSurvived = Elapsed, cause = cause, kills = Kills, coins = RunCoins };
        SetState(result);
        Wallet.Instance.Earn(RunCoins);
        if (result == State.Won)
        Telemetry.Log("level_complete", ("level_id", 1), ("time_s", Elapsed.ToString("F0")),
            ("coins_earned", RunCoins), ("score", Kills));
        else
        Telemetry.Log("level_fail", ("level_id", 1), ("time_s", Elapsed.ToString("F0")),
            ("cause", cause), ("coins_earned", RunCoins));
        _ = SceneLoader.Instance.Load("Result", Application.exitCancellationToken);
    }

    void SetState(State s)
    {
        if (Current == s) return; 
        Current = s;
        StateChanged?.Invoke(s);
    }

    void OnKilled(Enemy e) 
    {
        Kills++;
        RunCoins += e.Type.coins;
    }
}

