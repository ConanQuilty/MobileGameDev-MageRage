using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    bool _loading;   // stops a double tap from loading Game twice

    public void OnPlay()
    {
        if (_loading) return;
        _loading = true;
        SceneManager.LoadSceneAsync("Game");
    }
}