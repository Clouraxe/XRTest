using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Stopwatch _stopWatch;
    [SerializeField] private Transform _tools;

    private int targetsLeft = -1;

    void Start()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        targetsLeft = targets.Length;

        // Keep track of targets
        foreach (GameObject go in targets) {
            var t = go.GetComponentInChildren<Target>();
            t.OnPop += OnTargetPopped;
        }

        // Keep track of items removed from pedestals (for stopwatch)
        foreach (Transform obj in _tools) {
            if (obj.childCount < 1) continue;

            if (obj.GetChild(0).TryGetComponent<ItemPedestalContainer>(out var itemPedestalContainer)) {
                itemPedestalContainer.OnItemRemoved += StartTimer;
            }
        }
    }

    public void OnTargetPopped()
    {
        Debug.Log("pop goes the weasel");
        targetsLeft--; // minus one because the targets doesn't get destroyed instantly.
        // Finished the level!
        if (targetsLeft == 0) {
            _stopWatch.StopTimer();
            Debug.Log($"You finished {SceneManager.GetActiveScene().name} in {_stopWatch.GetTimeString()}!");
            Invoke(nameof(PlayLevelCompleteSound), 1f);
            Invoke(nameof(GoToNextLevel), 3f);
        }
    }

    private void PlayLevelCompleteSound() => GetComponent<AudioSource>().Play();

    public void StartTimer() => _stopWatch.StartTimer();

    public void GoToNextLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int lvl = int.Parse(Regex.Match(sceneName, @"\d+").Value);
        string nextLvlName = "Level " + (lvl + 1);

        try {
            SceneManager.LoadScene(nextLvlName);
            Pooler<Arrow>.Instance.Clear();
        } catch (Exception) {
            Debug.Log("I guess we ran out bucko");
        }
    }
    
    public void RestartLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
