using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInteractStatus : MonoBehaviour
{
    // Event actions calls
    public static event Action<GameObject> onTaskFailed; // Function should be made that resets the task when this action is called
    public static event Action<float> onChangeTaskDifficulty; // Function should be made that sets the task difficulty when this action is called

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

    public void ChangeTaskDifficulty(float difficulty)
    {
        onChangeTaskDifficulty?.Invoke(difficulty);
    }
    
    public void TaskFailed(GameObject gameObject)
    {
        if (gameObject.transform.GetChild(0) == gameObject)
        {
            onTaskFailed?.Invoke(gameObject);
        }
    }

    private void Update()
    {
        isBeingSolved = task.isBeingSolved; // Task should not be able to be solved if isBeingSolved is false
    }
}
