using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TaskStatus : MonoBehaviour
{
    // Event Actions:
    public static event Action<GameObject> onTaskSelected;
    public static event Action<GameObject> onTaskDeSelected;
    public static event Action<GameObject> onTaskBegan;
    public static event Action<GameObject> onTaskFailed;
    public static event Action<GameObject> onTaskCompleted;
    public static event Action<GameObject> onTaskGoneWrong;
    public static event Action<GameObject> onKeyDecided;

    // Static values:
    public static bool AnyTaskFocused;
    public static bool AnyTaskBeingSolved;

    // Current state of the task
    public bool hasBeenSolved = false;
    public bool isSolved = false;
    public bool isBeingSolved = false;
    public bool isSelected = false;
    public bool isGoingWrong = false;
    public bool isOnCurrentLayer = false;
    public float taskCompletion = 0f;
    public List<int> keys = new List<int>(); // letters of the alphabet are assigned between 0 and 25 for A to Z
    public int taskLayer;
    public float difficulty;
    
    private void OnEnable()
    {
        PlayerKeyInput.onKeyPressed += CheckKeysHeld;
        PlayerKeyInput.onKeyReleased += CheckKeysReleased;
    }

    private void OnDisable()
    {
        PlayerKeyInput.onKeyPressed -= CheckKeysHeld;
        PlayerKeyInput.onKeyReleased -= CheckKeysReleased;
    }

    /// <summary>
    /// Sets the keys of a task to the keys given as a parameter
    /// </summary>
    public void SetKeysRequired(List<int> newKeys) 
    {
        keys.Clear();

        foreach (var key in newKeys)
        {
            keys.Add(key);
            onKeyDecided?.Invoke(gameObject);
        }
    }

    /// <summary>
    /// Called when the task is selected and correctly sets the status of the task
    /// </summary>
    public bool TaskSelected()
    {
        if (AnyTaskFocused || isSolved) return false;

        // If dev cheatmode is on 
        if (CheatLogic.cheatTool.GetCanCheat())
        {
            // Autocomplete task
            TaskCompleted();
            return false;
        }

        isSelected = true;
        AnyTaskFocused = true;

        onTaskSelected?.Invoke(gameObject);
        return true;
    }

    /// <summary>
    /// Called when the task is deselected and correctly sets the status of the task
    /// </summary>
    public bool TaskDeselected()
    {
        if (!isSelected) return false;

        isSelected = false;
        AnyTaskFocused = false;

        onTaskDeSelected?.Invoke(gameObject);
        return true;
    }

    /// <summary>
    /// Called when the task is completed and correctly sets the status of the task
    /// </summary>
    public void TaskCompleted() 
    {
        AnyTaskFocused = false;

        isSolved = true;
        isBeingSolved = false;
        isSelected = false;
        isGoingWrong = false;
        taskCompletion = 1f;

        onTaskCompleted?.Invoke(gameObject);
        
        hasBeenSolved = true;
    }

    /// <summary>
    /// Called when the task has gone wrong and correctly sets the status of the task
    /// </summary>
    public void TaskGoneWrong() 
    {
        isSolved = false;
        isGoingWrong = true;
        taskCompletion = 0f;

        onTaskGoneWrong?.Invoke(gameObject);
    }

    /// <summary>
    /// Checks that the keys for this task are all being held and is called whenever a key is pressed
    /// </summary>
    public void CheckKeysHeld(int keyJustPressed)
    {
        if (isSelected)
        {
            foreach (int key in keys)
            {
                if (PlayerKeyInput.instance.keysDown[key] == 0) { return; }
            }
            isBeingSolved = true;
            onTaskBegan?.Invoke(gameObject);
        }
    }

    /// <summary>
    /// Checks if any of the keys for this task have been released and is called whenever a key is released
    /// </summary>
    public void CheckKeysReleased(int keyJustReleased)
    {
        if (isBeingSolved)
        {
            foreach (int key in keys)
            {
                if (PlayerKeyInput.instance.keysDown[key] == 1) { return; }
            }
            isBeingSolved = false;
            onTaskFailed?.Invoke(gameObject);
        }
    }
}
