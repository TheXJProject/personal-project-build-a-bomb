using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReactorLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Inspector Adjustable Values:
    [SerializeField] [Range(0.01f, 5f)] float baseFanSpeed;
    [SerializeField] [Range(1f, 100f)] float fanMaxSpeedMultiplier;
    [SerializeField] [Range(0.00001f, 0.05f)] float fanSpeedScaler;
    [SerializeField] [Range(0.01f, 40f)] float timeToChargeMaxLimit;
    [SerializeField] [Range(0.01f, 40f)] float timeToCharge;
    [SerializeField] [Range(0.01f, 20f)] float chargeIncreaseSpeed;

    // Initialise In Inspector:
    [SerializeField] GameObject fan;

    // Runtime Variables:
    [HideInInspector] public bool canSpool;
    [HideInInspector] public bool charged;
    [HideInInspector] public float fanCompletePercentage;
    [HideInInspector] public float timeHeld;
    float currentFanSpeed;
    bool holdingReactor = false;
    bool isMouseOver = false;

    private void Awake()
    {
        if (Msg) Debug.Log("Reactor Script Awake.");

        // The fan start off with no charge and still
        canSpool = false;
        charged = false;
        fanCompletePercentage = 0;
        timeHeld = 0;
        currentFanSpeed = 0;

        // Start fan at random angle
        fan.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    private void FixedUpdate()
    {
        // canSpool will be controlled by isBeingSolved
        // If we canSpool and we have clicked on the reactor
        if (canSpool)
        {
            // (Added so player can hold click)
            holdingReactor = Input.GetMouseButton(0);

            // If the player started holding the reactor, is holding left mouse and the pointer is over the reactor
            if (holdingReactor && Input.GetMouseButton(0) && isMouseOver)
            {
                // Increase charge
                timeHeld = Mathf.Min(timeToChargeMaxLimit, timeHeld + (Time.fixedDeltaTime * chargeIncreaseSpeed));

                // Increase fan speed
                ChangeFanSpeed(baseFanSpeed * fanMaxSpeedMultiplier);
            }
            // Otherwise, if current speed is less that base
            else if (currentFanSpeed < baseFanSpeed)
            {
                // Player is not charging reactor
                holdingReactor = false;

                // Reduce charge
                timeHeld = Mathf.Max(0f, timeHeld - Time.fixedDeltaTime);

                // Increase fan speed to base
                ChangeFanSpeed(baseFanSpeed);
            }
            else
            {
                // Player is not charging reactor
                holdingReactor = false;

                // Reduce charge
                timeHeld = Mathf.Max(0f, timeHeld - Time.fixedDeltaTime);

                // If fan is spooled up without being held
                if (currentFanSpeed > baseFanSpeed)
                {
                    // Slowly reduce fan speed
                    ChangeFanSpeed(baseFanSpeed);
                }
            }
        }
        else
        {
            holdingReactor = false;

            // Reduce charge
            timeHeld = Mathf.Max(0f, timeHeld - Time.fixedDeltaTime);

            // Slowly reduce fan speed to zero
            ChangeFanSpeed(0f);
        }

        // Rotate the fan at the correct speed
        fan.transform.Rotate(0, 0, currentFanSpeed);

        // Completeness check for this reactor
        fanCompletePercentage = Mathf.Min(100f, (timeHeld * 100f / timeToCharge));

        // Is the reactor charged?
        charged = (fanCompletePercentage == 100f);

        // Figure out what colour the fan should be
        FanHueAlteration();

        // TODO: apply animation to fan, motion blur depending on speed

        // For debugging
        if (holdingReactor)
        {
            if (Msg) Debug.Log("Time Held: " + timeHeld);
            if (Msg) Debug.Log("Charged: " + charged + ", Percentage: " + fanCompletePercentage);
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Sets Fan Speed. Calculate what the speed could be <br />
    /// based on how long the player has help the reactor <br />
    /// and the speed the fan is aiming to get to.
    /// Parameter 1: The speed to be reached. <br />
    /// </summary>
    void ChangeFanSpeed(float targetSpeed)
    {
        // If the fan speed is greater than the target
        if (currentFanSpeed > targetSpeed)
        {
            // reduce speed
            currentFanSpeed -= fanSpeedScaler * 0.75f * Mathf.Sqrt(currentFanSpeed - targetSpeed);

            // Make sure we don't reduce past the target
            currentFanSpeed = Mathf.Max(currentFanSpeed, targetSpeed);
        }
        else
        {
            // increase speed
            currentFanSpeed += fanSpeedScaler * Mathf.Sqrt(targetSpeed - currentFanSpeed);

            // Make sure we don't increase past the target
            currentFanSpeed = Mathf.Min(currentFanSpeed, targetSpeed);
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Sets fan inner colour depending on current conditions. <br />
    /// </summary>
    void FanHueAlteration()
    {
        // TODO: Use this function for animations instead of colours?

        // If completely uncharged
        if (fanCompletePercentage == 0f)
        {
            fan.GetComponent<Image>().color = Color.red;
        }
        // If we are at the charge limit
        else if (timeHeld == timeToChargeMaxLimit)
        {
            fan.GetComponent<Image>().color = Color.cyan;
        }
        // If we are at required charge
        else if (fanCompletePercentage == 100f)
        {
            fan.GetComponent<Image>().color = Color.green;
        }
        // Otherwise we are idling
        else
        {
            fan.GetComponent<Image>().color = Color.yellow;
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Sets fan inner colour depending on current conditions. <br />
    /// </summary>
    public void StartChargeReactor(BaseEventData data)
    {
        // If left mouse button was pressed
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // The player has begun charging (holding) the reactor
            holdingReactor = true;
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Triggered when the mouse enteres the object's area. <br />
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Msg) Debug.Log("Mouse is Over.");
        isMouseOver = true;
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Triggered when the mouse exits the object's area. <br />
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Msg) Debug.Log("Mouse is NOT Over.");
        isMouseOver = false;
    }
}
