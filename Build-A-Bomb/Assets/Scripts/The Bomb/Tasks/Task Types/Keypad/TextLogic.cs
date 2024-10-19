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

    /// FUNCTION DESCRIPTION<summary>
    /// Displays given number in the keypad display. <br />
    /// Parameter: The number to display.
    /// </summary>
    public void DisplayText (int number)
    {
        // TODO: display number
    }

    /// FUNCTION DESCRIPTION<summary>
    /// (Overload) Displays given string in the keypad display. <br />
    /// And, optional number. <br />
    /// Parameter 1: The string. <br />
    /// Parameter 2: The number.
    /// </summary>
    public void DisplayText (string text, int number = -1)
    {
        // TODO: display number
    }
}
