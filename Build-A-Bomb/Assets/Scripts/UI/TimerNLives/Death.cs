using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public static Action onGameOver;

    private void OnEnable()
    {
        DeathTimer.onTimerZero += GameOver;
        LivesTracker.onNoLives += GameOver;
    }

    private void OnDisable()
    {
        DeathTimer.onTimerZero -= GameOver;
        LivesTracker.onNoLives -= GameOver;
    }

    /// <summary>
    /// Called whenever something happens that means the game is over (e.g ran out of lives) <br/>
    /// Invokes the onGameOver event which should begin the gameover scenario
    /// </summary>
    public void GameOver()
    {
        onGameOver?.Invoke();
    }
}
