using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private int timerDuration;

    private int _timeLeft;

    private int TimeLeft
    {
        get
        { 
            return _timeLeft; 
        }

        set
        {
            _timeLeft = value;
            TimerUpdatedEvent(_timeLeft);
        }
    }


    private bool timerRunning = false;

    private IEnumerator timerCoroutine;

    public Action<int> TimerUpdatedEvent;

    private void Awake()
    {
        gridManager.GameEventUpdatedEvent += UpdateTimer;
    }

    private void UpdateTimer(GameState gameState)
    {
        if (gameState.CheckCurrentState("new game"))
            StartTimer();
        else if (gameState.CheckCurrentState(new List<string>() { "full win", "partial win", "lose" }) == true)
        {
            StopTimer();
        }
    }

    public void StartTimer()
    {
        if(timerRunning == true)
        {
            Debug.LogError("The timer was already running and you are trying to start it. " +
                           "This is not allowed.");
            return;
        }

        timerCoroutine = RunTimer();

        StartCoroutine(timerCoroutine);
    }

    public void StopTimer()
    {
        if(timerRunning == true && timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerRunning = false;
        }   
    }

    private IEnumerator RunTimer()
    {
        timerRunning = true;

        TimeLeft = timerDuration;

        while(TimeLeft > 0) 
        {
            yield return new WaitForSeconds(1f);

            TimeLeft -= 1;
        }

        timerRunning = false;
    }
}
