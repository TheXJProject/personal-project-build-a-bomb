using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReactorLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ==== For Debugging ====
    readonly bool Msg = false;

    // Inspector Adjustable Values:
    [SerializeField] [Range(0.01f, 5f)] float baseFanSpeed;
    [SerializeField] [Range(1f, 100f)] float fanMaxSpeedMultiplier;
    [SerializeField] [Range(0.01f, 40f)] float timeToFullCharged;
    [SerializeField] [Range(0.01f, 40f)] float timeToCharge;

    // Initialise In Inspector:
    [SerializeField] GameObject fan;

    // Runtime Variables:
    [HideInInspector] public bool canSpool;
    [HideInInspector] public bool charged;
    [HideInInspector] public float fanCompletePercentage;
    float timeHeld;
    float currentFanSpeed;
    bool holdingReactor = false;
    bool isMouseOver = false;

    private void Awake()
    {
        if (Msg) Debug.Log("Reactor Script Awake.");

        canSpool = true;
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
            // If the player started holding the reactor, is holding left mouse and the pointer is over the reactor
            if (holdingReactor && Input.GetMouseButton(0) && isMouseOver)
            {
                // The reactor is being charged

            }
            // Otherwise, if current speed is less that base
            else if (currentFanSpeed < baseFanSpeed)
            {
                // Increase fan speed to base

            }
            else
            {
                // Player is not charging reactor
                holdingReactor = false;

                // If fan is spooled up without being held
                if (currentFanSpeed > baseFanSpeed)
                {
                    // Slowly reduce fan speed
                }
                else
                {
                    // Otherwise, hold base fan speed
                    fan.transform.Rotate(0, 0, baseFanSpeed);
                }
            }
        }
        else
        {
            holdingReactor = false;
        }

        // TODO: check charged, complete percnetage etc

        // Figure out what colour the fan should be
        FanHueAlteration();
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Sets fan inner colour depending on current conditions. <br />
    /// </summary>
    void FanHueAlteration()
    {
        fan.GetComponent<Image>().color = Color.cyan;
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
