using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralCameraLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Inspector Adjustable Values:
    [SerializeField] [Range(0f, 1f)] float zoomInIncreaseToTime = 0.1f;
    [SerializeField] [Range(0f, 1f)] float zoomInDecreaseAfterTime = 0.9f;
    [SerializeField] [Range(0f, 1f)] float zoomOutIncreaseToTime = 0.1f;
    [SerializeField] [Range(0f, 1f)] float zoomOutDecreaseAfterTime = 0.9f;
    [SerializeField] [Range(0.000001f, 1000f)] float vFluc = 0;
    public float transitionTime = 0.3f;

    // Runtime Variables:
    [HideInInspector] public float layerAcceleration = 0;
    int currentLayer;
    float oldCameraSize;
    float currentCameraSize;
    float newCameraSize;
    float differenceCameraSize;
    float timeSinceSet;
    bool showingCorrectLayer = true;
    float currentSpeed = 0;
    float averageSpeed = 0;
    float travelledDifference = 0;
    float t = 0;        // Current transition position 0 - 1

    float t0 = 0;       // Time to complete first section
    float t1 = 0;       // Time to complete second section
    float t2 = 0;       // Time to complete third section

    float h0 = 0;       // Camera speed when time is zero
    float h1 = 0;       // Camera speed at first time point
    float h2 = 0;       // Camera speed at second time point

    void Awake()
    {
        // Set values for the different time positions
        t0 = zoomOutIncreaseToTime;
        t1 = zoomOutDecreaseAfterTime - zoomOutIncreaseToTime;
        t2 = 1 - zoomOutDecreaseAfterTime;

        // Show errors if required
        if (zoomOutIncreaseToTime > zoomOutDecreaseAfterTime)
        {
            Debug.LogWarning("Error: zoomOutIncreaseToTime should be less than decreaseToTime!");
        }
        if (zoomInIncreaseToTime > zoomInDecreaseAfterTime)
        {
            Debug.LogWarning("Error: zoomInIncreaseToTime should be less than decreaseToTime!");
        }
    }

    void Update()
    {
        // If we are not showing the correct layer
        if (!showingCorrectLayer)
        {
            // Set the camera to what the current camera size should currently 
            if (timeSinceSet < transitionTime)
            {
                currentCameraSize = DetermineNewCameraSize();
                gameObject.GetComponent<Camera>().orthographicSize = currentCameraSize;
            }
            // Once the transition is finished
            else
            {
                // Set the size of the camera to the exact size it should be, then set it to be showing the correct layer
                currentCameraSize = newCameraSize;
                gameObject.GetComponent<Camera>().orthographicSize = newCameraSize;
                showingCorrectLayer = true;

                // When the camera transition is finished the speed of the camera will be zero
                currentSpeed = 0;
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
        showingCorrectLayer = false;

        // Converts the difference in camersize from a logorithmic system into a linear system.
        differenceCameraSize = ConvertDistanceDiff(newCameraSize) - ConvertDistanceDiff(oldCameraSize);
        
        SetAverageSpeeds();
    }

    /// <summary>
    /// Function which determines what the camera size should be based on the its current, old and new camera properties and <br />
    /// the time since the last NewCameraSize() call
    /// </summary>
    float DetermineNewCameraSize()
    {
        // ============== OLD ==============

        // The camera transition previously determined the current camera size based on a linear transition.
        // Variables:
        //  - oldCameraSize         -> the size of the camera at the start of the transition
        //  - differenceCameraSize  -> the change in size from oldCameraSize to the new size (positive or negative depending on whether the camera is shrinking or growing)
        //  - timeSinceSet          -> the time in seconds since the camera changed size
        //  - transitionTime        -> the time it should take for a transition to be completed

        // Linear Original:
        //return oldCameraSize + ((timeSinceSet / transitionTime) * differenceCameraSize);

        // =========== NEW METHOD ==========
        
        // The camera movement is seperated up into three sections - first - second - third.

        //  - First: Accelerate camera movement to set average speed.
        //  - Second: Hold camera at average speed with potential for fluctuations.
        //  - Third: Decelerate camera to a speed of zero.

        // Get current transition position
        t = timeSinceSet / transitionTime;

        // If we are in the first third
        if (t < t0)
        {
            // Calculate current speed
            currentSpeed = (Mathf.Pow(t, 3) / Mathf.Pow(t0, 3)) * (h1 - h0) + h0;

            // Calculate area (difference/ distance travelled since oldCameraSize)
            travelledDifference = (Mathf.Pow(t, 4) / (4 * Mathf.Pow(t0, 3))) * (h1 - h0) + h0 * t;
        }
        // If we are in the second third
        else if (t < (t0 + t1))
        {
            // Calculate current speed
            currentSpeed = ((h2 - h1) / t1) * (t - t0) + h1;

            // Calculate area (difference/ distance travelled since oldCameraSize)
            travelledDifference = (t0 / 4) * (3 * h0 + h1) + 0.5f * (h1 + currentSpeed) * (t - t0);
        }
        // If we are in the final third
        else
        {
            // Calculate current speed
            currentSpeed = (-h2 / Mathf.Pow(t2, 3)) * Mathf.Pow((t - 1), 3);

            // Calculate area (difference/ distance travelled since oldCameraSize)
            travelledDifference = (t0 / 4) * (3 * h0 + h1) + 0.5f * (h1 + h2) * t1 + (h2 / (4 * Mathf.Pow(t2, 3))) * (Mathf.Pow(t2, 4) - Mathf.Pow((t - 1), 4));
        }

        if (Msg) Debug.Log("Current Speed: " + currentSpeed);
        if (Msg) Debug.Log("Distance travelled: " + travelledDifference);

        // If we have no difference
        if (travelledDifference == 0)
        {
            return oldCameraSize;
        }
        // Check if the travel difference is pos or neg
        else if (travelledDifference > 0)
        {
            // If it is positive we convert it back to a logorithmic form and add to starting camera size
            return oldCameraSize * Mathf.Pow(layerAcceleration, travelledDifference);
        }
        else
        {
            // If it is negitive we make adjustments first, then convert it back to a logorithmic form and add to starting camera size
            return oldCameraSize / Mathf.Pow(layerAcceleration, -travelledDifference);
        }
    }

    /// <summary>
    /// Calculates the average speed of the middle third of camera travel.
    /// </summary>
    void SetAverageSpeeds()
    {
        // Show errors if required
        if (zoomOutIncreaseToTime > zoomOutDecreaseAfterTime)
        {
            Debug.LogWarning("Error: zoomOutIncreaseToTime should be less than decreaseToTime!");
        }
        if (zoomInIncreaseToTime > zoomInDecreaseAfterTime)
        {
            Debug.LogWarning("Error: zoomInIncreaseToTime should be less than decreaseToTime!");
        }

        // If the difference is negitive alter how the times are used in the equations
        if (differenceCameraSize > 0)
        {
            // Set values for the different time positions
            t0 = zoomOutIncreaseToTime;
            t1 = zoomOutDecreaseAfterTime - zoomOutIncreaseToTime;
            t2 = 1 - zoomOutDecreaseAfterTime;
        }
        else
        {
            // Set values for the different time positions
            t0 = zoomInIncreaseToTime;
            t1 = zoomInDecreaseAfterTime - zoomInIncreaseToTime;
            t2 = 1 - zoomInDecreaseAfterTime;
        }

        // Set h0
        h0 = currentSpeed;

        // Set average speed of second third using equation
        averageSpeed = (4 * differenceCameraSize - 3 * h0 * t0) / (vFluc * (t0 + 2 * t1) + (1 / vFluc) * (2 * t1 + t2));

        // Set h1 and h2 from averageSpeed considering fluctuation
        h1 = averageSpeed * vFluc;
        h2 = averageSpeed / vFluc;

        if (Msg) Debug.Log("Average Speed Set: " + averageSpeed);
    }

    /// <summary>
    /// Converts the camersize from a logorithmic system into a linear system.
    /// </summary>
    /// <returns>
    /// Layer difference as linear value.
    /// </returns>
    float ConvertDistanceDiff(float distance)
    {
        // Check that we have a valid base
        if (layerAcceleration <= 0)
        {
            Debug.LogWarning("Error, layerAcceleration is zero or less than zero!");
            return 0;
        }
        else
        {
            return Mathf.Log(distance, layerAcceleration);
        }
    }
}
