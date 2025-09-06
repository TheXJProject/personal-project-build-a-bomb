using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    // Event Actions:
    public static Action onTimerZero;

    const float defaultTimerSpeed = 1f;
    const float defaultFasterTimerSpeed = 1.5f;

    // Inspector Adjustable Values:
    [SerializeField] int startingMinutes = 5;
    [SerializeField] float startingSeconds = 0.0f;
    [SerializeField] float timerSpeedIncreaseMax;
    [SerializeField] float timerSpeedIncreaseIncrement;

    // Initialise In Inspector:
    [SerializeField] TextMeshProUGUI timerText;

    // Runtime Variables:
    bool timerRunning = false;
    bool taskGoingWrong = false;
    int minutesLeft;
    float secondsLeft;
    float currentTimerSpeed = defaultTimerSpeed;
    float increasingTimerSpeed = defaultFasterTimerSpeed;
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
            // If the task is going wrong
            if (taskGoingWrong)
            {
                // Steadily increase timer speed up to a threshold
                increasingTimerSpeed = Mathf.Min(increasingTimerSpeed + timerSpeedIncreaseIncrement * Time.deltaTime, timerSpeedIncreaseMax);

                // Apply the calculated timer speed to current
                currentTimerSpeed = increasingTimerSpeed;
            }
            else
            {
                // Reset the timer speed increment
                increasingTimerSpeed = defaultFasterTimerSpeed;

                // Otherwise, use default speed
                currentTimerSpeed = defaultTimerSpeed;
            }

            if (!CheatLogic.cheatTool.GetPauseTimer())
            {
                secondsLeft -= Time.deltaTime * currentTimerSpeed;
            }

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
        // Save whether a task is going wrong
        taskGoingWrong = tasksGoingWrongNotBeingSolved;

        if (tasksGoingWrongNotBeingSolved)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = originalCol;
        }
    }

    void DisplayTime()
    {
        timerText.text = string.Format("{0:00}:{1:00}", minutesLeft, (int)secondsLeft);
    }
}
