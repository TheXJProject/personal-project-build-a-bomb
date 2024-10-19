using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = true;

    // Constant Values:
    const int maxPossibleDifficultly = 30;
    const int minPossibleDifficultly = 1;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;

    // Initialise In Inspector:
    public TaskInteractStatus statInteract;
    [SerializeField] TextLogic display;

    // Runtime Variables:
    int numOfPressesNeeded = minPossibleDifficultly;
    int numCorrectPresses = 0;
    List<int> codeSequence;
    List<int> playerSequence;
    bool isSetup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        isSetup = false;
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetSwitch;
        TaskInteractStatus.onTaskDifficultySet += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetSwitch;
        TaskInteractStatus.onTaskDifficultySet -= SetDifficulty;
    }

    /// FUNCTION DESCRIPTION<summary>
    /// Return the amount of common elements in each list. <br />
    /// Parameter 1: The list the player is trying to match. <br />
    /// Parameter 2: The list inputted by the player.
    /// </summary>
    int CountCommonElements(List<int> code, List<int> player)
    {
        //Use the smaller size list to compare
        int minLength = System.Math.Min(code.Count, player.Count);
        int count = 0;

        // For each element in the shortest length list
        for (int i = 0; i < minLength; i++)
        {
            // If we have a matching element increase the count
            if (code[i] == player[i])
            {
                count++;
            }
        }

        // Return number of similar elements
        return count;
    }

    /// FUNCTION DESCRIPTION<summary>
    /// When called by the hash button, this checks if the inputted code is correct. <br />
    /// Then it sets the task completion level and if the task is completed respectively.
    /// </summary>
    public void CheckCode()
    {
        // This function only works if the task canBeSolved
        if (statInteract.isBeingSolved)
        {
            // Check the playerSequence is instantiated
            if (playerSequence == null)
            {
                Debug.LogWarning("Error, playerSequence not instantiated!");
            }
            else if (codeSequence == null)
            {
                Debug.LogWarning("Error, codeSequence not instantiated!");
            }
            else
            {
                // calculate number correctly pressed
                numCorrectPresses = CountCommonElements(codeSequence, playerSequence);

                // Set the number to compare the number of correct presses to
                int numToCompare = System.Math.Max(1, playerSequence.Count);

                // Set the completion level
                statInteract.SetTaskCompletion((float)numCorrectPresses / numToCompare);

                // Check if task is completed
                if (numCorrectPresses >= numOfPressesNeeded)
                {
                    statInteract.TaskCompleted();
                }
            }
        }
    }

    /// FUNCTION DESCRIPTION<summary>
    /// This function resets any input the player has entered. <br />
    /// Then it sets the task completion level and if the task is completed respectively.
    /// </summary>
    public void ResetPlayerSequence()
    {
        // Check the playerSequence is instantiated
        if (playerSequence == null)
        {
            Debug.LogWarning("Error, playerSequence not instantiated! (Reset attempted)");
        }
        else
        {
            // Reset playerSequence to empty
            playerSequence = new();

            // Show Reset
            CheckCode();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Returns a list of randomly generated intergers. <br />
    /// Parameter: Number of elements in the list.
    /// </summary>
    List<int> GenerateRandomNumbers(int numNeeded)
    {
        // Our list to return
        List<int> randomNumbers = new();
        System.Random rand = new();

        // Keep going until we fill the requirements
        while (randomNumbers.Count < numNeeded)
        {
            // Adds new random integer to list
            randomNumbers.Add(rand.Next(10));
        }

        // Returns the list
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

            // Instantiate and generate keypad sequence
            codeSequence = new();
            codeSequence = GenerateRandomNumbers(numOfPressesNeeded);

            // Instantiate player sequence list
            playerSequence = new();
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
            numOfPressesNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of switches cannot be zero
            numOfPressesNeeded = Mathf.Max(numOfPressesNeeded, minPossibleDifficultly);

            SetupTask();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskFailed event only! <br />
    /// Resets the task back to its state just after SetupTask <br />
    /// has been called.
    /// </summary>
    void ResetSwitch(GameObject trigger)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");

            // Reset anything the player has entered
            ResetPlayerSequence();
        }
    }
}
