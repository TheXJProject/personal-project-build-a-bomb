using System;
using System.Collections.Generic;
using UnityEngine;


public class TutorialControl : MonoBehaviour
{
    public static event Action onAllowGameplay;
    public static event Action onTutorialStart;
    public static event Action<GameObject> onStopTaskBeingSolvable;
    public static event Action<GameObject> onContinueTaskBeingSolvable;

    [SerializeField] private AnimationCurve bubbleEnterCurve;
    [SerializeField] private float bubbleEnterTime;
    [SerializeField] private AnimationCurve bubbleLeaveCurve;
    [SerializeField] private float bubbleLeaveTime;
    [SerializeField] private float bubbleTextEnterTime;
    [SerializeField] private float bubbleBeginEnterTime;
    [SerializeField] private List<TutorialSpeachBubble> orderedSpeechBubbles;
    [SerializeField] private TutorialTaskGoesWrong goWrongController;

    [Header("The following variable is the percentage to stop progress during the second layer task solve to make a task go wrong")]
    [SerializeField] private float percentStopTaskProgress;

    private int currentBubble = 0;
    private GameObject secondLayerTask;
    private TaskStatus secondLayerTaskStatus;
    private bool stoppedTaskProgress = false;
    private bool gotSecondLayerTask = false;

    private void OnEnable()
    {
        TaskStatus.onTaskCompleted += SolvedGoingWrongTask;
        LayerStatus.onTaskCreated += GetTaskOnSecondLayer;
        LayerStatus.onLayerFinishedSpawning += StartAllowGameplay;
        BombStatus.onBombFinished += FinishedLevel;

        // Subscribe the first tutorial prompt in the list
        TaskStatus.onTaskSelected += FirstClickOnTask;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskCompleted -= SolvedGoingWrongTask;
        LayerStatus.onTaskCreated -= GetTaskOnSecondLayer;
        LayerStatus.onLayerFinishedSpawning -= StartAllowGameplay;
        BombStatus.onBombFinished -= FinishedLevel;

        // Unsubscribe all events that might potentially be subscribed
        TaskStatus.onTaskSelected -= FirstClickOnTask;
        TaskStatus.onTaskBegan -= SecondStartTask;
        TaskStatus.onTaskCompleted -= ThirdSolvedTask;
        LayerStatus.onLayerCompleted -= FourthSolvedLayer;
        TaskStatus.onTaskGoneWrong -= FifthTaskGoesWrong;
        TaskStatus.onTaskDeSelected -= SixthMovedOffTask;
        TaskStatus.onTaskSelected -= SeventhSelectGoneWrongTask;
        TaskStatus.onTaskCompleted -= EighthSolvedGoneWrongTask;

        // Unsubscribe deselect bubble events
        TaskStatus.onTaskSelected -= HideBubbleFromEventCall;
        TaskStatus.onTaskDeSelected -= HideBubbleFromEventCall;
    }

    private void Start()
    {
        onTutorialStart?.Invoke(); // Made this as a separate event from "onAllowGameplay" just to give me the option to distinguish them if I wanna
        foreach (TutorialSpeachBubble bubble in orderedSpeechBubbles)
        {
            bubble.gameObject.SetActive(true);
            bubble.SetHideAnimationCurveParams(bubbleLeaveCurve, bubbleLeaveTime);
            bubble.SetShowAnimationCurveParams(bubbleEnterCurve, bubbleEnterTime);
            bubble.SetTimeForTextToEnter(bubbleTextEnterTime);
            bubble.SetTimeToBeginEnter(bubbleBeginEnterTime);
        }
        ShowCurrentBubble();
    }

    private void Update()
    {
        if (gotSecondLayerTask && !stoppedTaskProgress && secondLayerTaskStatus.taskCompletion >= percentStopTaskProgress)
        {
            stoppedTaskProgress = true;
            onStopTaskBeingSolvable?.Invoke(secondLayerTask);
            goWrongController.MakeTaskGoWrong();
        }
    }

