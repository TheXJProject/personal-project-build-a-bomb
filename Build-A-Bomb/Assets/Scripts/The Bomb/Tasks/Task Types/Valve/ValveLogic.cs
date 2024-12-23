using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ValveLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 20000;
    const int minPossibleDifficultly = 100;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;
    [SerializeField] [Range(0.00001f,0.1f)] float valveVisualSpeed;

    // Initialise In Inspector:
    [SerializeField] TaskInteractStatus statInteract;
    [SerializeField] GameObject valve;

    // Runtime Variables:
    int valveResistanceTotal = minPossibleDifficultly;
    int valveResistancePassed = 0;
    Vector3 lastMouseWorldPos = Vector3.zero;
    Vector2 lastMousePos = Vector2.zero;
    Vector3 valvePos = Vector3.zero;
    bool holdingValve = false;
    Quaternion startingRotation;
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
        // If we are holding left click and we can complete the task and we are holding the valve
        if (Input.GetMouseButton(0) && statInteract.isBeingSolved && holdingValve)
        {
            // Move the valve a set amount and adjust completeness level
            ValveCompletenessCheck(MoveValve(CheckMouseSpeed()));

            // TODO: reduce vibration animation depending on valveResistancePassed

            // If we are not over the valve or left click is not held
            if (!valve.GetComponent<MousePositionLogic>().isMouseOver || !Input.GetMouseButton(0))
            {
                // We are no londer holding the valve
                holdingValve = false;
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by valve is clicked on. <br />
    /// </summary>
    public void HoldValve(BaseEventData data)
    {
        if (Msg) Debug.Log("Holding Mouse!");

        // If left click was pressed
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // We are holding the valve
            holdingValve = true;
            // Rest mouse position
            lastMousePos = Input.mousePosition;
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Rotates the valve by an amount calculated using mouse speed. <br />
    /// </summary>
    float MoveValve(float mouseSpeed)
    {
        float moveAmount;

        // If we are less than 50% complete
        if (((float)valveResistancePassed / (float)valveResistanceTotal) <= (1f / 2f))
        {
            // Valve move amount is set depending on mouse speed
            moveAmount = valveVisualSpeed * (1f / 2f) * mouseSpeed;
        }
        else
        {
            // Factorially decrease the amount the valve rotates depending on completeness
            moveAmount = valveVisualSpeed * (1f - ((float)valveResistancePassed / (float)valveResistanceTotal)) * mouseSpeed;
            
            // If completeness is over three quaters
            if (((float)valveResistancePassed / (float)valveResistanceTotal) >= (3f / 4f))
            {
                // If in the last 2%
                if (((float)valveResistancePassed / (float)valveResistanceTotal) >= (98f / 100f))
                {
                    moveAmount = 0f;
                }
                else
                {
                    // increase visual resistance
                    moveAmount *= (1 - (((float)valveResistancePassed / (float)valveResistanceTotal) - 0.75f) * 0.9f);
                }
            }
        }

        // Apply rotation
        valve.transform.rotation *= Quaternion.Euler(0, 0, moveAmount);

        // Returns back inputted mouse speed
        return mouseSpeed;
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Gets the current mouse position and compares it <br />
    /// with the previous mouse position. Then calculates <br />
    /// the speed of the mouse.
    /// </summary>
    float CheckMouseSpeed()
    {
        // Get current mouse position
        Vector2 currentMousePos = Input.mousePosition;

        // Get current mouse world position
        Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(currentMousePos);
        currentMouseWorldPos.z = 0f;

        // Get normalised vector of current and previous mouse position
        Vector3 valveCurrentMouseVec = (currentMouseWorldPos - valvePos).normalized;
        Vector3 valveLastMouseVec = (lastMouseWorldPos - valvePos).normalized;

        //Normalise mouse positions
        currentMousePos.x /= ((float)Screen.width * 0.01f);
        currentMousePos.y /= ((float)Screen.height * 0.01f);
        lastMousePos.x /= ((float)Screen.width * 0.01f);
        lastMousePos.y /= ((float)Screen.height * 0.01f);

        // Get cross product which points 90 degrees to the left of the vector towards last mouse pos
        Vector3 crossProduct = Vector3.Cross(valveLastMouseVec, Vector3.back);

        // Use dot product to determine if the new mouse pos is in the ccw direction of the old one
        float dotProduct = Vector3.Dot(crossProduct, valveCurrentMouseVec);

        // Mouse speed is zero unless player is moving mouse in correct direction
        float mouseSpeed = 0;
        
        // If the player is moving the mouse in the correct direction
        if (dotProduct > 0)
        {
            // Calculate the difference in position
            Vector2 mousePosDifference = currentMousePos - lastMousePos;

            // Calculate mouse speed using magnitude of the position difference
            mouseSpeed = mousePosDifference.magnitude / Time.deltaTime;
        }

        if (Msg) Debug.Log("Mouse Speed is: " + mouseSpeed);

        // Set new previous mouse position
        lastMousePos = Input.mousePosition;

        // Set new previous mouse world position
        lastMouseWorldPos = currentMouseWorldPos;

        // Return the speed of the mouse
        return mouseSpeed;
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Checks for valve completeness level. <br />
    /// </summary>
    void ValveCompletenessCheck(float mouseSpeed)
    {
        // Checks if the task can be solved
        if (statInteract.isBeingSolved)
        {
            // Increase amount of valve has been completed
            valveResistancePassed += (int)(mouseSpeed / 30);

            if (Msg) Debug.Log("Task is being solved. Completeness: " + valveResistancePassed + " Out of: " + valveResistanceTotal);

            // Set the completion level
            statInteract.SetTaskCompletion((float)valveResistancePassed / valveResistanceTotal);

            // Check if task is completed
            if (valveResistancePassed >= valveResistanceTotal)
            {
                statInteract.TaskCompleted();
            }
        }
    }

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

            startingRotation = valve.transform.rotation;

            // TODO: Start Vibration animation
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

            // The number needed cannot be zero
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

            holdingValve = false;

            valve.transform.rotation = startingRotation;

            // TODO: Reset any effects (like vibration)
        }
    }
}
