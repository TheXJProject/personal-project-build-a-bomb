using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLogic : MonoBehaviour
{
    int hardestDifficulty = 20;
    int numOfSwitches;
    
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

    //public void NailHit(BaseEventData data)   ---- This comment is if you are using EventTrigger components
    //{
    //  PointerEventData newData = (PointerEventData)data;
    //  if (newData.button.Equals(PointerEventData.InputButton.Left))
    //  {
    //
    //  }
    //}

    void ResetTask()
    {
        
    }

    void SetDifficulty(float difficulty)
    {
        numOfSwitches = (int)((hardestDifficulty * difficulty) + 0.5f);
    }
}
