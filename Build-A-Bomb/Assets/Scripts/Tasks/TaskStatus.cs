using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStatus : MonoBehaviour
{
    // Events from this class
    public static event Action<GameObject> onTaskSelected;
    public static event Action<GameObject> onTaskDeSelected;

    // Shared between all tasks
    public static bool AnyTaskFocused;

    // Current state of the task
    public bool isSolved = false;
    public bool isBeingSolved = false;
    public bool isSelected = false;
    public float percentComplete = 0;

    // To be set from outside sources 
    public List<int> keys = new List<int>(); // letters of the alphabet are assigned between 0 and 25 for A to Z

    public bool TaskSelected()
    {
        if (AnyTaskFocused) return false;
        onTaskSelected?.Invoke(gameObject);

        isSelected = true;
        AnyTaskFocused = true;

        return true;
    }

    public bool TaskDeselected()
    {
        if (!isSelected) return false;
        onTaskDeSelected?.Invoke(gameObject);

        isSelected = false;
        AnyTaskFocused = false;

        return true;
    }

    public void SetKeysRequired(List<int> newKeys)
    {
        keys.Clear();

        foreach (var key in newKeys)
        {
            keys.Add(key);
        }
    }
}
