using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCount : MonoBehaviour
{
    [Header("Initialise in inspector:")]
    [SerializeField] TextMeshProUGUI text;

    [Header("The pause before the countdown begins \n " +
            "and the countdown length itself:")]
    [SerializeField] float briefPauseLength = 0.5f;
    [SerializeField] int countDownLength = 3;

    public static event Action onCountdownFinished;
    public static event Action<MUSIC_TRACKS, bool, double> startGamplay1Music;


    bool gameStarted = false;

    private void OnEnable()
    {
        GameManager.onLevelFinshedLoading += BeginGameStartVisual;
    }

    private void Start()
    {
        startGamplay1Music.Invoke(MUSIC_TRACKS.GAMEPLAY1, true, briefPauseLength);

        if (GameManager.instance.gameplayStartsFromWithinGameplayScene) BeginGameStartVisual();
    }

    private void OnDisable()
    {
        GameManager.onLevelFinshedLoading -= BeginGameStartVisual;
    }

    void BeginGameStartVisual()
    {
        if (gameStarted) return;
        gameStarted = true;

        if (CheatLogic.cheatTool.GetNoCountdown())
        {
            GameStart();
            return;
        }

        StartCoroutine(BriefPause());
    }

    IEnumerator BriefPause()
    {
        yield return new WaitForSeconds(briefPauseLength);
        StartCoroutine(CountdownBegin());
    }

    IEnumerator CountdownBegin()
    {
        for (int i = countDownLength; i > 0; --i)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        GameStart();
    }

    void GameStart()
    {
        // Fade in the starting tracks
        MixerFXManager.instance.SetMusicParam("Main1 Pt1 FX", EX_PARA.VOLUME, 0.01f);
        MixerFXManager.instance.SetMusicParam("Main1 Pt1 Start", EX_PARA.VOLUME, 0.01f);

        onCountdownFinished?.Invoke();
        gameObject.SetActive(false);
    }
}
