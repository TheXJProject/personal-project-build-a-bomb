using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    public int frontOrder = 10, backOrder = 2;
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    private void OnEnable()
    {
        TaskStatus.onTaskSelected += ToggleDarkOrder;
        TaskStatus.onTaskSelected += ToggleDarkness;
        TaskStatus.onTaskDeSelected += ToggleDarkness;
        TaskStatus.onTaskBegan += ToggleDarkOrder;
        TaskStatus.onTaskFailed += ToggleDarkOrder;
        TaskStatus.onTaskCompleted += ToggleDarkness;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskSelected -= ToggleDarkOrder;
        TaskStatus.onTaskSelected -= ToggleDarkness;
        TaskStatus.onTaskDeSelected -= ToggleDarkness;
        TaskStatus.onTaskBegan -= ToggleDarkOrder;
        TaskStatus.onTaskFailed -= ToggleDarkOrder;
        TaskStatus.onTaskCompleted -= ToggleDarkness;
    }

    void ToggleDarkness(GameObject task)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void ToggleDarkOrder(GameObject task)
    {
        canvas.sortingOrder = frontOrder;
    }

}
