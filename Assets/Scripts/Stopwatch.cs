using System;
using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    private TextMeshPro text;
    private bool isRunning = false;
    private float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning) {
            time += Time.deltaTime;
            int min = (int)time / 60;
            int sec = (int)time % 60;
            text.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public string GetTimeString() => text.text;

    public int GetMilliSeconds(int digits = 2)
    {
        int tens = (int)Mathf.Pow(10, digits);
        return (int)(time * tens) % tens;
    }

}
