using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInteractStatus : MonoBehaviour
{
    // Function that resets the task should be activated whenever the following event is called
    public static event Action onTaskFailed;

    public TaskStatus task;
    public bool isBeingSolved;

    private void OnEnable()
    {
        TaskStatus.onTaskFailed += TaskFailed;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskFailed -= TaskFailed;
    }

    public void SetTaskCompletion(float amount) // Function should be ran whenever the player progresses task completion (between 0 and 1)
    {
        task.taskCompletion = amount;
    }

    public void TaskCompleted() // Function should be ran when task has been completed
    {
        task.TaskCompleted();
    }

    public void TaskFailed(GameObject gameObject)
    {
        onTaskFailed?.Invoke();
    }

    private void Update()
    {
        isBeingSolved = task.isBeingSolved; // Task should not be able to be solved if isBeingSolved is false
    }
}
