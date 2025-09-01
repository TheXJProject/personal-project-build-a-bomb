using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bolt : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Inspector Adjustable Values:
    public float boltTime = 1;

    // Runtime Variables:
    [HideInInspector] public bool complete;
    bool boltingInProgress = false;
    float timeElapsed = 0f;
    BoltingLogic mainLogic;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not complete when just spawned
        complete = false;
        boltingInProgress = false;
        timeElapsed = 0f;
        mainLogic = gameObject.transform.parent.parent.parent.parent.GetComponent<BoltingLogic>();
    }

    private void Update()
    {
        // Check if the left button is pressed initially
        if (Input.GetMouseButton(0) && boltingInProgress && mainLogic.statInteract.isBeingSolvedAndSelected)
        {
            // TODO: Replace with call for animation!
            gameObject.GetComponent<Image>().color = Color.yellow;

            // Wait for set amount of time, checking if the button stays pressed
            if (timeElapsed < boltTime)
            {
                // Increment the time elapsed and continue
                timeElapsed += Time.deltaTime;
            }
            else
            {
                if (Msg) Debug.Log("Bolt completed.");

                // After a set amount of time, if the button is still held, log success
                complete = true;
                boltingInProgress = false;

                // TODO: Replace with call for animation!
                gameObject.GetComponent<Image>().color = Color.green;

                // Call check complete function
                mainLogic.CheckIfComplete();
            }
        }
        else
        {
            // If the player lets go of the bolt and it is not complete
            if (!complete)
            {
                // Reset the bolt
                ResetBolt();
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by the Bolt gameobject. When the player holds <br />
    /// left click on the Bolt a timer starts. <br />
    /// </summary>
    public void CompleteBolt(BaseEventData data)
    {
        // Check if the task can be solved
        if (mainLogic.statInteract.isBeingSolvedAndSelected && !complete)
        {
            PointerEventData newData = (PointerEventData)data;
            // Check left click is pressed
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Bolting in!");

                // Start checking that left click is held
                boltingInProgress = true;
            }
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
        boltingInProgress = false;
        timeElapsed = 0;

        // TODO: Replace with call for animation!
        gameObject.GetComponent<Image>().color = Color.red;
    }
}
