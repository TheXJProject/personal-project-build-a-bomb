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
    [SerializeField] float iFramesLength = 0.03f;

    // Runtime variables
    public bool hardMode = false;
    bool invisible = false;
    int livesLeft;
    int bulbsNotBlown;
    Coroutine iFrames;

    private void Awake()
    {
        if (hardMode)
        {
            livesLeft = 1;
            lives[1].SetActive(false);
            lives[2].SetActive(false);
        }
        else livesLeft = 3;
        bulbsNotBlown = livesLeft;
    }

    private void OnEnable()
    {
        TaskStatus.onTaskFailed += loseLife;
        LivesBulbBlown.onBulbBlown += checkFinalBulbBlown;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskFailed -= loseLife;
        LivesBulbBlown.onBulbBlown -= checkFinalBulbBlown;
    }

    public void loseLife(GameObject triggerTask)
    {
        if (livesLeft == 0) return;
        if (!invisible) 
        { 
            lives[--livesLeft].GetComponent<Animator>().SetBool("blown", true); 
            if (iFrames != null) StopCoroutine(iFrames);
            iFrames = StartCoroutine(BeginIFrames());
        }
    }

    IEnumerator BeginIFrames()
    {
        invisible = true;
        yield return new WaitForSeconds(iFramesLength);
        invisible = false;
    }

    void checkFinalBulbBlown()
    {
        if ((--bulbsNotBlown) != 0) return;
        if (livesLeft == 0)
        {
            onNoLives?.Invoke();
        }
    }
}
