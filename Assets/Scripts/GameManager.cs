using UnityEngine;

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

        if (targetsLeft == 0) Debug.Log("yippie!!");
    }
}
