using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public void OnPlay()
    {
        _ = SceneLoader.Instance.Load("Game", Application.exitCancellationToken);
    }
}