using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    // Initialise In Inspector:
    public FuelingLogic fuelingLogic;
    [SerializeField] Image light_;

    private void Awake()
    {
        // Button and light starts on red
        SetRedButton();
        SetRedLight();
    }

    private void OnDisable()
    {
        // Button and light starts on red
        SetRedButton();
        SetRedLight();

        StopAllCoroutines();
    }

    void SetRedButton()
    {
        // Set the colour of the button to red
        gameObject.GetComponent<Image>().color = Color.red;
    }

    void SetGreenButton()
    {
        // Set the colour of the button to green
        gameObject.GetComponent<Image>().color = Color.green;
    }

    public void SetRedLight()
    {
        // Set the colour of the light to red
        light_.color = Color.red;
    }

    public void SetGreenLight()
    {
        // Set the colour of the light to green
        light_.color = Color.green;
    }

    public void SetYellowLight()
    {
        // Set the colour of the light to yellow
        light_.color = Color.yellow;
    }

    public void PressedButton(BaseEventData data)
    {
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // Show the button has been pressed
            StartCoroutine(ButtonHoldTime(0.2f));

            // If the button has been clicked with the left mouse button
            fuelingLogic.ButtonPressed();
        }
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Shows that the Key has been pressed for a set amount of time. <br />
    /// Parameter 1: Time the key will be shown for
    /// </summary>
    IEnumerator ButtonHoldTime(float time)
    {
        // TODO: Replace with call for animation!
        SetGreenButton();

        float timeElapsed = 0f;

        // Wait for set amount of time
        while (timeElapsed < time)
        {
            // TODO: Replace with call for animation!
            SetGreenButton();

            // Increment the time elapsed and continue
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // TODO: Replace with call for animation!
        SetRedButton();
    }
}
