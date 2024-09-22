using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HammerTask : MonoBehaviour
{
    readonly bool Msg = false; // ==== For Debugging! ====

    const int maxPossibleDifficultly = 150;
    const int minPossibleDifficultly = 1;
    [Range(minPossibleDifficultly, maxPossibleDifficultly)]
    public int currentHardestDifficulty;

    int numOfHitsNeeded = minPossibleDifficultly;
    int numOfHits = 0;
    
    TaskInteractStatus statInteract;
    
    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");
        statInteract = GetComponent<TaskInteractStatus>();
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

    public void NailHit(BaseEventData data)
    {
        if (statInteract.isBeingSolved)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                numOfHits++;
                statInteract.SetTaskCompletion((float)numOfHits / numOfHitsNeeded);
                if (numOfHits >= numOfHitsNeeded)
                {
                    statInteract.TaskCompleted();
                }
            }
        }
    }

    void SetDifficulty(GameObject triggerTask)
    {
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Set Difficultly");
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty;
            numOfHitsNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);
            numOfHitsNeeded = Mathf.Max(numOfHitsNeeded, minPossibleDifficultly);
        }
    }

    void ResetTask(GameObject trigger)
    {
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");
            numOfHits = 0;
        }
    }
}
