using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchFlick : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;
    [SerializeField] GameObject snap;
    [SerializeField] Image switchImage;
    [SerializeField] Sprite switchOn;
    [SerializeField] Sprite switchOff;

    // Runtime Variables:
    [HideInInspector] public bool flicked = false;

    /// FUNCTION DESCRIPTION <summary>
    /// This is called when the player clicks on this switch gameobject <br />
    /// instance. If the task canBeSolved then this switch becomes flicked. <br />
    /// It also calls to check which other switches are flicked.
    /// </summary>
    public void FlickSwitch(BaseEventData data)
    {
        if (Msg) Debug.Log("FlickedSwitch Called.");

        // Checks which switches are flicked
        gameObject.transform.parent.parent.parent.GetComponent<SwitchLogic>().CheckSwitches(); // I had to add another parent to this and it hurt my soul, we will never code like this again

        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // Are we able to solve this task
            if (gameObject.transform.parent.parent.parent.GetComponent<SwitchLogic>().canBeSolved) // Oh god I have to do it again....
            {
                // The switch is now flicked on
                if (!flicked) snap.SetActive(true);
                flicked = true;

                // TODO: Replace with call for animation!
                switchImage.sprite = switchOn;
            }
            
            if (Msg) Debug.Log("Switch clicked with left click. Switch has been flicked: " + flicked);
        }
        
        // Checks which switches are flicked again
        gameObject.transform.parent.parent.parent.GetComponent<SwitchLogic>().CheckSwitches(); // Please... make it stop....
    }

    /// FUNCTION DESCRIPTION <summary>
    /// This is called when the task is reset. It resets this switch <br />
    /// instance back to off. <br />
    /// </summary>
    public void ResetSwitch()
    {
        // The switch is now flicked off
        flicked = false;

        // TODO: Replace with call for animation!
        switchImage.sprite = switchOff;
    }
}