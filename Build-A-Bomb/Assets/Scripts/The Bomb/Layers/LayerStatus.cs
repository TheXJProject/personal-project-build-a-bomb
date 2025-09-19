using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerStatus : MonoBehaviour
{
    // Event Actions:
    public static event Action<GameObject> onLayerCompleted;
    public static event Action<GameObject> onTaskCreated;
    public static event Action<GameObject> onLayerSelected;
    public static event Action<GameObject> onLayerUnSelected;

    // Inspector Adjustable Values:
    public int minNoOfTasksSpawned = 4;
    public int maxNoOfTasksSpawned = 4;
    public List<GameObject> typeOfTasks;
    public List<int> taskMinDifficulty;
    public List<int> taskMaxDifficulty;
    public List<int> taskMinSpawned;
    public int layer;
    public int noOfKeysPerTask = 1;
    public float layerMinRadius = 1f;
    public float layerMaxRadius = 3f;
    public float taskSize = 0.6f;
    public float taskScaleUp = 1f;
    public float taskColliderRadius = 0.8f;
    public List<GameObject> tasks = new List<GameObject>();

    // Runtime Variables:
    public bool isSelected = false; // Whether it is the layer the player has currently got selected
    public bool isCompleted = false;
    public bool isGoingWrong = false; // Contains a task that is going wrong
    public bool isBeingSolved = false; // Contains a task that is being solved by the player
    public bool isBeingFocused = false; // Contains the task that is currently dispayed and being solved by the player
    int numberOfTasksThisLayer = 0;
    public List<int> taskTypesSpawned;
    System.Random rnd = new System.Random();
    public int infiniteLoopPrevention = 100;

    private void OnEnable()
    {
        TaskStatus.onTaskSelected += SetTaskKeys;
        TaskStatus.onTaskGoneWrong += SetGoneWrong;
        TaskStatus.onTaskCompleted += LayerCompleted;
        BombStatus.onLayerCreated += SpawnAllTasks;
        BombStatus.onLayerCreated += SetCurrentLayer;
        BombStatus.onEachGoingWrongTasksSolved += SetCurrentLayer;
        LayerButtonPress.onLayerButtonPressed += SetCurrentLayer;

        taskTypesSpawned = new List<int>(new int[taskMinSpawned.Count]);
    }

    private void OnDisable()
    {
        TaskStatus.onTaskSelected -= SetTaskKeys;
        TaskStatus.onTaskGoneWrong += SetGoneWrong;
        TaskStatus.onTaskCompleted -= LayerCompleted;
        BombStatus.onLayerCreated -= SpawnAllTasks;
        BombStatus.onLayerCreated -= SetCurrentLayer;
        BombStatus.onEachGoingWrongTasksSolved -= SetCurrentLayer;
        LayerButtonPress.onLayerButtonPressed -= SetCurrentLayer;
    }

    /// <summary>
    /// Spawns in one tasks using a Vector2 spawn position
    /// </summary>
    void SpawnTask(Vector2 spawnPos)
    {
        int tasksSpawnedSoFar = 0;
        int tasksNeedingToSpawn = 0;
        foreach (var tasksSpawned in taskTypesSpawned) // How many tasks have been spawned so far?
        {
            tasksSpawnedSoFar += tasksSpawned;
        }

        foreach (var tasksToSpawn in taskMinSpawned) // How many tasks that have been hard set not to be random need to be spawned?
        {
            tasksNeedingToSpawn += tasksToSpawn;
        }


        int i = 0;
        // Gets a random task types out of those available for that layer level if tasks set not to be random has finished
        if (tasksSpawnedSoFar >= tasksNeedingToSpawn)
        {
            i = rnd.Next(typeOfTasks.Count);
        }
        else
        {
            foreach (var tasksToSpawn in taskMinSpawned) // Determine which task we are on by subtracting tasks to spawn from number of tasks until we reach the next discrepency in expectation
            {
                tasksSpawnedSoFar -= tasksToSpawn;
                if (tasksSpawnedSoFar < 0)
                {
                    ++taskTypesSpawned[i];
                    break;
                } 
                else ++i;
            }
        }

        // Instantiates a new task
        GameObject task = Instantiate(typeOfTasks[i], spawnPos, Quaternion.identity, transform);

        // Sets the percentage difficulty to a random value between the min and max percentage difficulty for that task type on this layer
        task.GetComponent<TaskStatus>().difficulty = (float)(rnd.Next(taskMinDifficulty[i], taskMaxDifficulty[i] + 1)) / 100f;

        // Sets the task's assigned layer number
        task.GetComponent<TaskStatus>().taskLayer = layer;

        // Tasks only spawn when the player advances to next level, so they spawn in on the current layer
        task.GetComponent<TaskStatus>().isOnCurrentLayer = true;

        // Set the tasks size assigned 
        task.transform.localScale = new Vector2(taskSize, taskSize);
        tasks.Add(task);
        onTaskCreated?.Invoke(task);
    }

    /// <summary>
    /// Gets the next task's spawn position based on the sizings of the current layer
    /// </summary>
    Vector2 GetTaskSpawnPos(float layerMinRadius, float layerMaxRadius,  float taskRadius) 
    {
        bool tasksOverlap;
        bool taskWithinInner;
        int count = 0;
        Vector2 newLocation;
        // Do while a suitable spawn position hasn't been found (or it took too long, if so, choose a position that isn't within the centre spawn area)
        do
        {
            count++;

            // Find a new location within the area of the layers radius
            newLocation = UnityEngine.Random.insideUnitCircle * (layerMaxRadius - taskRadius);
            tasksOverlap = false;
            taskWithinInner = false;

            // Determine if the task would spawn on top of the previous layers (the inner circle)
            if (Vector2.Distance(newLocation, Vector2.zero) <= (layerMinRadius + taskRadius)) 
            {
                taskWithinInner = true;
                continue;
            }

            // Determine if the task would overlap with any existing tasks
            foreach (var task in tasks)
            {
                if (Vector2.Distance(newLocation, task.transform.position) <= (2 * taskRadius)) 
                {
                    tasksOverlap = true;
                    break;
                }
            } // After a set number of attempts to find a non-overlapping position, it decides just to find one that isn't within the centre but might overlap a task
        } while ((taskWithinInner || tasksOverlap) && count < infiniteLoopPrevention); 
        if (count >= infiniteLoopPrevention) { Debug.LogWarning("Couldn't find good location for new task"); }

        return newLocation;
    }

    /// <summary>
    /// Spawn all the tasks of a layer into the game using the function to get the next task spawn position
    /// </summary>
    void SpawnAllTasks(GameObject triggerLayer)
    {
        int spawningLayer = triggerLayer.GetComponent<LayerStatus>().layer;
        if (layer == spawningLayer)
        {
            numberOfTasksThisLayer = rnd.Next(minNoOfTasksSpawned, maxNoOfTasksSpawned + 1);
            for (int i = 0; i < numberOfTasksThisLayer; i++)
            {
                SpawnTask(GetTaskSpawnPos(layerMinRadius, layerMaxRadius, taskSize * taskColliderRadius * taskScaleUp));
            }
        }
    }

    /// <summary>
    /// Function that decides the keys for a task. should be used when a player clicks on a task
    /// </summary>
    /// <param name="task"></param>
    void SetTaskKeys(GameObject task)
    {
        TaskStatus status = task.GetComponent<TaskStatus>();
        if (!status.isBeingSolved && status.taskLayer == layer)
        {
            List<int> keys = PlayerKeyInput.instance.DetermineFreeKeys(noOfKeysPerTask);
            task.GetComponent<TaskStatus>().SetKeysRequired(keys);
        }
    }

    void SetGoneWrong(GameObject taskGoneWrong)
    {
        if (taskGoneWrong.GetComponent<TaskStatus>().taskLayer == layer)
        {
            isCompleted = false;
        }
    }

    /// <summary>
    /// Loops through every task in the layer and returns true if they are all completed
    /// </summary>
    /// <returns></returns>
    public bool IsLayerCompleted() 
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

    /// <summary>
    /// Called whenever a task is completed to check whether the layer is completed
    /// </summary>
    void LayerCompleted(GameObject triggerTask)
    {
        if (IsLayerCompleted())
        {
            isCompleted = true;
            onLayerCompleted?.Invoke(gameObject);
        }
    }

    /// <summary>
    /// Loops through every task in the layer and sets status of the layer accordingly if it is going wrong
    /// </summary>
    public bool ContainsTaskGoneWrong()
    {
        bool taskGoneWrong = false;
        foreach (var task in tasks)
        {
            if (task.GetComponent<TaskStatus>().isGoingWrong)
            {
                taskGoneWrong = true;
                break;
            }
        }
        isGoingWrong = taskGoneWrong;
        return taskGoneWrong;
    }

    /// <summary>
    /// Loops through every task in the layer and sets status of the layer accordingly if it contains a task currently being solved by the player
    /// </summary>
    public bool ContainsTaskBeingSolved()
    {
        bool taskBeingSolved = false;
        foreach (var task in tasks)
        {
            if (task.GetComponent<TaskStatus>().isBeingSolved)
            {
                taskBeingSolved = true;
                break;
            }
        }
        isBeingSolved = taskBeingSolved;
        return taskBeingSolved;
    }

    /// <summary>
    /// Loops through every task in the layer and sets status of the layer accordingly if it contains a task currently being solved by the player
    /// </summary>
    public bool ContainsGoingWrongAndNotBeingSolved()
    {
        bool taskNotBeingSolved = false;
        foreach (var task in tasks)
        {
            if (task.GetComponent<TaskStatus>().isGoingWrong && !task.GetComponent<TaskStatus>().isBeingSolved)
            {
                taskNotBeingSolved = true;
                break;
            }
        }
        return taskNotBeingSolved;
    }

    /// <summary>
    /// Loops through every task in the layer and sets status of the layer accordingly if it containts a task that the player has focused
    /// </summary>
    public bool ContainsTaskBeingFocused()
    {
        bool taskBeingFocused = false;
        foreach (var task in tasks)
        {
            if (task.GetComponent<TaskStatus>().isSelected)
            {
                taskBeingFocused = true;
                break;
            }
        }
        isBeingFocused = taskBeingFocused;
        return taskBeingFocused;
    }

    /// <summary>
    /// Loops through every task in the layer and finds every valid task which isn't already going wrong, returning one of them at random
    /// </summary>
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

    /// <summary>
    /// Called when a new layer needs to be set as the current layer, checks if it is the current layer, and sets its tasks accordingly
    /// </summary>
    void SetCurrentLayer(GameObject triggerLayer)
    {
        if (triggerLayer == gameObject)
        {
            isSelected = true;
            foreach (var task in tasks)
            {
                task.GetComponent<TaskStatus>().isOnCurrentLayer = true;
            }
            onLayerSelected?.Invoke(gameObject);
        }
        else
        {
            isSelected = false;
            foreach (var task in tasks)
            {
                task.GetComponent<TaskStatus>().isOnCurrentLayer = false;
            }
            onLayerUnSelected?.Invoke(gameObject);
        }
    }
}
