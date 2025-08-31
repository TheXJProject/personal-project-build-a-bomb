using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    // Event Actions:
    public static Action onTimerZero;

    // Inspector Adjustable Values:
    [SerializeField] int startingMinutes = 5;
    [SerializeField] float startingSeconds = 0.0f;
    [SerializeField] float timerSpeedIncrease = 2.0f;

    // Initialise In Inspector:
    [SerializeField] TextMeshProUGUI timerText;

    // Runtime Variables:
    bool timerRunning = false;
    int minutesLeft;
    float secondsLeft;
    float currentTimerSpeed = 1.0f;
    Color originalCol;

    private void Awake()
    {
        originalCol = timerText.color;
        minutesLeft = startingMinutes;
        secondsLeft = startingSeconds;
        DisplayTime();
    }

    private void OnEnable()
    {
        BombStatus.onLayerCreated += BeginTimer;
        BombStatus.onGoingWrongCheck += DetermineTimerAggression;
    }

    private void OnDisable()
    {
        BombStatus.onLayerCreated -= BeginTimer;
        BombStatus.onGoingWrongCheck -= DetermineTimerAggression;
    }

    private void Update()
    {
        if (timerRunning)
        {
            secondsLeft -= Time.deltaTime * currentTimerSpeed;
            if (secondsLeft < 0.0f)
            {
                if (minutesLeft == 0)
                {
                    onTimerZero?.Invoke();
                    timerRunning = false;
                }
                else
                {
                    --minutesLeft;
                    secondsLeft += 60.0f;
                    DisplayTime();
                }
            }
            else
            {
                DisplayTime();
            }
        }
    }

    public void BeginTimer(GameObject triggerLayer)
    {
        timerRunning = true;
        BombStatus.onLayerCreated -= BeginTimer;
    }

    void DetermineTimerAggression(bool tasksGoingWrongNotBeingSolved)
    {
        if (tasksGoingWrongNotBeingSolved)
        {
            currentTimerSpeed = timerSpeedIncrease;
            timerText.color = Color.red;
        }
        else
        {
            currentTimerSpeed = 1.0f;
            timerText.color = originalCol;
        }
    }

    void DisplayTime()
    {
        timerText.text = string.Format("{0:00}:{1:00}", minutesLeft, (int)secondsLeft);
    }
}
