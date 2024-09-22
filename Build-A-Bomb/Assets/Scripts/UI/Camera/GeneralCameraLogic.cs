using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralCameraLogic : MonoBehaviour
{
    // Statistics of the camera to be set
    public float transitionTime = 0.3f;

    // Current information about the camera
    int currentLayer;
    float oldCameraSize;
    float currentCameraSize;
    float newCameraSize;
    float differenceCameraSize;
    float timeSinceSet;
    bool showingCorrectLayer = true;

    void Update()
    {
        // If we are not showing the correct layer, code sets camera to a new size determined by DetermineNewCameraSize()
        // Then if the transition time is over, it sets it equal to the exact new camera size value
        if (!showingCorrectLayer)
        {
            currentCameraSize = DetermineNewCameraSize();
            gameObject.GetComponent<Camera>().orthographicSize = currentCameraSize;

            if (timeSinceSet > transitionTime)
            {
                gameObject.GetComponent<Camera>().orthographicSize = newCameraSize;
                showingCorrectLayer = true;
            }
            timeSinceSet += Time.deltaTime;
        }
    }

    // InitialiseCameraSize() should be called before any calls to NewCameraSize()
    public void InitialiseCameraSize(float cameraSize, int layer)
    {
        currentLayer = layer;
        currentCameraSize = cameraSize;
        gameObject.GetComponent<Camera>().orthographicSize = cameraSize;
    }

    // Changes new, current and old camera sizes; resets the timer and finds the changes in size between the original and next camera size
    // Doesn't do anything if it determines it is already looking at the correct layer
    public void NewCameraSize(float cameraSize, int layer)
    {
        if (currentLayer == layer) return;
        currentLayer = layer;
        oldCameraSize = currentCameraSize;
        newCameraSize = cameraSize;
        timeSinceSet = 0f;
        differenceCameraSize = newCameraSize - oldCameraSize;
        showingCorrectLayer = false;
    }

    float DetermineNewCameraSize()
    {
        // Simply creates a linear transition from the old camera size to the new one
        // WE SHOULD PROBABLY REPLACE THIS LATER WITH A SMOOTHER TRANSITION
        return oldCameraSize + ((timeSinceSet / transitionTime) * differenceCameraSize);
    }
}
