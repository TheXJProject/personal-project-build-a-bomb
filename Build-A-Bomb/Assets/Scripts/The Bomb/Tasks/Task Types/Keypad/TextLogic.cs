using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = true;

    // Initialise In Inspector:
    [SerializeField] TextMeshProUGUI keypadDisplay;
    public string startingMessage = "Hi :)";

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        keypadDisplay.text = startingMessage;
    }

    /// FUNCTION DESCRIPTION<summary>
    /// Displays given number in the keypad display. <br />
    /// Parameter: The number to display.
    /// </summary>
    public void DisplayText (int number)
    {
        if (Msg) Debug.Log(" Number entered: " + number);

        // Display number entered
        keypadDisplay.text = number.ToString();
    }

    /// FUNCTION DESCRIPTION<summary>
    /// (Overload) Displays given string in the keypad display. <br />
    /// And, optional number. <br />
    /// Parameter 1: The string. <br />
    /// Parameter 2: The number.
    /// </summary>
    public void DisplayText (string text, int number = -1)
    {
        if (Msg) Debug.Log("String entered: " + text + " Number entered: " + number);

        if (number == -1)
        {
            keypadDisplay.text = text;
        }
        else
        {
            keypadDisplay.text = text + " " + number.ToString();
        }
    }

    /// FUNCTION DESCRIPTION<summary>
    /// Displays the default message in the keypad display. <br />
    /// </summary>
    public void DisplayDefault ()
    {
        if (Msg) Debug.Log("Showing starting message.");

        // Showing initial starting message
        keypadDisplay.text = startingMessage;
    }
}
