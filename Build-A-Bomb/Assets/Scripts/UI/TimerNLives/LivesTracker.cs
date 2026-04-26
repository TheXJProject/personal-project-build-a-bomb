using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesTracker : MonoBehaviour
{
    // Event Actions
    public static Action onNoLives;
    public static Action onFuseBeginsToBlow;

    // Initialise In Inspector
    [SerializeField] List<GameObject> lives;
    [SerializeField] float iFramesLength = 0.03f;

    // Runtime variables
    public bool hardMode = false;
    public bool devInvun = false;
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
        LivesBulbBlown.onFuseBlown += checkFinalBulbBlown;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskFailed -= loseLife;
        LivesBulbBlown.onFuseBlown -= checkFinalBulbBlown;
    }

    public void loseLife(GameObject triggerTask)
    {
        if (devInvun) return;
        if (livesLeft == 0) return;
        if (!invisible) 
        {
            onFuseBeginsToBlow?.Invoke();
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
