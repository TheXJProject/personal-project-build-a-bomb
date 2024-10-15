using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltPanelLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = true;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        complete = false;
    }
}
