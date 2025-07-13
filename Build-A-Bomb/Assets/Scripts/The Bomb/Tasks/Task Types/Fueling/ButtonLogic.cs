using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    // Initialise In Inspector:
    public FuelingLogic fuelingLogic;

    public void SetRed()
    {
        // Set the colour of the button to red
        gameObject.GetComponent<Image>().color = Color.red;
    }

    public void SetGreen()
    {
        // Set the colour of the button to green
        gameObject.GetComponent<Image>().color = Color.green;
    }

    public void PressedButton(BaseEventData data)
    {
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // If the button has been clicked with the left mouse button
            // TODO: call logic tell the button has been pressed
            //fuelingLogic.ButtonPressed();
        }
    }
}
