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
    }
    private void OnEnable()
    {
        rightMouseDown = playerControls.Mouse.RightClick;
        rightMouseDown.Enable();
        rightMouseDown.performed += Deselect;

        task = GetComponent<TaskStatus>();
    }
    private void OnDisable()
    {
        rightMouseDown.Disable();
    }
    private void OnMouseDown()
    {
        task.TaskSelected();
    }

    private void Deselect(InputAction.CallbackContext context)
    {
        task.TaskDeselected();
    }
}
