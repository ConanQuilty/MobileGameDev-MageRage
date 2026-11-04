using UnityEngine;
using UnityEngine.SceneManagement;

// Way of sending
public struct RunResult
{
    public bool won;
    public float timeSurvived;
    public string cause;
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

    void Awake() => Instance = this;
    void OnEnable()  => LifecycleGuard.PausedChanged += OnPausedChanged;
    void OnDisable() => LifecycleGuard.PausedChanged -= OnPausedChanged;
    void Start() => SetState(State.Playing);

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
        LastResult = new RunResult { won = result == State.Won, timeSurvived = Elapsed, cause = cause };
        SetState(result);
        SceneManager.LoadSceneAsync("Result");
    }

    void SetState(State s)
    {
        if (Current == s) return; 
        Current = s;
        StateChanged?.Invoke(s);
    }
}

