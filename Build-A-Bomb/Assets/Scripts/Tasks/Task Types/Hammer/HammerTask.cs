using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HammerTask : MonoBehaviour
{
    public int numOfHitsNeeded = 5;
    public int numOfHits = 0;
    
    TaskInteractStatus statInteract;
    
    private void Awake()
    {
        statInteract = GetComponent<TaskInteractStatus>();
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetTask;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetTask;
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

    void ResetTask()
    {
        Debug.Log("Reseting");
        numOfHits = 0;
    }
}
