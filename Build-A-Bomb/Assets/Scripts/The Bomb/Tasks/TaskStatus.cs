using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TaskStatus : MonoBehaviour
{
    // Events from this class
    public static event Action<GameObject> onTaskSelected;
    public static event Action<GameObject> onTaskDeSelected;
    public static event Action<GameObject> onTaskBegan;
    public static event Action<GameObject> onTaskFailed;
    public static event Action<GameObject> onTaskCompleted;
    public static event Action<GameObject> onTaskGoneWrong;

    // Shared between all tasks
    public static bool AnyTaskFocused;
    public static bool AnyTaskBeingSolved;

    // Current state of the task
    public bool isSolved = false;
    public bool isBeingSolved = false;
    public bool isSelected = false;
    public bool isGoingWrong = false;
    public float taskCompletion = 0f;

    // To be set during or after the tasks creation to affect gameplay of task 
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

    public void SetKeysRequired(List<int> newKeys) 
    {
        keys.Clear();

        foreach (var key in newKeys)
        {
            keys.Add(key);
            Debug.Log(PlayerKeyInput.instance.alphabet[key]);
        }
    }

    public bool TaskSelected() // Called when the task is selected and correctly sets the status of the task
    {
        if (AnyTaskFocused || isSolved) return false;

        isSelected = true;
        AnyTaskFocused = true;

        onTaskSelected?.Invoke(gameObject);
        return true;
    }

    public bool TaskDeselected() // Called when the task is deselected and correctly sets the status of the task
    {
        if (!isSelected) return false;

        isSelected = false;
        AnyTaskFocused = false;

        onTaskDeSelected?.Invoke(gameObject);
        return true;
    }

    public void TaskCompleted() // Called when the task is completed and correctly sets the status of the task
    {
        AnyTaskFocused = false;

        isSolved = true;
        isBeingSolved = false;
        isSelected = false;
        isGoingWrong = false;
        taskCompletion = 1f;

        onTaskCompleted?.Invoke(gameObject);
    }

    public void TaskGoneWrong() // Called when the task has gone wrong and correctly sets the status of the task
    {
        isSolved = false;
        isGoingWrong = true;

        onTaskGoneWrong?.Invoke(gameObject);
    }

    public void CheckKeysHeld(int keyJustPressed) // Checks that the keys for this task are all being held and is called whenever a key is pressed
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

    public void CheckKeysReleased(int keyJustReleased) // Checks if any of the keys for this task have been released and is called whenever a key is released
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
