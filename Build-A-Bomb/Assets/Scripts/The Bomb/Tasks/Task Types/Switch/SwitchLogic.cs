using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchLogic : MonoBehaviour
{
    readonly bool Msg = false; // ==== For Debugging! ====
    
    [SerializeField]
    GameObject switchPrefab;

    const int maxPossibleDifficultly = 60;
    const int minPossibleDifficultly = 1;
    [Range(minPossibleDifficultly, maxPossibleDifficultly)]
    public int currentHardestDifficulty;

    int numOfSwitchesNeeded = minPossibleDifficultly;
    int numFlickedSwitches = 0;
    
    [Range(4,400)]
    public float spawnBoxHeight;

    [Range(8, 800)]
    public float spawnBoxWidth;

    [Range(2, 60)]
    public int maxNumberSwitchesRow;

    GameObject[] switches;
    List<Vector2> switchPositions;
    
    public bool canBeSolved = false;

    TaskInteractStatus statInteract;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");
        statInteract = GetComponent<TaskInteractStatus>();
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetSwitch;
        TaskInteractStatus.onChangeTaskDifficulty += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetSwitch;
        TaskInteractStatus.onChangeTaskDifficulty -= SetDifficulty;
    }

    public void CheckSwitches()
    {
        int totalOn = 0;
        canBeSolved = statInteract.isBeingSolved;

        if (canBeSolved)
        {
            foreach (GameObject s in switches)
            {
                if (s.GetComponent<SwitchFlick>().flicked)
                {
                    totalOn++;
                }
            }

            numFlickedSwitches = totalOn;
            statInteract.SetTaskCompletion(numFlickedSwitches / numOfSwitchesNeeded);

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

        if (Msg) Debug.Log("Num Of Switches Needed: " + numOfSwitchesNeeded);
        if (Msg) Debug.Log("Max Num Switches: " + maxNumberSwitchesRow);
        if (Msg) Debug.Log("Rows: " + rows);
        if (Msg) Debug.Log("Unused Values: " + unused);

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
                if (Msg) Debug.Log("Miss Position: " + i);
                switchPositions.RemoveAt(i);
            }
        }
    }

    void SpawnSwitches()
    {
        SwitchPositionCreator();

        switches = new GameObject[numOfSwitchesNeeded];

        if (Msg) Debug.Log("Spawned Switchs");

        for (int i = 0; i < numOfSwitchesNeeded; i++)
        {
            switches[i] = Instantiate(switchPrefab, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
            switches[i].transform.localPosition = switchPositions[i];
        }
    }

    void SetDifficulty(GameObject triggerTask)
    {
        if (triggerTask = gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Correct comparison");
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
            numFlickedSwitches = 0;

            foreach (GameObject s in switches)
            {
                s.GetComponent<SwitchFlick>().ResetSwitch();
            }
        }
    }
}