    private void SolvedGoingWrongTask(GameObject task)
    {
        if (task.GetComponent<TaskStatus>().hasBeenSolved)
        {
            onContinueTaskBeingSolvable?.Invoke(secondLayerTask);
        }
    }

    private void GetTaskOnSecondLayer(GameObject task)
    {
        TaskStatus taskStatus;
        if ((taskStatus = task.GetComponent<TaskStatus>()) != null)
        {
            if (taskStatus.taskLayer == 1)
            {
                gotSecondLayerTask = true;  
                secondLayerTask = task;
                secondLayerTaskStatus = taskStatus;
            }
        }
        else
            Debug.LogError("Task doesn't have a TaskStatus script");
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

    private void NextBubble()
    {
        HideCurrentBubble();
        ++currentBubble;
        if (currentBubble >= orderedSpeechBubbles.Count)
        {
            currentBubble = orderedSpeechBubbles.Count - 1;
            Debug.LogWarning("TutorialControl:: Attempting to go to the next tutorial bubble even though the last one has been reached");
        }
    }

    private void ShowCurrentBubble()
    {
        orderedSpeechBubbles[currentBubble].ShowBubble();
    }

    private void HideCurrentBubble()
    {
        orderedSpeechBubbles[currentBubble].HideBubble();
    }

    // Yeah I couldn't be bothered to figure out a better way:
    private void FirstClickOnTask(GameObject task)
    {
        NextBubble();
        ShowCurrentBubble();
        TaskStatus.onTaskSelected -= FirstClickOnTask;
        TaskStatus.onTaskBegan += SecondStartTask;

    }

    private void SecondStartTask(GameObject task)
    {
        NextBubble();
        ShowCurrentBubble();
        TaskStatus.onTaskBegan -= SecondStartTask;
        TaskStatus.onTaskCompleted += ThirdSolvedTask;
    }

    private void ThirdSolvedTask(GameObject task)
    {
        NextBubble();
        ShowCurrentBubble();
        BubbleHidesOnTaskInteraction();
        TaskStatus.onTaskCompleted -= ThirdSolvedTask;
        LayerStatus.onLayerCompleted += FourthSolvedLayer;
    }

    private void FourthSolvedLayer(GameObject layer)
    {
        NextBubble();
        ShowCurrentBubble();
        BubbleHidesOnTaskInteraction();
        LayerStatus.onLayerCompleted -= FourthSolvedLayer;
        TaskStatus.onTaskGoneWrong += FifthTaskGoesWrong;
    }

    private void FifthTaskGoesWrong(GameObject task)
    {
        NextBubble();
        ShowCurrentBubble();
        TaskStatus.onTaskGoneWrong -= FifthTaskGoesWrong;
        TaskStatus.onTaskDeSelected += SixthMovedOffTask;
    }

    private void SixthMovedOffTask(GameObject task)
    {
        NextBubble();
        ShowCurrentBubble();
        TaskStatus.onTaskDeSelected -= SixthMovedOffTask;
        TaskStatus.onTaskSelected += SeventhSelectGoneWrongTask;
    }

    private void SeventhSelectGoneWrongTask(GameObject task)
    {
        NextBubble();
        ShowCurrentBubble();
        TaskStatus.onTaskSelected -= SeventhSelectGoneWrongTask;
        TaskStatus.onTaskCompleted += EighthSolvedGoneWrongTask;
    }

    private void EighthSolvedGoneWrongTask(GameObject task)
    {
        NextBubble();
        ShowCurrentBubble();
        BubbleHidesOnTaskInteraction();
        TaskStatus.onTaskCompleted -= EighthSolvedGoneWrongTask;
    }

    private void BubbleHidesOnTaskInteraction()
    {
        TaskStatus.onTaskSelected += HideBubbleFromEventCall;
        TaskStatus.onTaskDeSelected += HideBubbleFromEventCall;
    }

    private void HideBubbleFromEventCall(GameObject task)
    {
        HideCurrentBubble();
        TaskStatus.onTaskSelected -= HideBubbleFromEventCall;
        TaskStatus.onTaskDeSelected -= HideBubbleFromEventCall;
    }
}
