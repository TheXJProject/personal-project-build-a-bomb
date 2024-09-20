using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchLogic : MonoBehaviour
{
    [SerializeField]
    GameObject switchObject;

    [Header("(Don't Change!!)")]
    public int hardestDifficulty = 20;
    int numOfSwitches;
    int numFlickedSwitches = 0;
    
    TaskInteractStatus statInteract;

    private void Awake()
    {
        statInteract = GetComponent<TaskInteractStatus>();
        SpawnSwitches();
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

    public void Swit(BaseEventData data)
    {
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {

        }
    }

    void SpawnSwitches()
    {
        Instantiate(switchObject, Vector2.zero, Quaternion.identity, transform.GetChild(0).transform);
    }

    void ResetTask(GameObject trigger)
    {
        if (trigger == gameObject)
        {

        }
    }

    void SetDifficulty(float difficulty)
    {
        numOfSwitches = (int)((hardestDifficulty * difficulty) + 0.5f);
    }
}
