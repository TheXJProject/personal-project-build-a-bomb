using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInteractStatus : MonoBehaviour
{
    // Event actions calls
    public static event Action<GameObject> onTaskFailed; // Function should be made that resets the task when this action is called
    public static event Action<GameObject> onTaskDifficultySet; // Function should be made that sets the task difficulty when this action is called

    public TaskStatus task;
    public bool isBeingSolvedAndSelected;
    public bool isBeingSolved;
    public bool isBeingSelected;

    private void OnEnable()
    {
        TaskStatus.onTaskFailed += TaskFailed;
        TaskStatus.onTaskGoneWrong += TaskFailed;
        LayerStatus.onTaskCreated += SetTaskDifficulty;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskFailed -= TaskFailed;
        TaskStatus.onTaskGoneWrong -= TaskFailed;
        LayerStatus.onTaskCreated -= SetTaskDifficulty;
    }

    public void SetTaskCompletion(float amount) // Function should be ran whenever the player progresses task completion (between 0 and 1)
    {
        task.taskCompletion = amount;
    }

    public void TaskCompleted() // Function should be ran when task has been completed
    {
        task.TaskCompleted();
    }

    public void SetTaskDifficulty(GameObject triggerTask)
    {
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            onTaskDifficultySet?.Invoke(triggerTask);
        }
    }
    
    public void TaskFailed(GameObject trigger)
    {
        if (trigger.transform.GetChild(0).gameObject == gameObject)
        {
            onTaskFailed?.Invoke(gameObject);
        }
    }

    private void Update()
    {
        isBeingSolvedAndSelected = task.isBeingSolved && task.isSelected; // Task should not be able to be solved if isBeingSolved is false
        isBeingSolved = task.isBeingSolved;
        isBeingSelected = task.isSelected;
    }
}
