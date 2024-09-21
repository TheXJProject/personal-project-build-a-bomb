using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchFlick : MonoBehaviour
{
    readonly bool Msg = true; // ==== For Debugging! ====

    public bool flicked = false;

    public void FlickSwitch(BaseEventData data)
    {
        if (Msg) Debug.Log("FlickedSwitch Called.");
        gameObject.transform.parent.parent.GetComponent<SwitchLogic>().CheckSwitches();

        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            if (gameObject.transform.parent.parent.GetComponent<SwitchLogic>().canBeSolved)
            {
                flicked = true;

                // Replace both with call for animation!!!!
                gameObject.GetComponent<Image>().color = Color.green;
                //
            }
            
            if (Msg) Debug.Log("Switch clicked with left click. Switch has been flicked: " + flicked);
        }
        
        gameObject.transform.parent.parent.GetComponent<SwitchLogic>().CheckSwitches();
    }

    public void ResetSwitch()
    {
        flicked = false;

        // Replace both with call for animation!!!!
        gameObject.GetComponent<Image>().color = Color.red;
        //
    }
}