using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour
{
    private void OnDisable()
    {
        // Button starts on red
        gameObject.GetComponent<Image>().color = Color.red;

        StopAllCoroutines();
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Called when button is pressed. <br />
    /// </summary>
    public void CheckIfComplete(BaseEventData data)
    {
        // If left mouse button was pressed
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // Only start coroutine if the script is enabled
            if (this.isActiveAndEnabled)
            {
                StartCoroutine(AnimationHoldTime(0.2f));
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
        gameObject.GetComponent<Image>().color = Color.green;

        float timeElapsed = 0f;

        // Wait for set amount of time
        while (timeElapsed < time)
        {
            // TODO: Replace with call for animation!
            gameObject.GetComponent<Image>().color = Color.green;

            // Increment the time elapsed and continue
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // TODO: Replace with call for animation!
        gameObject.GetComponent<Image>().color = Color.red;
    }
}
