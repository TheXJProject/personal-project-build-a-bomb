using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PanelInfo
{
    // Initialise In Inspector:
    public GameObject panel;
    public int size;
}

public class BoltingVarInfoAndSetup : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Initialise In Inspector:
    public PanelInfo[] panels;

    // Runtime Variables:
    bool setup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not setup yet
        setup = false;
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by BoltingLogic Script <br />
    /// Deletes the correct panels according to the set difficulty.
    /// </summary>
    public void SetupTwoo(int numbBoltsRequired)
    {
        // If we have not already set up the variation
        if (setup)
        {
            Debug.LogWarning("Error, bolt task variation is already setup!");
        }
        else
        {
            if (Msg) Debug.Log("Num Bolts to add: " + numbBoltsRequired);

            // Find the max amount of bolts
            foreach (PanelInfo panel in panels)
            {
                // If the panel exceeds size, it is removed
                if (numbBoltsRequired < panel.size)
                {
                    // Panel Removed
                    Remove(panel.panel);
                }
                else
                {
                    // Otherwise it is included
                    numbBoltsRequired -= panel.size;
                }
            }

            if (numbBoltsRequired != 0)
            {
                Debug.LogWarning("Error, not all possible positions used up! Remaining: " + numbBoltsRequired);
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Destroys given panel. <br />
    /// </summary>
    void Remove(GameObject panel)
    {
        // Deletes panel gameobject
        Destroy(panel);
    }
}
