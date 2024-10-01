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
    /// TODO: WRITE LATER!
    /// </summary>
    public void CheckSwitches()
    {
        int totalOn = 0;
        canBeSolved = statInteract.isBeingSolved; // whatevers

        if (canBeSolved)
        {
            if (switches != null)
            {
                foreach (GameObject s in switches)
                {
                    if (s.GetComponent<SwitchFlick>().flicked)
                    {
                        totalOn++;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Error, switches not instantiated. (CheckSwitches)");
            }

            numFlickedSwitches = totalOn;
            statInteract.SetTaskCompletion((float)numFlickedSwitches / numOfSwitchesNeeded);

            if (numFlickedSwitches >= numOfSwitchesNeeded)
            {
                statInteract.TaskCompleted();
            }
        }
    }



    public List<int> GenerateUniqueRandomNumbers(int numUnused, int maxPosition)
    {
        System.Random rand = new();
        HashSet<int> uniqueNumbers = new();
        List<int> randomNumbers = new();

        while (uniqueNumbers.Count < numUnused)
        {
            int randomNumber = rand.Next(maxPosition + 1);
            if (uniqueNumbers.Add(randomNumber))
            {
                randomNumbers.Add(randomNumber);
            }
        }

        randomNumbers.Sort();

        return randomNumbers;
    }

    void SwitchPositionCreator()
    {
        int rows;
        int switchIndex = 0;

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

        float yCurrent = -spawnBoxHeight / 2;
        float xCurrent = -spawnBoxWidth / 2;
        float yIncrement = spawnBoxHeight / (rows + 1);
        float xIncrement = spawnBoxWidth / (maxNumberSwitchesRow + 1);
        int unused = (rows * maxNumberSwitchesRow) - numOfSwitchesNeeded;
        switchPositions = new List<Vector2>(rows * maxNumberSwitchesRow);
        Vector2 toAdd = Vector2.zero;

        for (int row = 1; row <= rows; row++)
        {
            yCurrent += yIncrement;

            for (int numInRow = 1; numInRow <= maxNumberSwitchesRow; numInRow++)
            {
                xCurrent += xIncrement;

                toAdd.y = yCurrent;
                toAdd.x = xCurrent;
                switchPositions.Insert(switchIndex, toAdd);

                switchIndex++;
            }

            xCurrent = -spawnBoxWidth / 2;
        }

        List<int> missPositions = GenerateUniqueRandomNumbers(unused, switchPositions.Count);

        for (int i = switchIndex - 1; i >= 0; i--)
        {
            if (missPositions.Contains(i))
            {
                switchPositions.RemoveAt(i);
            }
        }
    }

    void SpawnSwitches()
    {
        switches = new GameObject[numOfSwitchesNeeded];

        SwitchPositionCreator();

        if (Msg) Debug.Log("Spawned Switchs");
        if (Msg) Debug.Log("same " + numOfSwitchesNeeded);
        if (Msg) Debug.Log("same " + switches.Length);
        if (Msg) Debug.Log("same " + switchPositions.Count);

        for (int i = 0; i < switches.Length; i++)
        {
            switches[i] = Instantiate(switchPrefab, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
            switches[i].transform.localPosition = switchPositions[i];
        }
    }

    void SetDifficulty(GameObject triggerTask)
    {
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Set Difficultly " + gameObject.transform.parent.gameObject);
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty;

            numOfSwitchesNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);
            numOfSwitchesNeeded = Mathf.Max(numOfSwitchesNeeded, minPossibleDifficultly);

            SpawnSwitches();
        }
    }

    void ResetSwitch(GameObject trigger)
    {
        if (trigger == gameObject)
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
