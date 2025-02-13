using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
