using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerStatus : MonoBehaviour
{
    // Layer Event Actions
    public static event Action<GameObject> onLayerCompleted;
    public static event Action<GameObject> onTaskCreated;

    // To be adjusted as seen fit
    public int noOfTasksSpawned = 4;
    public List<GameObject> typeOfTasks;
    public List<int> taskMinDifficulty;
    public List<int> taskMaxDifficulty;

    // Stats of the layer
    public bool isSelected = false; // Whether it is the layer the player has currently got selected
    public bool isCompleted = false;
    public bool isGoingWrong = false; // Contains a task that is going wrong
    public bool isBeingSolved = false; // Contains a task that is being solved by the player
    public bool isBeingFocused = false; // Contains the task that is currently dispayed and being solved by the player

    // To be set when spawned (values given below are the default values)
    public int layer;
    public int noOfKeysPerTask = 1;
    public float layerMinRadius = 1f;
    public float layerMaxRadius = 3f;
    public float taskSize = 0.6f;
    public float taskScaleUp = 1f;
    public float taskColliderRadius = 0.8f;
    public List<GameObject> tasks = new List<GameObject>();

    // Other variables for miscellaneous purposes
    System.Random rnd = new System.Random();
    public int infiniteLoopPrevention = 100;

    private void OnEnable()
    {
        TaskStatus.onTaskSelected += SetTaskKeys;
        TaskStatus.onTaskCompleted += LayerCompleted;
        BombStatus.onLayerSettingsSet += SpawnAllTasks;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskSelected -= SetTaskKeys;
        TaskStatus.onTaskCompleted -= LayerCompleted;
        BombStatus.onLayerSettingsSet -= SpawnAllTasks;
    }

    void SpawnTask(Vector2 spawnPos) // Spawns in one tasks using a Vector2
    {
        int i = rnd.Next(typeOfTasks.Count);
        GameObject task = Instantiate(typeOfTasks[i], spawnPos, Quaternion.identity, transform);
        task.GetComponent<TaskStatus>().difficulty = (float)(rnd.Next(taskMinDifficulty[i], taskMaxDifficulty[i])) / 100f;
        task.GetComponent<TaskStatus>().taskLayer = layer;
        task.transform.localScale = new Vector2(taskSize, taskSize);
        tasks.Add(task);
        onTaskCreated?.Invoke(task);
    }

    Vector2 GetTaskSpawnPos(float layerMinRadius, float layerMaxRadius,  float taskRadius) // Gets the next tasks spawn position based on the sizings of the current layer
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
            if (Vector2.Distance(newLocation, Vector2.zero) <= (layerMinRadius + taskRadius)) // Determines if the task would spawn on top of the previous layers (the inner circle)
            {
                taskWithinInner = true;
                continue;
            }
            foreach (var task in tasks)
            {
                if (Vector2.Distance(newLocation, task.transform.position) <= (2 * taskRadius)) // Determines if the task would overlap with any existing tasks
                {
                    tasksOverlap = true;
                    break;
                }
            } // After a set number of attempts to find a non-overlapping position, it decides just to find one that isn't within the centre but might overlap a task
        } while (taskWithinInner || (tasksOverlap && count < infiniteLoopPrevention)); 
        if (count >= infiniteLoopPrevention) { Debug.LogWarning("Couldn't find good location for new task"); }

        return newLocation;
    }

    void SpawnAllTasks(int spawningLayer)
    {
        if (layer == spawningLayer)
        {
            for (int i = 0; i < noOfTasksSpawned; i++)
            {
                SpawnTask(GetTaskSpawnPos(layerMinRadius, layerMaxRadius, taskSize * taskColliderRadius * taskScaleUp));
            }
        }
    }

    void SetTaskKeys(GameObject task) // The keys for a task are decided when a player clicks onto a task
    {
        if (!task.GetComponent<TaskStatus>().isBeingSolved)
        {
            List<int> keys = PlayerKeyInput.instance.DetermineFreeKeys(noOfKeysPerTask);
            task.GetComponent<TaskStatus>().SetKeysRequired(keys);
        }
    }

    bool IsLayerCompleted() // Loops through overy task in the layer and returns true if they are all completed
    {
        foreach (var task in tasks)
        {
            if (!task.GetComponent<TaskStatus>().isSolved)
            {
                return false;
            }
        }
        return true;
    }

    void LayerCompleted(GameObject triggerTask) // Called whenever a task is completed
    {
        if (IsLayerCompleted())
        {
            isCompleted = true;
            onLayerCompleted?.Invoke(gameObject);
        }
    }

    bool ContainsTaskGoneWrong() // Loops through overy task in the layer and sets status of the 
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
