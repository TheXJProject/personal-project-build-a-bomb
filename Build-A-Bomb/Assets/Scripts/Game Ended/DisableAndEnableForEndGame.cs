using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAndEnableForEndGame : MonoBehaviour
{
    public static event Action<float> onLightsOut;
    public static event Action<float> onCamShakeStart;

    [Header("Game objects to be enabled and disabled upon the game ending")]
    public GameObject gameLosSceneTransition;
    public GameObject gameWonSceneTransition;
    public GameObject[] wonObjects;
    public GameObject[] losObjects;
    public GameObject[] gamObjects;

    [Header("Attributes to change, affecting how the game loss and game win animations looks")]
    [SerializeField] float timeTillExplosion;
    [SerializeField] float timeTillLightsOut;
    [SerializeField] float maxLightsOutRandDelay;

    Coroutine deathTransition;
    Coroutine winTransition;

    private void OnEnable()
    {
        Death.onGameOver += beginDeathTransition;
        BombStatus.onBombFinished += beginWonTransition;
    }

    private void OnDisable()
    {
        Death.onGameOver -= beginDeathTransition;
        BombStatus.onBombFinished -= beginWonTransition;
    }

    // Loss transition logic ================
    void beginDeathTransition()
    {
        if (deathTransition != null) StopCoroutine(deathTransition);
        deathTransition = StartCoroutine(DeathTransition());
    }

    IEnumerator DeathTransition()
    {
        yield return new WaitForSeconds(timeTillLightsOut);     // Wait
        onLightsOut?.Invoke(maxLightsOutRandDelay);             // Turn off the lights
        onCamShakeStart?.Invoke(timeTillExplosion);             // Start the camera shake
        yield return new WaitForSeconds(timeTillExplosion);     // Wait
        gameLosSceneTransition.SetActive(true);                 // Explode
    }


    // Win Transition logic ================
    void beginWonTransition()
    {
        if (winTransition != null) StopCoroutine(winTransition);
        winTransition = StartCoroutine(WinTransition());
    }

    IEnumerator WinTransition()
    {
        yield return new WaitForSeconds(0); // TODO:: Setup variables for timings on the win transition
        gameWonSceneTransition.SetActive(true);
    }

    public void loadEndGameScreen(bool won)
    {
        // First, enable the appropriate endgame stuff
        if (won) EnableGameObjects(wonObjects);
        else EnableGameObjects(losObjects);

        // Then disable all the gameplay gameobjects so that the only thing ingame is the end screen and nothing again
        DisableGameObjects(gamObjects);
    }

    // helper function to enable a list of gameobjects
    void EnableGameObjects(GameObject[] objects)
    {
        foreach (GameObject go in objects)
        {
            go.SetActive(true);
        }
    }
    
    // same as above function except disable
    void DisableGameObjects(GameObject[] objects)
    {
        foreach (GameObject go in objects)
        {
            go.SetActive(false);
        }
    }
}
