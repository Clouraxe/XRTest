using UnityEngine;
using UnityEngine.SceneManagement;

public class WristMenu : MonoBehaviour
{

    public void OnQuitButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void OnRestartButtonPressed()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
