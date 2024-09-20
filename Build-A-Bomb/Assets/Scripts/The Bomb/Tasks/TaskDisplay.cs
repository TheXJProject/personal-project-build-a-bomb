using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{
    private void OnEnable() // Tasks displays are enabled/disabled when the play selected/unselects the task - Additionally they are disabled when the task is complete
    {
        TaskStatus.onTaskCompleted += UnDisplayTask;
        TaskStatus.onTaskSelected += DisplayTask;
        TaskStatus.onTaskDeSelected += UnDisplayTask;
    }
    private void OnDisable()
    {
        TaskStatus.onTaskCompleted -= UnDisplayTask;
        TaskStatus.onTaskSelected -= DisplayTask;
        TaskStatus.onTaskDeSelected -= UnDisplayTask;
    }

    void DisplayTask(GameObject task) // Canvas to display to player child gameobject of the child gameobject of the main gameobject for the task
    {
        if (task == gameObject)
        {
            task.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }
    void UnDisplayTask(GameObject task)
    {
        if (task == gameObject)
        {
            task.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }
}
