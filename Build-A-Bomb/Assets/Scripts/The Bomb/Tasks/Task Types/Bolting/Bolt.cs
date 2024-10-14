using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bolt : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = false;

    // Inspector Adjustable Values:
    public float boltTime = 1;

    // Runtime Variables:
    bool complete;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        complete = false;
    }

    // TODO: Function description after written correctly

    /// FUNCTION DESCRIPTION <summary>
    /// Called by the Bolt gameobject. When the player holds <br />
    /// left click on the Bolt the remaining number of <br />
    /// times the player needs to click is reduced by one.
    /// </summary>
    public void NailHit(BaseEventData data)
    {
        // Checks if the task can be solved
        if (true /*Can be solved*/)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Bolting in!");

                // start coroutine, checks if holding
            }
        }
    }
}
