using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchLogic : MonoBehaviour
{
    readonly bool Msg = true; // ==== For Debugging! ====
    
    [SerializeField]
    GameObject switchPrefab;

    const int maxPossibleDifficultly = 60;
    const int minPossibleDifficultly = 1;
    [Range(minPossibleDifficultly, maxPossibleDifficultly)]
    public int currentHardestDifficulty = maxPossibleDifficultly;

    int numOfSwitchesNeeded = minPossibleDifficultly;
    int numFlickedSwitches = 0;
    
    [Range(4,400)]
    public float spawnBoxHeight;

    [Range(8, 800)]
    public float spawnBoxWidth;

    [Range(3, 60)]
    public int maxNumberSwitchesRow;

    GameObject[] switches;
    Vector2[] switchPositions;
    
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

    void SwitchPositionCreator()
    {
        switchPositions = new Vector2[numOfSwitchesNeeded];

        int rows = 0;

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

        int switchCount = 0;

        for (int row = 1; row <= rows; row++)
        {
            // Y position

            for (int numInRow = 1; numInRow <= maxNumberSwitchesRow; numInRow++)
            {
                if (switchCount > numOfSwitchesNeeded)
                {

                }

                if ((row == rows) && (numOfSwitchesNeeded - switchCount < maxNumberSwitchesRow))
                {
                    if (Msg) Debug.Log("Using Remainder positioning method.");
                }
                else
                {
                    // X position
                }
                switchCount++;
            }
        }
    }

    void SpawnSwitches()
    {
        SwitchPositionCreator();

        switches = new GameObject[numOfSwitchesNeeded];

        for (int i = 0; i < numOfSwitchesNeeded; i++)
        {
            if (Msg) Debug.Log("Spawned Switch");
            switches[i] = Instantiate(switchPrefab, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
            switches[i].transform.localPosition = switchPositions[i];
        }
    }

    void SetDifficulty(float difficulty)
    {
        numOfSwitchesNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);
        numOfSwitchesNeeded = Mathf.Max(numOfSwitchesNeeded, minPossibleDifficultly);
        
        SpawnSwitches();
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
