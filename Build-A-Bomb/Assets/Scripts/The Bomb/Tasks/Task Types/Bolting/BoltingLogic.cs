using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltingLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 15;
    const int minPossibleDifficultly = 1;
    const int multipleFactor = 2;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;

    // Initialise In Inspector:
    [SerializeField] GameObject variation1;
    [SerializeField] GameObject variation2;
    [SerializeField] GameObject variation3;
    public TaskInteractStatus statInteract;

    // Runtime Variables:
    int numBoltsNeeded = minPossibleDifficultly * multipleFactor;
    int numBoltsCompleted = 0;
    GameObject variationInUse;
    bool isSetup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        isSetup = false;
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResestBolts;
        TaskInteractStatus.onTaskDifficultySet += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResestBolts;
        TaskInteractStatus.onTaskDifficultySet -= SetDifficulty;
    }

    /// FUNCTION DESCRIPTION <summary>
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

            // Choose a random number between 1 and 3
            int randomNumber = Random.Range(1, 4); // Random.Range's upper bound is exclusive for integers
            if (Msg) Debug.Log("Random Number: " + randomNumber);

            // Use the random number to decide which variation to spawn
            switch (randomNumber)
            {
                case 1:
                    // Spawn variation 1 of the task
                    variationInUse = Instantiate(variation1, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
                    break;
                case 2:
                    // Spawn variation 2 of the task
                    variationInUse = Instantiate(variation2, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
                    break;
                case 3:
                    // Spawn variation 3 of the task
                    variationInUse = Instantiate(variation3, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
                    break;
                default:
                    Debug.LogWarning("Error, random number, " + randomNumber + " out of range!");
                    break;
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskDifficultySet event only! <br />
    /// Retrieves a difficult setting and applies it to this task <br />
    /// instance. Then calls for the task to be setup.
    /// </summary>
    void SetDifficulty(GameObject triggerTask)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Set Difficultly " + gameObject.transform.parent.gameObject);

            // Retrieves difficulty
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty;

            // Sets difficulty level (the number of switches in this case)
            numBoltsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f) * multipleFactor;

            // The number of switches cannot be zero
            numBoltsNeeded = Mathf.Max(numBoltsNeeded, (minPossibleDifficultly * multipleFactor));

            SetupTask();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskFailed event only! <br />
    /// Resets the task back to its state just after SetupTask <br />
    /// has been called.
    /// </summary>
    void ResestBolts(GameObject trigger)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");

            // Set the number of flicked switches to zero
            numBoltsCompleted = 0;

            // TODO: Add in required reset measures

            // Reset Bolts first 
            // Then reset bolting panels

            //variationInUse.
        }
    }
}
