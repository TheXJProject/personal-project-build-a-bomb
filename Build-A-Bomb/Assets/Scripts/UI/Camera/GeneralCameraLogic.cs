using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class GeneralCameraLogic : MonoBehaviour
{
    // Inspector Adjustable Values:
    public float transitionTime = 0.3f;
    [Header("All values should be above zero.")]
    [Header("Impact: How sharp the change should be at the start.")]
    [SerializeField] double cImpact = 0;
    [Header("Gradient: How sharp the remaining gradients should be.")]
    [SerializeField] double dGradient = 0;
    [Header("PeakPosition: The position of the peak speed.")]
    [SerializeField] double fPeakPosition = 0;

    // Runtime Variables:
    [HideInInspector] public float layerAcceleration = 0;
    int currentLayer;
    float oldCameraSize;
    float currentCameraSize;
    float newCameraSize;
    float differenceCameraSize;
    float timeSinceSet;
    bool showingCorrectLayer = true;
    double currentSpeed = 0;
    double travelledDifference = 0;
    double time = 0;

    double a = 0; // Used for mainly tracking starting speed
    double b = 0; // Used for mainly tracking required max speed
    double h = 0; // Used for making sure final speed is zero

    double c = 0; // Used to simplify cImpact
    double d = 0; // Used to simplify dGradient
    double f = 0; // Used to simplify fPeakPosition

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

                // When the camera transition is finished the speed of the camera will be zero and time will be 1
                currentSpeed = 0;
                time = 1;
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

        oldCameraSize = currentCameraSize;
        newCameraSize = cameraSize;

        // Converts the difference in camersize from a logorithmic system into a linear system.
        differenceCameraSize = ConvertDistanceDiffToLin(newCameraSize) - ConvertDistanceDiffToLin(oldCameraSize);

        // Set all required dependent variables to new values
        SetAverageSpeeds();

        currentLayer = layer;
        timeSinceSet = 0f;
        showingCorrectLayer = false;
    }

    /// <summary>
    /// Converts the camersize from a logorithmic system into a linear system.
    /// </summary>
    /// <returns>
    /// Layer difference as linear value.
    /// </returns>
    float ConvertDistanceDiffToLin(float distance)
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

    /// <summary>
    /// Converts the camersize from a linear system into a logorithmic system.
    /// </summary>
    /// <returns>
    /// Layer difference as logorithmic value.
    /// </returns>
    float ConvertDistanceDiffsToLog(float distance)
    {
        // Check that we have a valid base
        if (layerAcceleration <= 0)
        {
            Debug.LogWarning("Error, layerAcceleration is zero or less than zero!");
            return 0;
        }
        // If we have zero distance
        else if (distance == 0)
        {
            return 1;
        }
        // Check if the distance is pos or neg
        else if (distance > 0)
        {
            // If it is positive we convert it back to a logorithmic form
            return Mathf.Pow(layerAcceleration, distance);
        }
        else
        {
            // If it is negitive we make adjustments first, then convert it back to a logorithmic form
            return 1 / Mathf.Pow(layerAcceleration, -distance);
        }
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

        // =========== MK2 METHOD ==========

        // The camera movement is seperated up into three sections - first - second - third.

        //  - First: Account for any initial speed, then accelerate camera movement to set average speed.
        //  - Second: Hold camera at average speed with potential for incremental or decremental fluctuations.
        //  - Third: Decelerate camera to a speed of zero.

        // This method used a mixture of linear and non-linear equations to get the speed and distance. 
        // As a result it was less smooth than intended. 

        // === (A reduced commented out version can be found at the bottom of the script) ===

        // =========== MK3 METHOD ==========

        // We use y = a sech(cx) + b sech(dx - f) - h, to calculate speed, for x = t (time) {0 <= t <= 1}
        // c, d, f are adjustable in the inspector. 
        // We use a set of equations to calculate values for a, b, h.

        // Then intergrate y = f(x) between x = 0 and x = t to find the distance covered.

        // After we have all our required values, we start by getting the current t (time).

        // Get current transition position
        time = (double)timeSinceSet / (double)transitionTime;

        // Calculate travelled difference
        // There are three parts to this calculation
        double part1 = (a * Arctan(Sinh(c * time))) / c;
        double part2 = -h * time;
        double part3 = b * (Arctan(Sinh(d * time - f)) - Arctan(Sinh(-f))) / d;
        travelledDifference = part1 + part2 + part3;

        // Return the old camera size with adjustments made, converted back to log form
        return oldCameraSize * ConvertDistanceDiffsToLog((float)travelledDifference);
    }

    /// <summary>
    /// Calculates the average speed of the middle third of camera travel.
    /// </summary>
    void SetAverageSpeeds()
    {
        // Show errors if required
        if (0 >= cImpact)
        {
            Debug.LogWarning("Error: cImpact is less than or equal to zero!");
        }
        if (0 >= dGradient)
        {
            Debug.LogWarning("Error: dGradient is less than or equal to zero!");
        }
        if (0 >= fPeakPosition)
        {
            Debug.LogWarning("Error: fPeakPostion is less than or equal to zero!");
        }

        // Convert names for ease
        c = cImpact;
        d = dGradient;
        f = fPeakPosition;

        // Calculate current speed from the last point t (time)
        // Using the speed equation: y = a sech(cx) + b sech(dx - f) - h
        currentSpeed = a * Sech(c * time) + b * Sech(d * time - f) - h;

        // Calculate new values for a,b,h
        CalculateABH(currentSpeed);
    }

    /// <summary>
    /// Uses set parameters to calculate a,b,h.
    /// </summary>
    void CalculateABH(double h0)
    {
        // First we set up our theta values
        // These use used to prevent overcomplication when coding
        double theta0 = Sech(f);
        double theta1 = Sech(c);
        double theta2 = Sech(d - f);
        double theta3 = Arctan(Sinh(c)) / c;
        double theta4 = (Arctan(Sinh(d - f)) - Arctan(Sinh(-f))) / d;

        // Next we caculate b in two parts
        double numerator = -(double)differenceCameraSize * (1 - theta1) + h0 * (theta3 - theta1);
        double denominator = (theta0 - theta2) * (theta3 - theta1) - (theta4 - theta2) * (1 - theta1);
        b = numerator / denominator;

        // Next calculate a
        a = (h0 - b * (theta0 - theta2)) / (1 - theta1);

        // Finally calculate h
        h = a * theta1 + b * theta2;

        // TODO: remove
        double A1 = a * theta3;
        double A2 = b * theta4;
        double A3 = -h;
        double A = A1 + A2 + A3;
    }

    /// <summary>
    /// Calculates Sech x.
    /// </summary>
    double Sech(double x)
    {
        return 1 / Math.Cosh(x);
    }

    /// <summary>
    /// Gets Sinh x.
    /// </summary>
    double Sinh(double x)
    {
        return Math.Sinh(x);
    }

    /// <summary>
    /// Gets arctan x.
    /// </summary>
    double Arctan(double x)
    {
        return Math.Atan(x);
    }
}

