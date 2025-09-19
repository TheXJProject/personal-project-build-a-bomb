using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskSelection : MonoBehaviour
{
    // Inspector Adjustable Values:
    InputAction rightMouseDown;
    TaskStatus task;

    // Runtime Variables
    [HideInInspector] public bool assignedPlayerControls = false;

    private void Awake()
    {
        task = GetComponent<TaskStatus>();
    }
    private void OnEnable()
    {
        LayerButtonPress.onLayerButtonPressed += PressLayerButtonDeselect;
        PlayerKeyInput.instance.playerControls.Mouse.RightClick.performed += RightClickDeselect;
    }
    private void OnDisable()
    {
        LayerButtonPress.onLayerButtonPressed -= PressLayerButtonDeselect;
        PlayerKeyInput.instance.playerControls.Mouse.RightClick.performed -= RightClickDeselect;
    }

    private void OnMouseDown()
    {
        // Selects a task when it is clicked
        if (task.isOnCurrentLayer)
        {
            task.TaskSelected();
        }
    }

    /// <summary>
    /// Called when right click is pressed, deselects the task
    /// </summary>
    private void RightClickDeselect(InputAction.CallbackContext context)
    {
        task.TaskDeselected();
    }

    /// <summary>
    /// Called when a layer button is pressed, deselects the task
    /// </summary>
    private void PressLayerButtonDeselect(GameObject triggerLayer)
    {
        task.TaskDeselected();
    }
}
