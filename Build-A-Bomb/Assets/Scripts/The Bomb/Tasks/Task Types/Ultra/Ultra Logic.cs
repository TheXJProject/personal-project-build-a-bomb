using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltraLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Constant Values:
    const int maxPossibleDifficultly = 15;
    const int minPossibleDifficultly = 1;

    // Inspector Adjustable Values:
    [Range(minPossibleDifficultly, maxPossibleDifficultly)] public int currentHardestDifficulty;

    // Initialise In Inspector:
    [SerializeField] TaskInteractStatus statInteract;
    [SerializeField] Image lightColour;

    [SerializeField] RectTransform energyLevelLeft;
    [SerializeField] RectTransform energyLevelOutlineLeft;
    [SerializeField] RectTransform energyLevelRight;
    [SerializeField] RectTransform energyLevelOutlineRight;

    [SerializeField] RectTransform energyLevelBehindLeft;
    [SerializeField] RectTransform energyLevelBehindRight;

    [SerializeField] float energyOutlineWidth;

    [SerializeField] RectTransform energyLevelBack;

    [SerializeField] Sprite lightOff;
    [SerializeField] Sprite lightOn;

    [SerializeField] Image energyGlow;
    [SerializeField] Image buttonGlow;

    // Runtime Variables:
    int amountOfEnergyNeeded = minPossibleDifficultly;
    int currentEnergy = 0;
    float widthPerEnergy;
    float energyMaxWidth;
    bool playingSound = false;

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

    private void Update()
    {
        if (statInteract.isBeingSelected && isSetup)
        {
            // If we are not playing sound
            if (!playingSound)
            {
                // If you can see the task, start playing the background noise
                AudioManager.instance.PlayLoopingSFX("Ultra", AudioSettings.dspTime + 0.1);
                
                MixerFXManager.instance.ForceSetParam(GROUP_OPTIONS.LOOPING_SFX, EX_PARA.VOLUME, 0);
                MixerFXManager.instance.SetLoopingSFXParam("Ultra", EX_PARA.VOLUME, 4f);

                playingSound = true;
            }
        }
        else
        {
            // If we are playing sound
            if (playingSound)
            {
                // If you can't see the the task, stop the noise
                AudioManager.instance.StopLoopingSFX("Ultra");
                MixerFXManager.instance.ForceSetParam(GROUP_OPTIONS.LOOPING_SFX, EX_PARA.VOLUME);
                playingSound = false;
            }
        }

        // Checks if the task can be solved
        if (statInteract.isBeingSolvedAndSelected)
        {

            // Set energy level
            currentEnergy = CheckActiveTasks();

            if (Msg) Debug.Log(currentEnergy);

            // Set the colour of the light depending on the current energy
            if (currentEnergy >= amountOfEnergyNeeded)
            {
                lightColour.sprite = lightOn;
            }
            else if (currentEnergy > 0)
            {
                lightColour.sprite = lightOff;
            }
            else
            {
                lightColour.sprite = lightOff;
            }

            // TODO: The energy level should look animated and cool and electricccyyish if ygm
            SetEnergyVisual();

            // Set the completion level
            statInteract.SetTaskCompletion((float)currentEnergy / amountOfEnergyNeeded);
        }
    }

    public void AttemptFinish(BaseEventData data)
    {
        // Checks if the task can be solved
        if (statInteract.isBeingSolvedAndSelected)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Current Energy: " + currentEnergy);
                if (Msg) Debug.Log("Required Energy: " + amountOfEnergyNeeded);

                // Check if task is completed
                if (currentEnergy >= amountOfEnergyNeeded)
                {
                    statInteract.TaskCompleted();
                }
            }
        }
    }

    int CheckActiveTasks()
    {
        // Get number of active tasks, should be clamped between 0 and amountOfEnergyNeeded
        int tasksNum = Mathf.Clamp(PlayerKeyInput.instance.keysInUseTotal - 1, 0, amountOfEnergyNeeded);
        return tasksNum;
    }

    void SetEnergyVisual()
    {
        float newWidth = (float)widthPerEnergy * currentEnergy;

        // Update width
        //energyLevel.sizeDelta = new Vector2(newWidth, energyLevel.sizeDelta.y);
        float energyFillAmount = (newWidth / energyMaxWidth);

        float adjustedFillAmount = (energyFillAmount * energyFillAmount * energyFillAmount * energyFillAmount);

        Color glowCol = energyGlow.color;
        glowCol.a = adjustedFillAmount;
        energyGlow.color = glowCol;

        glowCol = buttonGlow.color;
        glowCol.a = adjustedFillAmount;
        buttonGlow.color = glowCol;

        energyLevelLeft.GetComponent<Image>().fillAmount = energyFillAmount;
        energyLevelOutlineLeft.GetComponent<Image>().fillAmount = energyFillAmount;
        energyLevelRight.GetComponent<Image>().fillAmount = energyFillAmount;
        energyLevelOutlineRight.GetComponent<Image>().fillAmount = energyFillAmount;

        float energyExtraWidth = ((energyFillAmount < 0.999) && (energyFillAmount > 0.001)) ? energyOutlineWidth : 0.0f; 

        energyLevelBehindLeft.GetComponent<Image>().fillAmount = energyFillAmount + energyExtraWidth;
        energyLevelBehindRight.GetComponent<Image>().fillAmount = energyFillAmount + energyExtraWidth;

        // Set X position based on the new width
        //energyLevel.localPosition = new Vector3(-(energyMaxWidth - newWidth) / 2f, energyLevel.localPosition.y, energyLevel.localPosition.z);
    }

    /// <summary>
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
            // Set energy width levels
            energyMaxWidth = energyLevelBack.rect.width;
            widthPerEnergy = energyMaxWidth / amountOfEnergyNeeded;

            SetEnergyVisual();

            // This instance is now setup
            isSetup = true;
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

            // Sets difficulty level (the number of hits needed in this case)
            amountOfEnergyNeeded = (int)((currentHardestDifficulty * difficulty) + 0.5f);

            // The number of hits needed cannot be zero
            amountOfEnergyNeeded = Mathf.Max(amountOfEnergyNeeded, minPossibleDifficultly);

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

            // Reset the current energy
            currentEnergy = 0;

            // Set to inactive
            lightColour.sprite = lightOff;

            SetEnergyVisual();

            // Set the completion level
            statInteract.SetTaskCompletion((float)currentEnergy / amountOfEnergyNeeded);
        }
    }
}
