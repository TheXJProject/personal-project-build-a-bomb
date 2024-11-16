using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ValveLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = true;

    // Constant Values:
    const int maxPossibleDifficultly = 2000;
    const int minPossibleDifficultly = 10;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;

    // Initialise In Inspector:
    [SerializeField] TaskInteractStatus statInteract;
    [SerializeField] Transform valve;

    // Runtime Variables:
    int valveResistanceTotal = minPossibleDifficultly;
    int valveResistancePassed = 0;
    Vector2 lastMousePos;
    bool isSetup;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // This instance is not set up yet
        isSetup = false;
    }

    private void OnEnable()
    {
        TaskInteractStatus.onTaskFailed += ResetTask;
        TaskInteractStatus.onTaskDifficultySet += SetDifficulty;
    }

    private void OnDisable()
    {
        TaskInteractStatus.onTaskFailed -= ResetTask;
        TaskInteractStatus.onTaskDifficultySet -= SetDifficulty;
    }


    private void FixedUpdate()
    {
        if (statInteract.isBeingSolved)
        {
            CheckMouseSpeed();
        }
    }


    /// <summary>
    /// Temp
    /// </summary>
    public void MoveValve()
    {
        valve.rotation *= Quaternion.Euler(0, 0, 10);
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by Nail Head gameobject. When the player <br />
    /// clicks on the Nail Head the remaining number of <br />
    /// times the player needs to click is reduced by one.
    /// </summary>
    float CheckMouseSpeed()
    {
        // Get current mouse position
        Vector2 currentMousePos = Input.mousePosition;

        // Calculate the difference in position
        Vector2 mousePosDifference = currentMousePos - lastMousePos;

        // Calculate mouse speed using magnitude of the position difference
        float mouseSpeed = mousePosDifference.magnitude / Time.deltaTime;

        if (Msg) Debug.Log("Mouse Speed is: " + mouseSpeed);

        // Set new previous mouse position
        lastMousePos = currentMousePos;

        // Return the speed of the mouse
        return mouseSpeed;
    }

    // TODO: Create Function for detecting movement of mouse

    ///// FUNCTION DESCRIPTION <summary>
    ///// Called by Nail Head gameobject. When the player <br />
    ///// clicks on the Nail Head the remaining number of <br />
    ///// times the player needs to click is reduced by one.
    ///// </summary>
    //public void NailHit(BaseEventData data)
    //{
    //    if (Msg) Debug.Log("Called function");

    //    // Checks if the task can be solved
    //    if (statInteract.isBeingSolved)
    //    {
    //        if (Msg) Debug.Log("Task is being solved");
    //        PointerEventData newData = (PointerEventData)data;
    //        if (newData.button.Equals(PointerEventData.InputButton.Left))
    //        {
    //            if (Msg) Debug.Log("Left click is being pressed");

    //            // Increases the total number of times Nail Head has been hit by one
    //            numOfHits++;

    //            // Set the completion level
    //            statInteract.SetTaskCompletion((float)numOfHits / numOfHitsNeeded);

    //            // Check if task is completed
    //            if (numOfHits >= numOfHitsNeeded)
    //            {
    //                statInteract.TaskCompleted();
    //            }
    //        }
    //    }
    //}

    /// FUNCTION DESCRIPTION <summary>
    /// Called by SetDifficulty method only! <br />
    /// Starts required setup for the task. <br />
    /// </summary>
    void SetupTask()
    {
        // This function can only be activated once
        if (isSetup)
        {
            Debug.LogWarning("Error, this task is already set up!");
        }
        else
        {
            // This instance is now setup
            isSetup = true;
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskDifficultySet event only! <br />
    /// Retrieves a difficult setting and applies it to this task <br />
    /// instance. Then calls for the task to be setup.
    /// </summary>
    void SetDifficulty(GameObject triggerTask)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (triggerTask == gameObject.transform.parent.gameObject)
        {
            if (Msg) Debug.Log("Set Difficultly");

            // Retrieves difficulty
            float difficulty = triggerTask.GetComponent<TaskStatus>().difficulty;

            // Sets difficulty level (the number of hits needed in this case)
            valveResistanceTotal = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of hits needed cannot be zero
            valveResistanceTotal = Mathf.Max(valveResistanceTotal, minPossibleDifficultly);

            SetupTask();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by onTaskFailed event only! <br />
    /// Resets the task back to its state just after SetupTask <br />
    /// has been called.
    /// </summary>
    void ResetTask(GameObject trigger)
    {
        // When the onTaskDifficultySet event is called, check whether the triggering gameobject is itself
        if (trigger == gameObject)
        {
            if (Msg) Debug.Log("Reset Task");

            // Reset the number of times the player has hit
            valveResistancePassed = 0;

            // TODO: Reset Valve position and any effects (like vibration)
        }
    }
}
