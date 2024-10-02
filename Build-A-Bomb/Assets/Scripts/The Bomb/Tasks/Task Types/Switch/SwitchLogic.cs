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
    
    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");
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

        if (canBeSolved) // This function only works if the task canBeSolved
        {
            if (switches != null)
            {
                foreach (GameObject s in switches)
                {
                    if (s.GetComponent<SwitchFlick>().flicked)
                    {
                        totalOn++; // Getting the total number of switches flicked on
                    }
                }
            }
            else
            {
                Debug.LogWarning("Error, switches not instantiated. (CheckSwitches)");
            }

            numFlickedSwitches = totalOn;
            statInteract.SetTaskCompletion((float)numFlickedSwitches / numOfSwitchesNeeded); // Set the completion level

            if (numFlickedSwitches >= numOfSwitchesNeeded) // Check if task is completed
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
        HashSet<int> uniqueNumbers = new();
        List<int> randomNumbers = new();

        while (uniqueNumbers.Count < numUnused) // Keep going until we fill the requirements
        {
            int randomNumber = rand.Next(maxPosition); // Generates new number
            if (uniqueNumbers.Add(randomNumber)) // Checks for unique number
            {
                randomNumbers.Add(randomNumber); // Adds new integer to list
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

        if (numOfSwitchesNeeded % maxNumberSwitchesRow == 0) // This if statement allows for calculating the required number of rows
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

        float yCurrent = -spawnBoxHeight / 2; // Variables for calculating the shape and layout of the box containing the switches
        float xCurrent = -spawnBoxWidth / 2;
        float yIncrement = spawnBoxHeight / (rows + 1);
        float xIncrement = spawnBoxWidth / (maxNumberSwitchesRow + 1);
        int unused = (rows * maxNumberSwitchesRow) - numOfSwitchesNeeded;
        switchPositions = new List<Vector2>();
        Vector2 toAdd = Vector2.zero;

        for (int row = 1; row <= rows; row++) // For each row we have, find the y positions of switches in the row
        {
            yCurrent += yIncrement;

            for (int numInRow = 1; numInRow <= maxNumberSwitchesRow; numInRow++) // For each space in a row, find the x positions of switches in the row
            {
                xCurrent += xIncrement;

                toAdd.y = yCurrent;
                toAdd.x = xCurrent;
                switchPositions.Insert(switchIndex, toAdd); // Contain all the coordinates in a list

                switchIndex++;
            }

            xCurrent = -spawnBoxWidth / 2;
        }

        List<int> missPositions = GenerateUniqueRandomNumbers(unused, switchPositions.Count); // Generate the random positions to remove

        if (Msg) Debug.Log("Unused: " + unused);
        if (Msg) Debug.Log("missPositions Size: " + missPositions.Count);
        if (Msg) Debug.Log("Max index for array: " + switchPositions.Count);

        for (int i = switchIndex - 1; i >= 0; i--)
        {
            if (missPositions.Contains(i))
            {
                switchPositions.RemoveAt(i); // Remove the positions generated

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
        switches = new GameObject[numOfSwitchesNeeded]; // Number of switch gameobjects required

        SwitchPositionCreator(); // Generates positions for each of the gameobjects

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
            for (int i = 0; i < switches.Length; i++) // Instantiates each switch at the correct location
            {
                switches[i] = Instantiate(switchPrefab, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
                switches[i].transform.localPosition = switchPositions[i];
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by SetDifficulty method only! <br />
    /// Starts required setup for the task. <br />
    /// <br />
    /// (Some tasks require minimal or no additional code here.)
    /// </summary>
    void SetupTask()
    {
        SpawnSwitches(); // Spawn all required switches
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskDifficultySet event only! <br />
    /// Retrieves a difficult setting and applies it to this task <br />
    /// instance. Then calls for the task to be setup.
    /// </summary>
    void SetDifficulty(GameObject triggerTask)
    {
        if (triggerTask == gameObject.transform.parent.gameObject) // TODO: correct comment
        {
            if (Msg) Debug.Log("Set Difficultly " + gameObject.transform.parent.gameObject);
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty; // Retrieves difficulty

            numOfSwitchesNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f); // Sets difficulty level (the number of switches in this case)
            numOfSwitchesNeeded = Mathf.Max(numOfSwitchesNeeded, minPossibleDifficultly); // The number of switches cannot be zero

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
        if (trigger == gameObject) // TODO: correct comment
        {
            if (Msg) Debug.Log("Reset Task");

            if (switches != null)
            {
                numFlickedSwitches = 0;

                foreach (GameObject s in switches)
                {
                    s.GetComponent<SwitchFlick>().ResetSwitch();
                }
            }
            else
            {
                Debug.LogWarning("Error, switches not instantiated. (ResetSwitch)");
            }
        }
    }
}
