using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int targetsLeft = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        targetsLeft = targets.Length;

        foreach (GameObject go in targets) {
            var t = go.GetComponent<Target>();
            t.OnPop += OnTargetPopped;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTargetPopped()
    {
        Debug.Log("pop goes the weasel");
        targetsLeft--;
        
        // Finished the level!
        if (targetsLeft == 0) {
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
}
