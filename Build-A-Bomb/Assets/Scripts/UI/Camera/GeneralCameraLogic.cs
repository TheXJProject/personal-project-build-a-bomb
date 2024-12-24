using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralCameraLogic : MonoBehaviour
{
    // Inspector Adjustable Values:
    public float transitionTime = 0.3f;

    // Runtime Variables:
    int currentLayer;
    float oldCameraSize;
    float currentCameraSize;
    float newCameraSize;
    float differenceCameraSize;
    float timeSinceSet;
    bool showingCorrectLayer = true;

    void Update()
    {
        // If we are not showing the correct layer
        if (!showingCorrectLayer)
        {
            // Set the camera to what the current camera size should currently 
            currentCameraSize = DetermineNewCameraSize();
            gameObject.GetComponent<Camera>().orthographicSize = currentCameraSize;

            // Once the transition is finished
            if (timeSinceSet > transitionTime)
            {
                // Set the size of the camera to the exact size it should be, then set it to be showing the correct layer
                gameObject.GetComponent<Camera>().orthographicSize = newCameraSize;
                showingCorrectLayer = true;
            }

            // Increase the timer
            timeSinceSet += Time.deltaTime;
        }
    }

    /// <summary>
    /// Should be called before any call to NewCameraSize(). This function sets up the camera's properties e.g. layer
    /// </summary>
    public void InitialiseCameraSize(float cameraSize, int layer)
    {
        currentLayer = layer;
        currentCameraSize = cameraSize;
        gameObject.GetComponent<Camera>().orthographicSize = cameraSize;
    }

    /// <summary>
    /// Changes new, current and old camera sizes; resets the timer for camera transitions and finds the changes in <br />
    /// size between the original and next camera size. Doesn't do anything if it determines it is already looking at <br />
    /// the correct layer. Call this when the camera needs to be resized
    /// </summary>
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

    /// <summary>
    /// Function which determines what the camera size should be based on the its current, old and new camera properties and <br />
    /// the time since the last NewCameraSize() call
    /// </summary>
    float DetermineNewCameraSize()
    {
        // TODO: The camera transition currently determines current camera size based on a linear transition. The variables used to
        // determine camera size are below, they should be used to determine a size that transitions logarithmically through the sizes,
        // since the difference in size between layers increases by a specific factor each time (This is a better transition because
        // when we transition linearly in size, it appears to change size quickly when more zoomed in, and slowly when more zoomed out)
        //  - oldCameraSize         -> the size of the camera at the start of the transition
        //  - differenceCameraSize  -> the change in size from oldCameraSize to the new size (positive or negative depending on whether the camera is shrinking or growing)
        //  - timeSinceSet          -> the time in seconds since the camera changed size
        //  - transitionTime        -> the time it should take for a transition to be completed

        // Set equation parameters
        float a = oldCameraSize;
        float b = (3 * differenceCameraSize) / (Mathf.Pow(transitionTime, 2));
        float c = (-2 * differenceCameraSize) / (Mathf.Pow(transitionTime, 3));

        // x is what changes
        float x = timeSinceSet;

        // Return value from new 3rd order equation
        return a + b * Mathf.Pow(x, 2) + c * Mathf.Pow(x, 3);
        
        // Linear Original:
        //return oldCameraSize + ((timeSinceSet / transitionTime) * differenceCameraSize);
    }
}
