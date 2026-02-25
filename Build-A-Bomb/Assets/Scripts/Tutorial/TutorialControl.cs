using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
    public static event Action onAllowGameplay;
    public static event Action onTutorialStart;

    [SerializeField] List<TutorialSpeachBubble> orderedSpeechBubbles;

    private void OnEnable()
    {
        LayerStatus.onLayerFinishedSpawning += StartAllowGameplay;
        BombStatus.onBombFinished += FinishedLevel;
    }

    private void OnDisable()
    {
        LayerStatus.onLayerFinishedSpawning -= StartAllowGameplay;
        BombStatus.onBombFinished += FinishedLevel;
    }

    private void Start()
    {
        onTutorialStart?.Invoke(); // Made this as a separate event from "onAllowGameplay" just to give me the option to distinguish them if I wanna
    }

    void StartAllowGameplay()
    {
        onAllowGameplay?.Invoke();
    }

    void FinishedLevel()
    {
        // Probably wait a little before just suddenly ejecting the tutorial
        GameManager.instance.MainMenu();
    }

    void TutorialStateControl()
    {
        // when the thing I want to happens, happens, activate the next state
        // So I need to have this listen to a bunch of functions, including a mouse click, maybe abstraced by a layer
        // then have it determine if it is at the right point to continue to the next state (speech bubble pop up)
        // At some point in the tutorial, (ideally mid way through solving the length second layer task
    }
}
