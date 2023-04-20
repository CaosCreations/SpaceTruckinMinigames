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

    private int timeLeft
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

    private IEnumerator timerCoroutine;

    public Action<int> TimerUpdatedEvent;

    private bool timerRunning = false;

    private void Awake()
    {
        gridManager.GameEventUpdatedEvent += UpdateTimer;
    }

    private void UpdateTimer(GameState gameState)
    {
        if (gameState.CurrentState == "new game")
            StartTimer();

        else if (gameState.CurrentState == "full win" || 
                 gameState.CurrentState == "partial win" || 
                 gameState.CurrentState == "lose")
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
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerRunning = false;
        }   
    }

    private IEnumerator RunTimer()
    {
        timerRunning = true;

        timeLeft = timerDuration;

        while(timeLeft > 0) 
        {
            yield return new WaitForSeconds(1f);

            timeLeft -= 1;
        }

        timerRunning = false;
    }
}
