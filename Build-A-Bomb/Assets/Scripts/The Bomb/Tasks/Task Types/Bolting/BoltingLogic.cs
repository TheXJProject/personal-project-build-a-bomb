using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltingLogic : MonoBehaviour
{
    readonly bool Msg = true; // ==== For Debugging! ====

    const int maxPossibleDifficultly = 6;
    const int minPossibleDifficultly = 1;
    [Range(minPossibleDifficultly, maxPossibleDifficultly)]
    public int currentHardestDifficulty = maxPossibleDifficultly;

    int numOfLayersNeeded = minPossibleDifficultly;
    int numOfBoltsNeeded;
    int numOfBolts = 0;

    TaskInteractStatus statInteract;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");
        statInteract = GetComponent<TaskInteractStatus>();
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetTask;
        TaskInteractStatus.onChangeTaskDifficulty += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetTask;
        TaskInteractStatus.onChangeTaskDifficulty -= SetDifficulty;
    }

    public void NailHit(BaseEventData data)
    {
        if (statInteract.isBeingSolved)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                numOfHits++;
                statInteract.SetTaskCompletion(numOfHits / numOfHitsNeeded);
                if (numOfHits >= numOfHitsNeeded)
                {
                    statInteract.TaskCompleted();
                }
            }
        }
    }

    void ResetTask(GameObject trigger)
    {
        if (trigger == gameObject)
        {
            numOfHits = 0;
        }
    }

    void SetDifficulty(float difficulty)
    {
        numOfHitsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);
        numOfHitsNeeded = Mathf.Max(numOfHitsNeeded, minPossibleDifficultly);
    }
}
