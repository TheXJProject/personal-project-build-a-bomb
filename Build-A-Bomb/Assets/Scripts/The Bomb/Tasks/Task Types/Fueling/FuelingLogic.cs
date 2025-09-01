using UnityEngine;

public class FuelingLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const float maxPossibleDifficultly = 1000f;
    const float minPossibleDifficultly = 10f;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public float currentHardestDifficulty;
    [SerializeField] [Range(0, 1)] float upperLimit;
    [SerializeField] [Range(0, 1)] float lowerLimit;
    [SerializeField] [Range(-1, 0)] float bottomRL;
    [SerializeField] [Range(0, 1)] float topRL;
    [SerializeField] [Range(-1, 0)] float leftRL;
    [SerializeField] [Range(0, 1)] float rightRL;
    [SerializeField] float possibleWidthAndHeight;

    // Initialise In Inspector:
    [SerializeField] TaskInteractStatus statInteract;
    [SerializeField] ButtonLogic button;
    [SerializeField] RectTransform dock;
    [SerializeField] GameObject refueler;
    [SerializeField] GameObject canister;
    [SerializeField] RectTransform upper;
    [SerializeField] RectTransform lower;
    [SerializeField] RectTransform fuelLevel;

    // Runtime Variables:
    float fuelNeeded = minPossibleDifficultly;
    float currentFuel = 0;
    [HideInInspector] public Vector2 topBottomRefulerLimits;
    [HideInInspector] public Vector2 leftRightRefulerLimits;
    float maxFuel;
    float overFuelLimit;
    Vector2 refuelerStartPos;
    float canisterHeight;
    double amountOfCanisterNeededPerOneFuelUnit;
    bool isSetup;

    private void Awake()
    {
        // This instance is not set up yet
        isSetup = false;

        topBottomRefulerLimits = new Vector2(bottomRL * possibleWidthAndHeight, topRL * possibleWidthAndHeight);
        leftRightRefulerLimits = new Vector2(leftRL * possibleWidthAndHeight, rightRL * possibleWidthAndHeight);
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

    private void Update()
    {
        // Checks if the task can be solved
        if (statInteract.isBeingSolved)
        {
            // Set the completion level
            statInteract.SetTaskCompletion(Mathf.Clamp01(currentFuel / fuelNeeded));
            SetFuelLevel();

            // Change light colour depending on level of fuel
            if (currentFuel > fuelNeeded && currentFuel < overFuelLimit)
            {
                // Show that the button can be pressed
                button.SetGreenLight();
            }
            else if (currentFuel > 0)
            {
                // The fuel is changing
                button.SetYellowLight();
            }
            else
            {
                // No fuel in the tank
                button.SetRedLight();
            }
        }
    }

    private void FixedUpdate()
    {
        // Checks if the task can be solved
        if (statInteract.isBeingSolved)
        {
            // Increase or decrease fuel level
            if (refueler.GetComponent<RefuelerLogic>().docked) //refueler is in position
            {
                currentFuel++;
            }
            else
            {
                currentFuel--;
            }

            // Prevent overflow
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);

            if (Msg) Debug.Log("Fuel level: " + currentFuel);
        }
    }

    /// <summary>
    /// Called by Nail Head gameobject. When the player <br />
    /// clicks on the Nail Head the remaining number of <br />
    /// times the player needs to click is reduced by one.
    /// </summary>
    public void ButtonPressed()
    {
        // Checks if the task can be solved
        if (statInteract.isBeingSolved)
        {
            // Check if task is completed
            if (currentFuel > fuelNeeded && currentFuel < overFuelLimit)
            {
                // Set the completion level to 100 percent
                statInteract.SetTaskCompletion(1);

                // The task is complete
                statInteract.TaskCompleted();
            }
        }
    }

    void SetFuelLevel()
    {
        float newHeight = (float)amountOfCanisterNeededPerOneFuelUnit * currentFuel;

        // Update height
        fuelLevel.sizeDelta = new Vector2(fuelLevel.sizeDelta.x, newHeight);

        // Set Y position based on the new height
        fuelLevel.localPosition = new Vector3(fuelLevel.localPosition.x, -(canisterHeight - newHeight) / 2f, fuelLevel.localPosition.z);
    }

    /// <summary>
    /// Called by SetDifficulty method only! <br />
    /// Starts required setup for the task. <br />
    /// </summary>
    void SetupTask()
    {
        Vector3 upperPos = upper.localPosition;
        Vector3 lowerPos = lower.localPosition;

        // This function can only be activated once
        if (isSetup)
        {
            Debug.LogWarning("Error, this task is already set up!");
        }
        else
        {
            // This instance is now setup
            isSetup = true;

            // Error Check
            if (upperLimit < lowerLimit)
            {
                Debug.LogWarning("Error, lower limit is higher than upper limit!");
            }

            // Get starting pos for refueler
            refuelerStartPos = refueler.GetComponent<RectTransform>().localPosition;

            // Get canister height
            canisterHeight = canister.GetComponent<RectTransform>().rect.height;

            // Set max fuel and over limit levels
            maxFuel = fuelNeeded / lowerLimit;
            overFuelLimit = maxFuel * upperLimit;

            // Set the ratio of fuel quantity compared to the aesthetic size of the canister
            amountOfCanisterNeededPerOneFuelUnit = canisterHeight / (double)maxFuel;

            // Set aesthetic positions for the upper and lower limits
            upperPos.y = (float)((overFuelLimit * amountOfCanisterNeededPerOneFuelUnit) - 0.5f * canisterHeight);
            lowerPos.y = (float)((fuelNeeded * amountOfCanisterNeededPerOneFuelUnit) - 0.5f * canisterHeight);
            upper.localPosition = upperPos;
            lower.localPosition = lowerPos;
        }
    }

    /// <summary>
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

            // Sets difficulty level (amount of fuel needed in this case)
            fuelNeeded = currentHardestDifficulty * difficulty;

            // The total fuel needed cannot be zero
            fuelNeeded = Mathf.Max(fuelNeeded, minPossibleDifficultly);

            SetupTask();
        }
    }

    /// <summary>
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

            // Reset the current amount of fuel in the tank
            currentFuel = 0f;

            // Set correct colours
            button.SetRedLight();
            refueler.GetComponent<RefuelerLogic>().ResetFuelerDock();
            refueler.GetComponent<RefuelerLogic>().docked = false;

            // Put refueler back to start position
            refueler.GetComponent<RectTransform>().localPosition = refuelerStartPos;

            // Set the completion level
            statInteract.SetTaskCompletion(currentFuel / fuelNeeded);
            SetFuelLevel();
        }
    }
}
