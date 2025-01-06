using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyDisplayVisuals : MonoBehaviour
{
    // Initialise in Inspector
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] Image textBack;

    // Initialise in Inspector
    [SerializeField] Color heldSelectedCol;
    [SerializeField] Color heldUnselectedCol;

    // Runtimate Variables:
    GameObject taskRepresented;
    bool isSelected;
    int keyDisplayed;
    Color originalCol;

    private void Awake()
    {
        originalCol = textBack.color;
    }

    private void OnEnable()
    {
        TaskStatus.onTaskFailed += DetermineOnFail;
        TaskStatus.onTaskSelected += DetermineOnSelection;
        TaskStatus.onTaskDeSelected += DetermineOnSelection;
        TaskStatus.onTaskCompleted += DetermineOnSuccess;
        PlayerKeyInput.onKeyPressed += DetermineOnPress;
        PlayerKeyInput.onKeyReleased += DetermineOnRelease;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskFailed -= DetermineOnFail;
        TaskStatus.onTaskSelected -= DetermineOnSelection;
        TaskStatus.onTaskDeSelected -= DetermineOnSelection;
        TaskStatus.onTaskCompleted -= DetermineOnSuccess;
        PlayerKeyInput.onKeyPressed -= DetermineOnPress;
        PlayerKeyInput.onKeyReleased -= DetermineOnRelease;
    }

    public void AssignTaskLetter(GameObject task, int whichKey)
    {
        keyDisplayed = whichKey;
        taskRepresented = task;
        isSelected = true;
        textDisplay.text = PlayerKeyInput.instance.alphabet[whichKey];
    }

    public void DetermineOnSelection(GameObject triggerTask)
    {
        isSelected = taskRepresented.GetComponent<TaskStatus>().isSelected;
        if (!isSelected && !taskRepresented.GetComponent<TaskStatus>().isBeingSolved)
        {
            UnselectDisplay();
        }
        else if (!isSelected) textBack.color = heldUnselectedCol;
        else if (isSelected) textBack.color = heldSelectedCol;
    }

    public void DetermineOnFail(GameObject triggerTask)
    {
        if (triggerTask == taskRepresented)
        {
            if (!isSelected) UnselectDisplay();
        }
    }

    public void DetermineOnSuccess(GameObject triggerTask)
    {
        if (triggerTask == taskRepresented) UnselectDisplay();
    }

    public void DetermineOnPress(int triggerKey)
    {
        if (triggerKey == keyDisplayed)
        {
            if (isSelected) textBack.color = heldSelectedCol;
        }
    }

    public void DetermineOnRelease(int triggerKey)
    {
        if (triggerKey == keyDisplayed)
        {
            if (isSelected) textBack.color = originalCol;
        }
    }

    void UnselectDisplay()
    {
        textBack.color = originalCol;
        gameObject.SetActive(false);
    }
}
