using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Inspector Adjustable Values:
    public float onPressedTime = 1;
    public int keynumber;

    [SerializeField] float altSizePercent;
    [SerializeField] Color altColor;

    Vector2 altSize;
    Vector2 baseSize;
    Color baseColor;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // TODO: Replace with call for animation!
        baseColor = gameObject.GetComponent<Image>().color;
        baseSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        altSize = baseSize * altSizePercent;
    }

    private void OnEnable()
    {
        // Key is red on start
        gameObject.GetComponent<Image>().color = baseColor;
        gameObject.GetComponent<RectTransform>().sizeDelta = baseSize;
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by the Key gameobject. Will show a keypad press. <br />
    /// </summary>
    public void PressKey(BaseEventData data)
    {
        // Check if the task can be solved
        if (gameObject.transform.parent.parent.parent.GetComponent<KeypadLogic>().statInteract.isBeingSolvedAndSelected &&
            gameObject.transform.parent.parent.parent.GetComponent<KeypadLogic>().canClickKeys)
        {
            PointerEventData newData = (PointerEventData)data;
            // Check left click is pressed
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                if (Msg) Debug.Log("Key pressed.");

                // If the object can be seen
                if (gameObject.activeInHierarchy)
                {
                    // Hold the colour for a set time
                    StartCoroutine(AnimationHoldTime(onPressedTime));
                }

                // Signal which key has been pressed
                KeyPressed();
            }
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Shows that the Key has been pressed for a set amount of time. <br />
    /// Parameter 1: Time the key will be shown for
    /// </summary>
    IEnumerator AnimationHoldTime(float time)
    {
        // TODO: Replace with call for animation!
        gameObject.GetComponent<Image>().color = altColor;
        gameObject.GetComponent<RectTransform>().sizeDelta = altSize;

        float timeElapsed = 0f;

        // Wait for set amount of time
        while (timeElapsed < time)
        {
            // TODO: Replace with call for animation!
            gameObject.GetComponent<Image>().color = altColor;
            gameObject.GetComponent<RectTransform>().sizeDelta = altSize;

            // Increment the time elapsed and continue
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // TODO: Replace with call for animation!
        gameObject.GetComponent<Image>().color = baseColor;
        gameObject.GetComponent<RectTransform>().sizeDelta = baseSize;
    }


    /// FUNCTION DESCRIPTION <summary>
    /// Show this key without it being pressed. This is used when <br />
    /// the player wants to see the code sequence order. <br />
    /// Parameter 1: Time the key will be shown for
    /// </summary>
    public void ShowKey(float showTime)
    {
        if (Msg) Debug.Log("Showing Key: " + keynumber);

        // If the object can be seen
        if (gameObject.activeInHierarchy)
        {
            // Show this key
            StartCoroutine(AnimationHoldTime(showTime));
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called by the Key gameobject. Tells keypad logic what has been pressed. <br />
    /// </summary>
    void KeyPressed()
    {
        // Find the keypad logic and call what key to process
        gameObject.transform.parent.parent.parent.GetComponent<KeypadLogic>().KeyToProcess(keynumber);

        if (Msg) Debug.Log("Pressed Key: " + keynumber);
    }
}
