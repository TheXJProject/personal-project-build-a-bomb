using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] Settings settings;
    [SerializeField] BackToMain settingsBack;
    [SerializeField] Tutorial tutorial;
    [SerializeField] BackToMain tutorialBack;
    [SerializeField] Normal normal;
    [SerializeField] BackToMain normalBack;
    [SerializeField] Hard hard;
    [SerializeField] BackToMain hardBack;
    [SerializeField] GoBack goBack;

    public void OnAnyButtonPressed()
    {
        // If any button is pressed, prevent any other front being pressed
        settings.pressed = true;
        tutorial.pressed = true;
        normal.pressed = true;
        hard.pressed = true;
        goBack.pressed = true;
        
        // Allow back buttons to be pressed
        settingsBack.backPressed = false;
        tutorialBack.backPressed = false;
        normalBack.backPressed = false;
        hardBack.backPressed = false;
    }

    public void OnBackToMainPressed()
    {
        // If any back button is pressed, allow main menu buttons to be pressed
        settings.pressed = false;
        tutorial.pressed = false;
        normal.pressed = false;
        hard.pressed = false;
        goBack.pressed = false;
    }
}
