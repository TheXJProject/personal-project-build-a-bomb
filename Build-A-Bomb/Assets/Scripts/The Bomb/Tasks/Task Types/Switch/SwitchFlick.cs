using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchFlick : MonoBehaviour
{
    public bool flicked = false;

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetSwitch;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetSwitch;
    }

    public void FlickSwitch(BaseEventData data)
    {
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            flicked = true;

            // Replace both with call for animation!!!!
            gameObject.GetComponent<Image>().color = Color.green;
            //
        }
    }

    public void ResetSwitch(GameObject trigger)
    {
        if (trigger == gameObject.transform.parent.parent)
        {
            flicked = false;

            // Replace both with call for animation!!!!
            gameObject.GetComponent<Image>().color = Color.red;
            //
        }
    }
}