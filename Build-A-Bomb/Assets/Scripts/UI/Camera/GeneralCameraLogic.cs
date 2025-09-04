using UnityEngine;
using System;

public class GeneralCameraLogic : MonoBehaviour
{
    [Header("IMPORTANT NOTE!")]
    [Header("This script has the ability to move the camera around the scene\n" +
        "if 'Adjust Position' is checked on. However, it will only move in\n" +
        "this way if it is also travelling to a different layer.")]
    [Header("Make sure that the player cannot click on a section where the\n" +
        "camera would move around the scene without changing layers!\n")]

    // Inspector Adjustable Values:
    [Header("(Turn on 'Adjust Position' only when used for the main menu)")]
    public bool adjustPosition = false;

    [Header("(When, and only when, 'Adjust Position' is true, 'Size Increase \n" +
        "From Layer', 'Starting Camera Layer' and 'Starting Layer\n" +
        "Acceleration'are used)")]
    public float sizeIncreaseFromLayer = 2; // The amount of space between edge of the first bomb layer and the top/bottom of the screen
    public int startingCameraLayer = 1;
    public float startingLayerAcceleration = 3;

    [Header("")]
    [Header("(All values below should be above zero)")]
    [Header("")]
    public float transitionTime = 0.3f;
    
    [Header("Impact: How sharp the change should be at the start.")]
    [SerializeField] double cImpact = 0;
    
    [Header("Gradient: How sharp the remaining gradients should be.")]
    [SerializeField] double dGradient = 0;
    
    [Header("PeakPosition: The position of the peak speed.")]
    [SerializeField] double fPeakPosition = 0;

    // Initialise In Inspector:
    public GameObject coreLayer;

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

    Vector3 startCameraPosition;
    Vector3 currentCameraPosition;
    [HideInInspector] public Vector3 finalCameraPosition;

    private void Start()
    {
        // If in menu, setup initialise variables manually
        if (adjustPosition)
        {
            // Get information about the initial sizings
            float initialSize = coreLayer.transform.GetChild(0).localScale.x;

            // Initial size of the camera is based on the initial layer size
            initialSize = sizeIncreaseFromLayer + (initialSize / 2);

            layerAcceleration = startingLayerAcceleration;
            InitialiseCameraSize(initialSize, startingCameraLayer);
        }

        // Get where the camera is starting
        startCameraPosition = gameObject.GetComponent<Camera>().transform.position;
        currentCameraPosition = gameObject.GetComponent<Camera>().transform.position;
        finalCameraPosition = gameObject.GetComponent<Camera>().transform.position;
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

                // If we are moving the camera
                if (adjustPosition)
                {
                    // Get the required camera position and set the camera
                    currentCameraPosition = DetermineNewCameraPosition();
                    gameObject.GetComponent<Camera>().transform.position = currentCameraPosition;
                }
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

                // If we are moving the camera
                if (adjustPosition)
                {
                    // Set the camera to the final camera position
                    gameObject.GetComponent<Camera>().transform.position = finalCameraPosition;

                    // The set the new starting position and current position for the camera to the final position
                    startCameraPosition = finalCameraPosition;
                    currentCameraPosition = finalCameraPosition;
                }
            }

            // Increase the timer
            timeSinceSet += Time.deltaTime;
        }
    }

    /// <summary>
    /// Used to calculate the current camera position, scaling using the currentCameraSize.
    /// </summary>
    Vector3 DetermineNewCameraPosition()
    {
        // If we are not moving the camera, throw error
        if (!adjustPosition)
        {
            Debug.LogWarning("Error, DetermineNewCameraPosition() shouldn't be called when not adjusting camera x,y position!");
            return Vector3.zero;
        }

        if (Mathf.Abs(differenceCameraSize) < 0.05)
        {
            Debug.Log(differenceCameraSize);
        }

        // Get the current distance travelled in x and y and add to starting position
        float x = startCameraPosition.x + (finalCameraPosition.x - startCameraPosition.x) * (float)travelledDifference / differenceCameraSize;
        float y = startCameraPosition.y + (finalCameraPosition.y - startCameraPosition.y) * (float)travelledDifference / differenceCameraSize;
        float z = finalCameraPosition.z;

        // Return the new position
        return new Vector3(x, y, z);
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
    /// (Additionally, get the position the camera needs to travel to and sets the start position to current)
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

    public void NewCameraSizeAndPosition(float cameraSize, int layer, Vector3 newPosition)
    {
        // Show error if attempting to move camera position while layer is unchanged or when not adjusting position
        if (!adjustPosition)
        {
            Debug.LogWarning("Error, camera cannot move position if 'Adjust Position' is not checked on!");
            return;
        }
        else if (currentLayer == layer)
        {
            Debug.LogWarning("Error, camera cannot move position if layer is unchanged!");
            return;
        }

        // Set start position to current, new camera final position and new camera size
        startCameraPosition = currentCameraPosition;
        finalCameraPosition = newPosition;
        NewCameraSize(cameraSize, layer);
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
        if (layerAcceleration <= 1)
        {
            Debug.LogWarning("Error, layerAcceleration is one or less than one!");
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
        if (layerAcceleration <= 1)
        {
            Debug.LogWarning("Error, layerAcceleration is is one or less than one!");
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

        // Desmos model:
        // https://www.desmos.com/calculator/ask6phhuyx

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