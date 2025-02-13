using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WristMenu : MonoBehaviour
{
    [SerializeField] private InputAction menuInputAction;
    void Start()
    {
        menuInputAction.Enable();
        menuInputAction.performed += (_) => ToggleMenu();
        gameObject.SetActive(false);
    }

    private void ToggleMenu()
    {
        bool on = !gameObject.activeInHierarchy;
        // Only turn on the main menu, and the canvas itself
        transform.GetChild(0).gameObject.SetActive(true);

        for (int i = 1; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        gameObject.SetActive(on);
    }

    public void OnQuitButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void OnRestartButtonPressed()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void OnDestroy()
    {
        menuInputAction.performed -= (_) => ToggleMenu();
    }
}
