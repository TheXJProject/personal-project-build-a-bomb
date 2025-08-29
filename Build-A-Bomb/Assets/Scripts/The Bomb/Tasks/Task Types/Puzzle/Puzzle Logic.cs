using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 7;
    const int minPossibleDifficultly = 1;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;

    // Initialise In Inspector:
    [SerializeField] TaskInteractStatus statInteract;

    // Runtime Variables:
    int numOfConnectionsNeeded = minPossibleDifficultly;
    int numOfConnections = 0;
    bool isSetup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        isSetup = false;
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetTask;
        TaskInteractStatus.onTaskDifficultySet += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetTask;
        TaskInteractStatus.onTaskDifficultySet -= SetDifficulty;
    }

    /// <summary>
    /// Called by Nail Head gameobject. When the player <br />
    /// clicks on the Nail Head the remaining number of <br />
    /// times the player needs to click is reduced by one.
    /// </summary>
    public void NailHit(BaseEventData data)
    {
        if (Msg) Debug.Log("Called function");

        // Checks if the task can be solved
        if (statInteract.isBeingSolved)
        {
            if (Msg) Debug.Log("Task is being solved");
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Left click is being pressed");

                // Increases the total number of times Nail Head has been hit by one
                numOfConnections++;

                // Set the completion level
                statInteract.SetTaskCompletion((float)numOfConnections / numOfConnectionsNeeded);

                // Check if task is completed
                if (numOfConnections >= numOfConnectionsNeeded)
                {
                    statInteract.TaskCompleted();
                }
            }
        }
    }

    /// <summary>
    /// Called by SetDifficulty method only! <br />
    /// Starts required setup for the task. <br />
    /// </summary>
    void SetupTask()
    {
        // This function can only be activated once
        if (isSetup)
        {
            Debug.LogWarning("Error, this task is already set up!");
        }
        else
        {
            // This instance is now setup
            isSetup = true;
        }
    }

    /// <summary>
    /// Called by onTaskDifficultySet event only! <br />
    /// Retrieves a difficult setting and applies it to this task <br />
    /// instance. Then calls for the task to be setup.
    /// </summary>
    void SetDifficulty(GameObject triggerTask)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Set Difficultly");

            // Retrieves difficulty
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty;

            // Sets difficulty level (the number of hits needed in this case)
            numOfConnectionsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of hits needed cannot be zero
            numOfConnectionsNeeded = Mathf.Max(numOfConnectionsNeeded, minPossibleDifficultly);

            SetupTask();
        }
    }

    /// <summary>
    /// Called by onTaskFailed event only! <br />
    /// Resets the task back to its state just after SetupTask <br />
    /// has been called.
    /// </summary>
    void ResetTask(GameObject trigger)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");

            // Reset the number of times the player has hit
            numOfConnections = 0;

            // Set the completion level
            statInteract.SetTaskCompletion((float)numOfConnections / numOfConnectionsNeeded);
        }
    }
}
