using UnityEngine;
using UnityEngine.EventSystems;

public class HammerTask : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 150;
    const int minPossibleDifficultly = 1;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;
    [SerializeField] float pauseAfterTaskComplete = 0.1f;

    // Initialise In Inspector:
    [SerializeField] TaskInteractStatus statInteract;
    [SerializeField] GameObject hammer;
    [SerializeField] HammerNailMove nail;

    // Runtime Variables:
    int numOfHitsNeeded = minPossibleDifficultly;
    int numOfHits = 0;
    bool isSetup;
    float pausedTime = 0;

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

    private void Update()
    {
        if (statInteract.isBeingSolvedAndSelected)
        {
            // Set the completion level
            statInteract.SetTaskCompletion((float)numOfHits / numOfHitsNeeded);

            // Check if task is completed
            if (numOfHits >= numOfHitsNeeded)
            {
                pausedTime += Time.deltaTime;

                if (pausedTime > pauseAfterTaskComplete)
                {
                    CheckFinished();
                }
            }
        }
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
        if (statInteract.isBeingSolvedAndSelected)
        {
            if (Msg) Debug.Log("Task is being solved");
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Left click is being pressed");

                // Increases the total number of times Nail Head has been hit by one
                numOfHits++;
                if (hammer.activeSelf) hammer.GetComponent<HammerVisuals>().stillHitting = true;
                else hammer.SetActive(true);

                nail.setRandomRotation();

                // Set the completion level
                statInteract.SetTaskCompletion(Mathf.Min(1,(float)numOfHits / numOfHitsNeeded));
            }
        }
    }

    public void CheckFinished()
    {
        // Check if task is completed
        if (numOfHits >= numOfHitsNeeded)
        {
            statInteract.TaskCompleted();
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
            numOfHitsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of hits needed cannot be zero
            numOfHitsNeeded = Mathf.Max(numOfHitsNeeded, minPossibleDifficultly);

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
            numOfHits = 0;

            // Set the completion level
            statInteract.SetTaskCompletion((float)numOfHits / numOfHitsNeeded);
        }
    }
}
