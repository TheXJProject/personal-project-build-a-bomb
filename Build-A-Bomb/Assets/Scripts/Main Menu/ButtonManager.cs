using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] Settings settings;
    [SerializeField] BackToMain settingsBack;
    [SerializeField] GoBack goBack;

    public void OnAnyButtonPressed()
    {
        // If any button is pressed 
        settings.pressed = true;
        goBack.pressed = true;
    }

    public void OnBackToMainPressed()
    {
        // If any back button is pressed
        settings.pressed = false;
        goBack.pressed = false;
    }
}
