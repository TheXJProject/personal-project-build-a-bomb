using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInteractStatus : MonoBehaviour
{
    public TaskStatus task;
    public bool isBeingSolved;

    public void SetTaskCompletion(float amount) // Function should be ran whenever the player progresses task completion
    {
        task.taskCompletion = amount;
    }

    public void TaskCompleted() // Function should be ran when task has been completed
    {
        task.TaskCompleted();
    }

    private void Update()
    {
        isBeingSolved = task.isBeingSolved; // Task should not be able to be solved if isBeingSolved is false
    }
}
