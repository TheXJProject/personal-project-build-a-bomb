using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{
    private void OnEnable()
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

    void DisplayTask(GameObject task)
    {
        if (task == gameObject)
        {
            task.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    void UnDisplayTask(GameObject task)
    {
        if (task == gameObject)
        {
            task.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
