using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInteractStatus : MonoBehaviour
{
    public TaskStatus task;

    public void SetTaskCompletion(float amount)
    {
        task.taskCompletion = amount;
    }

    public void TaskCompleted()
    {
        task.TaskCompleted();
    }
}
