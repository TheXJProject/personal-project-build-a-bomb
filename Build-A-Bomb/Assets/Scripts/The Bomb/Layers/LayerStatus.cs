using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerStatus : MonoBehaviour
{
    // To be adjusted as seen fit
    public int noOfTasksSpawned = 4;
    public List<GameObject> typeOfTasks = new List<GameObject>();

    // Stats of the layer
    public bool isSelected = false; // Whether it is the layer the player has currently got selected
    public bool isCompleted = false;
    public bool isGoingWrong = false; // Contains a task that is going wrong
    public bool isBeingSolved = false; // Contains a task that is being solved by the player
    public bool isBeingFocused = false; // Contains the task that is currently dispayed and being solved by the player

    // To be set when spawned
    public int layer;
    public int noOfKeysPerTask = 1;
    public float layerMinRadius = 2f;
    public float layerMaxRadius = 4.75f;
    public float taskRadius = 0.48f;
    public List<GameObject> tasks = new List<GameObject>();

    // Other variables for miscellaneous purposes
    System.Random rnd = new System.Random();
    public int infiniteLoopPrevention = 100;

    private void OnEnable()
    {
        TaskStatus.onTaskSelected += SetTaskKeys;
        SpawnAllTasks();
    }

    private void OnDisable()
    {
        TaskStatus.onTaskSelected -= SetTaskKeys;
    }

    void SpawnTask(Vector2 spawnPos)
    {
        GameObject task = Instantiate(typeOfTasks[rnd.Next(typeOfTasks.Count)], spawnPos, Quaternion.identity, transform);
        task.GetComponent<TaskStatus>().taskLayer = layer;
        tasks.Add(task);
    }

    Vector2 GetTaskSpawnPos(float layerMinRadius, float layerMaxRadius,  float taskRadius)
    {
        bool tasksOverlap;
        bool taskWithinInner;
        int count = 0;
        Vector2 newLocation;
        do
        {
            count++;
            newLocation = UnityEngine.Random.insideUnitCircle * (layerMaxRadius - taskRadius);
            tasksOverlap = false;
            taskWithinInner = false;
            if (Vector2.Distance(newLocation, Vector2.zero) <= (layerMinRadius + taskRadius))
            {
                taskWithinInner = true;
                continue;
            }
            foreach (var task in tasks)
            {
                if (Vector2.Distance(newLocation, task.transform.position) <= (2 * taskRadius))
                {
                    tasksOverlap = true;
                    break;
                }
            }
        } while (taskWithinInner || (tasksOverlap && count < infiniteLoopPrevention));
        if (count >= infiniteLoopPrevention) { Debug.LogWarning("Couldn't find good location for new task"); }

        return newLocation;
    }

    void SpawnAllTasks()
    {
        for (int i = 0; i < noOfTasksSpawned; i++)
        {
            SpawnTask(GetTaskSpawnPos(layerMinRadius, layerMaxRadius, taskRadius));
        }
    }

    void SetTaskKeys(GameObject task)
    {
        if (!task.GetComponent<TaskStatus>().isBeingSolved)
        {
            List<int> keys = PlayerKeyInput.instance.DetermineFreeKeys(noOfKeysPerTask);
            task.GetComponent<TaskStatus>().SetKeysRequired(keys);
        }
    }

    bool IsLayerCompleted()
    {
        bool completed = true;
        foreach (var task in tasks)
        {
            if (!task.GetComponent<TaskStatus>().isSolved)
            {
                completed = false;
            }
        }
        isCompleted = completed;
        return completed;
    }

    bool ContainsTaskGoneWrong()
    {
        bool taskGoneWrong = false;
        foreach (var task in tasks)
        {
            if (task.GetComponent<TaskStatus>().isGoingWrong)
            {
                taskGoneWrong = true;
            }
        }
        isGoingWrong = taskGoneWrong;
        return taskGoneWrong;
    }

    bool ContainsTaskBeingSolved()
    {
        bool taskBeingSolved = false;
        foreach (var task in tasks)
        {
            if (task.GetComponent<TaskStatus>().isBeingSolved)
            {
                taskBeingSolved = true;
            }
        }
        isBeingSolved = taskBeingSolved;
        return taskBeingSolved;
    }

    bool ContainsTaskBeingFocused()
    {
        bool taskBeingFocused = false;
        foreach (var task in tasks)
        {
            if (task.GetComponent<TaskStatus>().isSelected && task.GetComponent<TaskStatus>().isBeingSolved)
            {
                taskBeingFocused = true;
            }
        }
        isBeingFocused = taskBeingFocused;
        return taskBeingFocused;
    }

    GameObject FindTaskToGoWrong()
    {
        List<GameObject> validTasks = new List<GameObject>();
        foreach (var task in tasks)
        {
            if (!task.GetComponent<TaskStatus>().isGoingWrong)
            {
                validTasks.Add(task);
            }
        }
        if (validTasks.Count > 0) { return validTasks[rnd.Next(validTasks.Count)]; }
        else { return null; }
    }
}
