using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IsotopeLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 6;
    const int minPossibleDifficultly = 1;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;

    // Initialise In Inspector:
    [SerializeField] GameObject[] reactors;
    [SerializeField] GameObject[] grates;
    [SerializeField] Image chargedLight;
    public TaskInteractStatus statInteract;

    // Runtime Variables:
    int numReactorsNeeded = minPossibleDifficultly;
    int numReactorsCompleted = 0;
    List<int> activeReactors;
    bool reactorState = false;
    float removedTime = 0;
    bool isSetup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        isSetup = false;
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResestTask;
        TaskInteractStatus.onTaskDifficultySet += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResestTask;
        TaskInteractStatus.onTaskDifficultySet -= SetDifficulty;
    }

    private void FixedUpdate()
    {
        // If we can complete the task set its completion level
        if (statInteract.isBeingSolved)
        {
            // If the task is not selected
            if (!statInteract.task.isSelected)
            {
                // Increase the time accululated
                removedTime += Time.fixedDeltaTime;
            }
            else
            {
                // Remove accumulated time from reactors
                foreach (int reactorIdx in activeReactors)
                {
                    reactors[reactorIdx].GetComponent<ReactorLogic>().timeHeld = Mathf.Max(reactors[reactorIdx].GetComponent<ReactorLogic>().timeHeld - removedTime, 0f);
                }

                removedTime = 0;

                // Set completion level
                CompletionLevel();

                // If the reactors are off, change them to on
                if (!reactorState)
                {
                    // Each reactor can now spool up
                    foreach (int reactorIdx in activeReactors)
                    {
                        reactors[reactorIdx].GetComponent<ReactorLogic>().canSpool = true;
                    }

                    reactorState = true;
                }
            }
        }
        else
        {
            removedTime = 0;

            chargedLight.color = Color.red;

            // If the reactors are on, turn them off
            if (reactorState)
            {
                // Each reactor is off
                foreach (int reactorIdx in activeReactors)
                {
                    reactors[reactorIdx].GetComponent<ReactorLogic>().canSpool = false;
                }

                reactorState = false;
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Sets the task completion level.
    /// </summary>
    void CompletionLevel()
    {
        float totalPercentage = 0;
        int reactorNumCharged = 0;

        // Add up all percentage complete components
        foreach (int reactorIdx in activeReactors)
        {
            totalPercentage += reactors[reactorIdx].GetComponent<ReactorLogic>().fanCompletePercentage;

            // If the reactor is charged
            if (reactors[reactorIdx].GetComponent<ReactorLogic>().charged)
            {
                reactorNumCharged++;
            }
        }

        // TODO: Replace with animations
        // Adjust colour of light depending on number of charged reactors
        if (reactorNumCharged == numReactorsNeeded)
        {
            chargedLight.color = Color.green;
        }
        else if (reactorNumCharged > 0)
        {
            chargedLight.color = Color.yellow;
        }
        else
        {
            chargedLight.color = Color.red;
        }

        // Set the completion level
        statInteract.SetTaskCompletion(totalPercentage / (numReactorsNeeded * 100f));

        if (Msg) Debug.Log("Complete Percentage: " + (totalPercentage / numReactorsNeeded) + "%");
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called when button is pressed. <br />
    /// Check if the task is completed. <br />
    /// </summary>
    public void CheckIfComplete(BaseEventData data)
    {
        // If left mouse button was pressed
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            numReactorsCompleted = 0;

            // For every reactor in use
            foreach (int reactorIdx in activeReactors)
            {
                // If the reactor is charged
                if (reactors[reactorIdx].GetComponent<ReactorLogic>().charged)
                {
                    // Increase the number of reactors completed
                    numReactorsCompleted++;
                }
            }

            // Check if task is completed
            if (numReactorsCompleted >= numReactorsNeeded)
            {
                statInteract.TaskCompleted();
            }

            if (Msg) Debug.Log("Num complete reactors: " + numReactorsCompleted);
            if (Msg) Debug.Log("Num reactors needed: " + numReactorsNeeded);
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Returns a list of randomly generated, ordered integers. <br />
    /// Parameter 1: Number of elements in the list. <br />
    /// </summary>
    List<int> GenerateUniqueRandomNumbers(int numUnused)
    {
        System.Random rand = new();

        // Using HashSet since each element must be unique
        HashSet<int> uniqueNumbers = new();
        List<int> randomNumbers = new();

        // Keep going until we fill the requirements
        while (uniqueNumbers.Count < numUnused)
        {
            // Generates new number between max and min possible difficulty
            int randomNumber = rand.Next(maxPossibleDifficultly);

            // Checks for unique number
            if (uniqueNumbers.Add(randomNumber))
            {
                // Adds new integer to list
                randomNumbers.Add(randomNumber);
            }
        }

        randomNumbers.Sort();

        return randomNumbers;
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

            // If we are using all the reactors
            if (numReactorsNeeded == maxPossibleDifficultly)
            {
                // All reactors are active
                activeReactors = new List<int> { 0, 1, 2, 3, 4, 5 };

                // We don't need to remove any reactors
                // Instead remove all grates
                for (int i = 0; i < grates.Length; i++)
                {
                    Destroy(grates[i]);
                    grates[i] = null;
                }
            }
            else
            {
                // Set some reactors to active randomly
                activeReactors = GenerateUniqueRandomNumbers(numReactorsNeeded);

                // Remove required grates and reactors
                for (int i = 0; i < maxPossibleDifficultly; i++)
                {
                    // If we are using reactor i
                    if (activeReactors.Contains(i))
                    {
                        // Remove the grate
                        Destroy(grates[i]);
                        grates[i] = null;
                    }
                    else
                    {
                        // Otherwise remove the grate
                        Destroy(reactors[i]);
                        reactors[i] = null;
                    }
                }
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
            numReactorsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of reactors cannot be zero
            numReactorsNeeded = Mathf.Max(numReactorsNeeded, minPossibleDifficultly);

            SetupTask();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskFailed event only! <br />
    /// Resets the task back to its state just after SetupTask <br />
    /// has been called.
    /// </summary>
    void ResestTask(GameObject trigger)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");

            // For each reactor in use
            foreach (int reactorIdx in activeReactors)
            {
                // Set to uncharged and time held to zero
                reactors[reactorIdx].GetComponent<ReactorLogic>().charged = false;
                reactors[reactorIdx].GetComponent<ReactorLogic>().timeHeld = 0f;
            }

            // No reactors are completed
            numReactorsCompleted = 0;

            // Set the completion level to "0"
            statInteract.SetTaskCompletion(0);
        }
    }
}
