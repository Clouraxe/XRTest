using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int targetsLeft = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Stopwatch _stopWatch;
    [SerializeField] private Transform _tools;
    [SerializeField] private Transform _targets;
    void Start()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        targetsLeft = targets.Length;

        // Keep track of targets
        foreach (Transform go in _targets) {
            var t = go.GetComponent<Target>();
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

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTargetPopped()
    {
        Debug.Log("pop goes the weasel");
        targetsLeft = _targets.childCount - 1; // minus one because the targets doesn't get destroyed instantly.
        
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


    private void GoToNextLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
            int lvl = int.Parse(Regex.Match(sceneName, @"\d+").Value);
            string nextLvlName = "Level " + (lvl + 1);
            
            try {
                SceneManager.LoadScene(nextLvlName);
            } 
            catch (Exception) 
            {
                Debug.Log("I guess we ran out bucko");
            }
    }
}
