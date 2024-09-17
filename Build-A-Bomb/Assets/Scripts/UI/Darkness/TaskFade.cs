using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskFade : MonoBehaviour
{
    public int frontOrder = 10, backOrder = 2;
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void OnEnable()
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

    void ToggleFadeOrder(GameObject task)
    {
        if (task.GetComponent<TaskStatus>().isBeingSolved)
        {
            canvas.sortingOrder = backOrder;
        }
        else if (task.GetComponent<TaskStatus>().isBeingSolved == false)
        {
            canvas.sortingOrder = frontOrder;
        }
    }

}
