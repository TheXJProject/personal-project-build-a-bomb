using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchFlick : MonoBehaviour
{
    public bool flicked = false;

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

    public void ResetSwitch()
    {
        flicked = false;

        // Replace both with call for animation!!!!
        gameObject.GetComponent<Image>().color = Color.red;
        //
    }
}
