using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    //this script is not actually in use, it is an overkill timer.
    private Text timerDisplay;
    public static TimerController instance;

    private TimeSpan timePlaying;
    private bool timerOn = false;

    private float elapsedTime;

    private void Awake()
    {
        BeginTimer();         
    }

    private void Start()
    {
        instance = this;
        timerDisplay.text = "Alive for: 00:00.00";
    }

    public void BeginTimer()
    {
        timerOn = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerOn = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerOn)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Alive for: " + timePlaying.ToString("mm':'ss'.'ff");
            timerDisplay.text = timePlayingStr;
            yield return null;
        }
    }

}
