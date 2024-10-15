using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bolt : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = true;

    // Inspector Adjustable Values:
    public float boltTime = 1;

    // Runtime Variables:
    [HideInInspector] public bool complete;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        complete = false;
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by the Bolt gameobject. When the player holds <br />
    /// left click on the Bolt a timer starts. <br />
    /// </summary>
    public void CompleteBolt(BaseEventData data)
    {
        // Check if the task can be solved
        if (gameObject.transform.parent.parent.parent.parent.GetComponent<BoltingLogic>().statInteract.isBeingSolved && !complete)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Bolting in!");

                StartCoroutine(CheckIfLeftButtonHeld());
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// If the player hold down left click for a set amount <br />
    /// time, boltTime, then the Bolt will become complete.
    /// </summary>
    IEnumerator CheckIfLeftButtonHeld()
    {
        // Check if the left button is pressed initially
        if (Input.GetMouseButton(0))
        {
            // TODO: Replace with call for animation!
            gameObject.GetComponent<Image>().color = Color.yellow;

            float timeElapsed = 0f;

            // Wait for set amount of time, checking if the button stays pressed
            while (timeElapsed < boltTime)
            {
                // If the task can't still be solved
                if (gameObject.transform.parent.parent.parent.parent.GetComponent<BoltingLogic>().statInteract.isBeingSolved)
                {
                    if (Msg) Debug.Log("Task isn't being solved.");

                    // TODO: Replace with call for animation!
                    gameObject.GetComponent<Image>().color = Color.red;

                    yield break;
                }

                // If at any point the condition becomes false, break out of the loop
                if (!Input.GetMouseButton(0))
                {
                    if (Msg) Debug.Log("Left click was not held for " + boltTime + " seconds.");

                    // TODO: Replace with call for animation!
                    gameObject.GetComponent<Image>().color = Color.red;
                    
                    yield break;
                }

                // Increment the time elapsed and continue
                timeElapsed += Time.deltaTime;

                // Wait for the next frame
                yield return null;
            }

            if (Msg) Debug.Log("Bolt completed.");

            // After a set amount of time, if the button is still held, log success
            complete = true;

            // TODO: Replace with call for animation!
            gameObject.GetComponent<Image>().color = Color.green;
        }
        else
        {
            Debug.LogWarning("Error, Left button was not pressed initially.");
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Resets the Bolt to uncomplete. <br />
    /// </summary>
    public void ResetBolt()
    {
        if (Msg) Debug.Log("Bolt Reset.");
        
        // This bolt is reset
        complete = false;

        // TODO: Replace with call for animation!
        gameObject.GetComponent<Image>().color = Color.red;
    }
}
