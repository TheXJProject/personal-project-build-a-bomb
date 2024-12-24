using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskFade : MonoBehaviour
{
    // Initialise In Inspector:
    public int frontOrder = 10, backOrder = 2;

    // Runtime Variables:
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void OnEnable() // Task fade and the ordering of the fade should be toggled in all the situations that tasks are selected or deselected
    {
        TaskStatus.onTaskSelected += ToggleFadeOrder;
        TaskStatus.onTaskSelected += ToggleFade;
        TaskStatus.onTaskDeSelected += ToggleFade;
        TaskStatus.onTaskBegan += ToggleFadeOrder;
        TaskStatus.onTaskFailed += ToggleFadeOrder;
        TaskStatus.onTaskCompleted += ToggleFade;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskSelected -= ToggleFadeOrder;
        TaskStatus.onTaskSelected -= ToggleFade;
        TaskStatus.onTaskDeSelected -= ToggleFade;
        TaskStatus.onTaskBegan -= ToggleFadeOrder;
        TaskStatus.onTaskFailed -= ToggleFadeOrder;
        TaskStatus.onTaskCompleted -= ToggleFade;
    }

    /// <summary>
    /// Fade is toggled on or off depending on whether the task is selected or (deselected or solved)
    /// </summary>
    void ToggleFade(GameObject task)
    {
        if (task.GetComponent<TaskStatus>().isSelected)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (!task.GetComponent<TaskStatus>().isSelected || task.GetComponent<TaskStatus>().isSolved)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Fade is sorted to appear in front or behind the task depending on whether it is being solved.  <br/>
    /// (If the task is being solved but isn't selected, then it shouldn't affect the fade, and so doesn't do anything)
    /// </summary>
    void ToggleFadeOrder(GameObject task)
    {
        if (task.GetComponent<TaskStatus>().isBeingSolved)
        {
            canvas.sortingOrder = backOrder;
        }
        else if (!task.GetComponent<TaskStatus>().isBeingSolved && task.GetComponent<TaskStatus>().isSelected)
        {
            canvas.sortingOrder = frontOrder;
        }
    }

}
