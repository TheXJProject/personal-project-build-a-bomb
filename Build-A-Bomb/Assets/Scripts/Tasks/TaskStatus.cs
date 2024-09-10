using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStatus : MonoBehaviour
{
    public static bool AnyTaskFocused;

    public bool isSolved = false;
    public bool isSelected = false;
    public bool isBeingSolved = false;

    public bool TaskSelected()
    {
        if (AnyTaskFocused) return false;

        isSelected = true;
        AnyTaskFocused = true;

        return true;
    }

    public bool TaskDeselected()
    {
        if (!isSelected) return false;

        isSelected = false;
        AnyTaskFocused = false;

        return true;
    }
}
