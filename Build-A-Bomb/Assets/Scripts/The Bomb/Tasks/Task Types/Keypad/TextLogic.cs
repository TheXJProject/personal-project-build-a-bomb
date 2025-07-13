using UnityEngine;
using TMPro;

public class TextLogic : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Inspector Adjustable Values:
    public string startingMessage = "Hi :)";

    // Initialise In Inspector:
    [SerializeField] TextMeshProUGUI keypadDisplay;

    private void Awake()
    {
        if (Msg) Debug.Log("Script Awake().");

        // Starts with set starting message
        keypadDisplay.text = startingMessage;
    }

    private void OnEnable()
    {
        // Start with Default text whenever returning to task
        DisplayDefault();
    }

    /// FUNCTION DESCRIPTION<summary>
    /// (Overload) Displays given string in the keypad display. <br />
    /// And, optional number. <br />
    /// Parameter 1: The string. <br />
    /// Parameter 2: The number.
    /// </summary>
    public void DisplayText(string text, int number = -1)
    {
        if (Msg) Debug.Log("String entered: " + text + " Number entered: " + number);

        // If no number was entered
        if (number == -1)
        {
            // Display text entered
            keypadDisplay.text = text;
        }
        else
        {
            // Display text and number entered
            keypadDisplay.text = text + " " + number.ToString();
        }
    }

    /// FUNCTION DESCRIPTION<summary>
    /// Displays the default message in the keypad display. <br />
    /// </summary>
    public void DisplayDefault()
    {
        if (Msg) Debug.Log("Showing starting message.");

        // Showing initial starting message
        keypadDisplay.text = startingMessage;
    }
}
