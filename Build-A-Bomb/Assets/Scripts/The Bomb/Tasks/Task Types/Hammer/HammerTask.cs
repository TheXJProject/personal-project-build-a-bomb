using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HammerTask : MonoBehaviour
{
    [Header("(Don't Change!!)")]
    public int hardestDifficulty = 25;
    int numOfHitsNeeded = 5;
    int numOfHits = 0;
    
    TaskInteractStatus statInteract;
    
    private void Awake()
    {
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
        numOfHitsNeeded = (int)((hardestDifficulty * difficulty) + 0.5f);
    }
}
