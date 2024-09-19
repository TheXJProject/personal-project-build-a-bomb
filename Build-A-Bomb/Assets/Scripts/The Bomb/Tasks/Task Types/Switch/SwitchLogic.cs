using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLogic : MonoBehaviour
{
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
}
