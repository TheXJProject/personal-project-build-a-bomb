using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesTracker : MonoBehaviour
{
    // Event Actions
    public static Action onNoLives;

    // Initialise In Inspector
    [SerializeField] List<GameObject> lives;

    // Runtime variables
    public bool hardMode = false;
    int livesLeft;

    private void Awake()
    {
        if (hardMode)
        {
            livesLeft = 1;
            lives[1].SetActive(false);
            lives[2].SetActive(false);
        }
        else livesLeft = 3;
    }

    private void OnEnable()
    {
        TaskStatus.onTaskFailed += loseLife;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskFailed -= loseLife;
    }

    public void loseLife(GameObject triggerTask)
    {
        if (livesLeft == 0) return;
        lives[--livesLeft].GetComponent<Animator>().SetBool("blown", true);
        if (livesLeft == 0)
        {
            onNoLives?.Invoke();
        }
    }
}
