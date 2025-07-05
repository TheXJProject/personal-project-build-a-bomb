using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginMainMenu : MonoBehaviour
{
    // Event Actions:
    public static Action onBeginMainMenu;

    // Initialise In Inspector:
    [SerializeField] GameObject cameraObject;

    // Runtime Variables:
    [HideInInspector] public bool pressed = false;
    
    private void OnMouseDown()
    {
        if (!pressed)
        {
            onBeginMainMenu?.Invoke();
            pressed = true;
            cameraObject.GetComponent<Camera>().GetComponent<GeneralCameraLogic>().NewCameraSizeAndPosition(6f, 2, new Vector3(1, 3, -10));
        }
        else if (pressed)
        {
            pressed = false;
            cameraObject.GetComponent<Camera>().GetComponent<GeneralCameraLogic>().NewCameraSizeAndPosition(3.5f, 1, new Vector3(-1, -3, -10));
        }
    }
}
