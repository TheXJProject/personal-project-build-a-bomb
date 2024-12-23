using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 60;
    const int minPossibleDifficultly = 1;

    // Inspector Adjustable Values:
    [Range(4,400)] public float spawnBoxHeight;
    [Range(8, 800)] public float spawnBoxWidth;
    [Range(2, 60)] public int maxNumberSwitchesRow;
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;

    // Initialise In Inspector:
    [SerializeField] GameObject switchPrefab;
    [SerializeField] TaskInteractStatus statInteract;

    // Runtime Variables:
    [HideInInspector] public bool canBeSolved = false;
    int numOfSwitchesNeeded = minPossibleDifficultly;
    int numFlickedSwitches = 0;
    GameObject[] switches;
    List<Vector2> switchPositions;
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

    /// FUNCTION DESCRIPTION <summary>
    /// When called, this function goes through each switch gameobject <br />
    /// and checks if it is flicked on. Then it sets the task completion <br />
    /// level and if the task is completed respectively.
    /// </summary>
    public void CheckSwitches()
    {
        int totalOn = 0;
        canBeSolved = statInteract.isBeingSolved;

        // This function only works if the task canBeSolved
        if (canBeSolved) 
        {
            if (switches != null)
            {
                foreach (GameObject s in switches)
                {
                    if (s.GetComponent<SwitchFlick>().flicked)
                    {
                        // Getting the total number of switches flicked on
                        totalOn++; 
                    }
                }
            }
            else
            {
                Debug.LogWarning("Error, switches not instantiated. (CheckSwitches)");
            }

            numFlickedSwitches = totalOn;

            // Set the completion level
            statInteract.SetTaskCompletion((float)numFlickedSwitches / numOfSwitchesNeeded);

            // Check if task is completed
            if (numFlickedSwitches >= numOfSwitchesNeeded)
            {
                statInteract.TaskCompleted();
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Returns a list of randomly generated, ordered integers. <br />
    /// Parameter 1: Number of elements in the list. <br />
    /// Parameter 2: Integer upper limit, exclusive.
    /// </summary>
    List<int> GenerateUniqueRandomNumbers(int numUnused, int maxPosition)
    {
        System.Random rand = new();

        // Using HashSet since each element must be unique
        HashSet<int> uniqueNumbers = new();
        List<int> randomNumbers = new();

        // Keep going until we fill the requirements
        while (uniqueNumbers.Count < numUnused)
        {
            // Generates new number
            int randomNumber = rand.Next(maxPosition);

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
    /// Calculates and create all the positions for the switch gameobjects. <br />
    /// It finds the number and coordinates for all possible positions <br />
    /// then randomly leaves out however many it does not need.
    /// </summary>
    void SwitchPositionCreator()
    {
        int rows;
        int switchIndex = 0;

        // This if statement allows for calculating the required number of rows
        if (numOfSwitchesNeeded % maxNumberSwitchesRow == 0)
        {
            rows = numOfSwitchesNeeded / maxNumberSwitchesRow;
        }
        else
        {
            rows = (int)(numOfSwitchesNeeded / maxNumberSwitchesRow) + 1;
        }

        if (rows == 0)
        {
            Debug.LogWarning("Error, Number of Rows is zero!");
        }

        // Variables for calculating the shape and layout of the box containing the switches
        float yCurrent = -spawnBoxHeight / 2;
        float xCurrent = -spawnBoxWidth / 2;
        float yIncrement = spawnBoxHeight / (rows + 1);
        float xIncrement = spawnBoxWidth / (maxNumberSwitchesRow + 1);
        int unused = (rows * maxNumberSwitchesRow) - numOfSwitchesNeeded;
        switchPositions = new List<Vector2>();
        Vector2 toAdd = Vector2.zero;

        // For each row we have, find the y positions of switches in the row
        for (int row = 1; row <= rows; row++)
        {
            yCurrent += yIncrement;

            // For each space in a row, find the x positions of switches in the row
            for (int numInRow = 1; numInRow <= maxNumberSwitchesRow; numInRow++)
            {
                xCurrent += xIncrement;

                toAdd.y = yCurrent;
                toAdd.x = xCurrent;

                // Contain all the coordinates in a list
                switchPositions.Insert(switchIndex, toAdd);

                switchIndex++;
            }

            xCurrent = -spawnBoxWidth / 2;
        }

        // Generate the random positions to remove
        List<int> missPositions = GenerateUniqueRandomNumbers(unused, switchPositions.Count);

        if (Msg) Debug.Log("Unused: " + unused);
        if (Msg) Debug.Log("missPositions Size: " + missPositions.Count);
        if (Msg) Debug.Log("Max index for array: " + switchPositions.Count);

        for (int i = switchIndex - 1; i >= 0; i--)
        {
            if (missPositions.Contains(i))
            {
                // Remove the positions generated
                switchPositions.RemoveAt(i);

                if (Msg) Debug.Log("Miss position: " + i);
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Gets the number of switches required to spawn in and the positions <br />
    /// and creates that many instances.
    /// </summary>
    void SpawnSwitches()
    {
        // Number of switch gameobjects required
        switches = new GameObject[numOfSwitchesNeeded];

        // Generates positions for each of the gameobjects
        SwitchPositionCreator();

        if (Msg) Debug.Log("Spawned Switchs");
        if (Msg) Debug.Log("Num of Switches needed: " + numOfSwitchesNeeded);
        if (Msg) Debug.Log("Num of switch gameobjects: " + switches.Length);
        if (Msg) Debug.Log("Num of switch positions: " + switchPositions.Count);

        if ((switches.Length != numOfSwitchesNeeded) || (switchPositions.Count != numOfSwitchesNeeded))
        {
            Debug.LogWarning("Error, incorrect number of switches/positions spawned!");
        }
        else
        {
            // Instantiates each switch at the correct location
            for (int i = 0; i < switches.Length; i++)
            {
                switches[i] = Instantiate(switchPrefab, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
                switches[i].transform.localPosition = switchPositions[i];
            }
        }
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

            // Spawn all required switches
            SpawnSwitches();
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
            numOfSwitchesNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of switches cannot be zero
            numOfSwitchesNeeded = Mathf.Max(numOfSwitchesNeeded, minPossibleDifficultly);

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

            if (switches != null)
            {
                // Set the number of flicked switches to zero
                numFlickedSwitches = 0;

                // Set all switch gameobjects to off
                foreach (GameObject s in switches)
                {
                    s.GetComponent<SwitchFlick>().ResetSwitch();
                }

                // Set task completetion level
                CheckSwitches();
            }
            else
            {
                Debug.LogWarning("Error, switches not instantiated. (ResetSwitch)");
            }
        }
    }
}
