using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReactorLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Inspector Adjustable Values:
    [Header("Visual Only")]
    [Range(0.01f, 5f)] public float baseFanSpeed;
    [SerializeField] [Range(1f, 100f)] float fanMaxSpeedMultiplier;
    [SerializeField] [Range(0.01f, 5f)] float fanSpeedScaler;
    [SerializeField] [Range(0.001f, 10f)] float fanSoftener;

    [Header("\nNon-Visual")]
    [SerializeField] [Range(0.01f, 40f)] float chargeLimit;
    [SerializeField] [Range(0.01f, 40f)] float totalChargeNeeded;
    [SerializeField] [Range(0.01f, 20f)] float chargeIncreaseSpeed;
    [SerializeField] [Range(0.001f, 1000f)] float awayChargeDecreaseReduction;

    // Initialise In Inspector:
    [SerializeField] GameObject fan;
    [SerializeField] Image pieChart1;
    [SerializeField] Image pieChart2;

    // Runtime Variables:
    [HideInInspector] public bool canSpool;
    [HideInInspector] public bool charged;
    [HideInInspector] public float fanCompletePercentage;
    [HideInInspector] public float chargeAmount;
    [HideInInspector] public float currentFanSpeed;
    bool holdingReactor = false;
    bool isMouseOver = false;
    float timeStamp = 0;

    private void Awake()
    {
        if (Msg) Debug.Log("Reactor Script Awake.");

        // The fan start off with no charge and still
        canSpool = false;
        charged = false;
        fanCompletePercentage = 0;
        chargeAmount = 0;
        currentFanSpeed = 0;
        timeStamp = Time.time;

        // Start fan at random angle
        fan.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        pieChart1.color = Color.green;
        pieChart2.color = Color.black;
    }

    private void OnEnable()
    {
        // Calculate length of time away
        float awayTime = Time.time - timeStamp;

        // Reduce the charge appropriatly
        chargeAmount = Mathf.Max(0f, chargeAmount - awayTime * awayChargeDecreaseReduction);

        // Set visual fan speed, without softener
        ChangeFanSpeed(baseFanSpeed + chargeAmount, false);
    }

    private void OnDisable()
    {
        // Save time and frames
        timeStamp = Time.time;
        isMouseOver = false;
    }

    private void Update()
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
                chargeAmount = Mathf.Min(chargeLimit, chargeAmount + (Time.deltaTime * chargeIncreaseSpeed));

                // Set visual fan speed, without softener
                ChangeFanSpeed(baseFanSpeed + chargeAmount, false);
            }
            // Otherwise
            else
            {
                // Player is not charging reactor
                holdingReactor = false;

                // Reduce charge
                chargeAmount = Mathf.Max(0f, chargeAmount - Time.deltaTime);

                // Set visual fan speed, with softener
                ChangeFanSpeed(baseFanSpeed + chargeAmount, true);
            }
        }
        else
        {
            holdingReactor = false;

            // Reduce charge
            chargeAmount = Mathf.Max(0f, chargeAmount - Time.deltaTime);

            // Set visual fan speed, and use the softener
            ChangeFanSpeed(baseFanSpeed + chargeAmount, true);
        }

        // Rotate the fan at the correct speed
        fan.transform.Rotate(0, 0, currentFanSpeed * Time.deltaTime * 240f);

        // Show Pie percentage
        ShowPiePerc();

        // Completeness check for this reactor
        fanCompletePercentage = Mathf.Min(100f, (chargeAmount * 100f / totalChargeNeeded));

        // Is the reactor charged?
        charged = (fanCompletePercentage == 100f);

        // Figure out what colour the fan should be
        FanHueAlteration();

        // TODO: apply animation to fan, motion blur depending on speed

        // For debugging
        if (holdingReactor)
        {
            if (Msg) Debug.Log("Time Held: " + chargeAmount);
            if (Msg) Debug.Log("Charged: " + charged + ", Percentage: " + fanCompletePercentage);
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Sets Fan Speed. Calculate what the speed could be <br />
    /// based on how long the player has help the reactor <br />
    /// and the speed the fan is aiming to get to.
    /// Parameter 1: The speed to be reached. <br />
    /// </summary>
    void ChangeFanSpeed(float targetSpeed, bool useSoftener)
    {
        //// If the fan speed is greater than the target
        //if (currentFanSpeed > targetSpeed)
        //{
        //    // reduce speed (partially based on charge speed)
        //    currentFanSpeed -= fanSpeedScaler * (1f / chargeIncreaseSpeed) * Time.deltaTime * Mathf.Sqrt(currentFanSpeed - targetSpeed);

        //    // Make sure we don't reduce past the target
        //    currentFanSpeed = Mathf.Max(currentFanSpeed, targetSpeed);
        //}
        //else
        //{
        //    // increase speed
        //    currentFanSpeed += fanSpeedScaler * Time.deltaTime * Mathf.Sqrt(targetSpeed - currentFanSpeed);

        //    // Make sure we don't increase past the target
        //    currentFanSpeed = Mathf.Min(currentFanSpeed, targetSpeed);
        //}
        // OLD CODE

        // New Methods

        // If we want to use the softener
        if (useSoftener)
        {
            // The new current will use the previous to "soften" the transistion
            currentFanSpeed = Mathf.Lerp(currentFanSpeed, targetSpeed * fanSpeedScaler, (1f - Mathf.Exp(-Time.deltaTime * (1 / fanSoftener))));
        }
        else
        {
            // Calculate the visual speed directly from input
            currentFanSpeed = targetSpeed * fanSpeedScaler;
        }

        // Clamp the speed as precaution
        currentFanSpeed = Mathf.Clamp(currentFanSpeed, 0, fanMaxSpeedMultiplier);
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
        else if (chargeAmount == chargeLimit)
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

    void ShowPiePerc()
    {
        // If we need to show pie 1 charging
        if (fanCompletePercentage < 100f)
        {
            // Charge pie 1
            pieChart1.fillAmount = Mathf.Clamp01(fanCompletePercentage / 100f);
        }
        // Otherwise, if we need to show 
        else if (fanCompletePercentage >= 100f)
        {
            // Set pie 1 to full
            pieChart1.fillAmount = 1f;

            // Set pie 2
            pieChart2.fillAmount = Mathf.Clamp01((chargeAmount - totalChargeNeeded) / (chargeLimit - totalChargeNeeded));

            //// If maxxed out
            //if (chargeAmount == chargeLimit)
            //{
            //    // TODO: Over revving animatioin
            //}
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
