using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskSelection : MonoBehaviour
{
    public PlayerInputActions playerControls;

    InputAction rightMouseDown;
    TaskStatus task;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        task = GetComponent<TaskStatus>();
    }
    private void OnEnable()
    {
        LayerButtonPress.onLayerButtonPressed += PressLayerButtonDeselect;

        rightMouseDown = playerControls.Mouse.RightClick;
        rightMouseDown.Enable();
        rightMouseDown.performed += RightClickDeselect;
    }
    private void OnDisable()
    {
        LayerButtonPress.onLayerButtonPressed -= PressLayerButtonDeselect;
        rightMouseDown.Disable();
    }
    private void OnMouseDown()
    {
        if (task.isOnCurrentLayer)
        {
            task.TaskSelected();
        }
    }

    private void RightClickDeselect(InputAction.CallbackContext context)
    {
        task.TaskDeselected();
    }

    private void PressLayerButtonDeselect(GameObject triggerLayer)
    {
        task.TaskDeselected();
    }
}
