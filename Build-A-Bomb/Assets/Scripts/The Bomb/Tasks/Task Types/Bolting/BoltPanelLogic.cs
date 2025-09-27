using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoltPanelLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Initialise In Inspector:
    [SerializeField] GameObject panel;
    public GameObject[] bolts;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // Set each Gameobject to inactive
        panel.SetActive(false);

        // Each bolt inactive
        foreach (GameObject bolt in bolts)
        {
            bolt.SetActive(false);
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by Gameobject. Reveals the panel and bolts. <br />
    /// </summary>
    public void ShowPanelAndBolts(BaseEventData data)
    {
        PointerEventData newData = (PointerEventData)data;
        // Check player is using left click
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            if (Msg) Debug.Log("Panel being shown.");

            PlaySoundBasedOnSize();

            // Set each Gameobject to active
            panel.SetActive(true);

            // Each bolt active
            foreach (GameObject bolt in bolts)
            {
                bolt.SetActive(true);
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Rehides the panel and bolts. <br />
    /// </summary>
    public void ResetPanel()
    {
        if (Msg) Debug.Log("Panel Reset.");

        // Set each Gameobject to inactive
        panel.SetActive(false);

        // Reset and hide each bolt
        foreach (GameObject bolt in bolts)
        {
            // Reset each bolt
            bolt.GetComponent<Bolt>().ResetBolt();

            // Each bolt inactive
            bolt.SetActive(false);
        }
    }

    void PlaySoundBasedOnSize()
    {
        if (!panel.activeSelf)
        {
            // Get size of pannel
            int size = bolts.GetLength(0);

            // Adjust the pitch to be used
            float pitch = 1.15f - size * 0.04f;

            // Play sound, non-priority, using default volume, with a set pitch
            AudioManager.instance.PlaySFX("Place Pannel", false, null, false, pitch);
        }
    }
}