// =================================
// === The remains of MK2 Method ===
// =================================


//// Get current transition position
//t = timeSinceSet / transitionTime;

//// If we are in the first third
//if (t < t0)
//{
//    // Calculate current speed
//    currentSpeed = (Mathf.Pow(t, 3) / Mathf.Pow(t0, 3)) * (h1 - h0) + h0;

//    // Calculate area (difference/ distance travelled since oldCameraSize)
//    travelledDifference = (Mathf.Pow(t, 4) / (4 * Mathf.Pow(t0, 3))) * (h1 - h0) + h0 * t;
//}
//// If we are in the second third
//else if (t < (t0 + t1))
//{
//    // Calculate current speed
//    currentSpeed = ((h2 - h1) / t1) * (t - t0) + h1;

//    // Calculate area (difference/ distance travelled since oldCameraSize)
//    travelledDifference = (t0 / 4) * (3 * h0 + h1) + 0.5f * (h1 + currentSpeed) * (t - t0);
//}
//// If we are in the final third
//else
//{
//    // Calculate current speed
//    currentSpeed = (-h2 / Mathf.Pow(t2, 3)) * Mathf.Pow((t - 1), 3);

//    // Calculate area (difference/ distance travelled since oldCameraSize)
//    travelledDifference = (t0 / 4) * (3 * h0 + h1) + 0.5f * (h1 + h2) * t1 + (h2 / (4 * Mathf.Pow(t2, 3))) * (Mathf.Pow(t2, 4) - Mathf.Pow((t - 1), 4));
//}

//if (Msg) Debug.Log("Current Speed: " + currentSpeed);
//if (Msg) Debug.Log("Distance travelled: " + travelledDifference);

//// If we have no difference
//if (travelledDifference == 0)
//{
//    return oldCameraSize;
//}
//// Check if the travel difference is pos or neg
//else if (travelledDifference > 0)
//{
//    // If it is positive we convert it back to a logorithmic form and add to starting camera size
//    return oldCameraSize * Mathf.Pow(layerAcceleration, travelledDifference);
//}
//else
//{
//    // If it is negitive we make adjustments first, then convert it back to a logorithmic form and add to starting camera size
//    return oldCameraSize / Mathf.Pow(layerAcceleration, -travelledDifference);
//}

//void SetAverageSpeeds()
//{
//    // Show errors if required
//    if (zoomOutIncreaseToTime > zoomOutDecreaseAfterTime)
//    {
//        Debug.LogWarning("Error: zoomOutIncreaseToTime should be less than decreaseToTime!");
//    }
//    if (zoomInIncreaseToTime > zoomInDecreaseAfterTime)
//    {
//        Debug.LogWarning("Error: zoomInIncreaseToTime should be less than decreaseToTime!");
//    }

//    // If the difference is negitive alter how the times are used in the equations
//    if (differenceCameraSize > 0)
//    {
//        // Set values for the different time positions
//        t0 = zoomOutIncreaseToTime;
//        t1 = zoomOutDecreaseAfterTime - zoomOutIncreaseToTime;
//        t2 = 1 - zoomOutDecreaseAfterTime;
//    }
//    else
//    {
//        // Set values for the different time positions
//        t0 = zoomInIncreaseToTime;
//        t1 = zoomInDecreaseAfterTime - zoomInIncreaseToTime;
//        t2 = 1 - zoomInDecreaseAfterTime;
//    }

//    // Set h0
//    h0 = currentSpeed;

//    // Set average speed of second third using equation
//    averageSpeed = (4 * differenceCameraSize - 3 * h0 * t0) / (vFluc * (t0 + 2 * t1) + (1 / vFluc) * (2 * t1 + t2));

//    // Set h1 and h2 from averageSpeed considering fluctuation
//    h1 = averageSpeed * vFluc;
//    h2 = averageSpeed / vFluc;

//    if (Msg) Debug.Log("Average Speed Set: " + averageSpeed);
//}