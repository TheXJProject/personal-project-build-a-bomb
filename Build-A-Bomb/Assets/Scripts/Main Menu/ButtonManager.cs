using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MENUS
{
    SETTINGS,
    TUTORIAL,
    NORMAL_PLAY,
    HARD_PLAY
}

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

    public void OnAnyButtonPressed(MENUS buttonPressed)
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

        // Set music depending on which button was pressed
        switch (buttonPressed)
        {
            case MENUS.SETTINGS:
                // Set each track to be at the right volume for settings
                MixerFXManager.instance.SetMusicParam("Menu OfficeNoise", EX_PARA.VOLUME, 2f);
                MixerFXManager.instance.SetMusicParam("Menu Beeps", EX_PARA.VOLUME, 2f);
                MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, 2f, 0.7f);

                // And set each track to off that shouldn't be playing
                MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, 2f, 0f);
                MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, 2f, 0f);
                MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, 2f, 0f);
                MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, 2f, 0f);

                // Set high pass filter
                MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.HIGH_PASS, 2f, 0.45f);
                break;
            case MENUS.TUTORIAL:
                // Set each track to be at the right volume for tutorial
                MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.VOLUME, 2f);

                // And set each track to off that shouldn't be playing
                MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, 2f, 0f);
                MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, 2f, 0f);
                MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, 2f, 0f);

                // Set high pass filter
                MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.HIGH_PASS, 2f, 0.45f);
                break;
            case MENUS.NORMAL_PLAY:
                // Set each track to be at the right volume for normal play
                MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.VOLUME, 2f);
                MixerFXManager.instance.SetMusicParam("Menu FullChoirCrash", EX_PARA.VOLUME, 2f);
                break;
            case MENUS.HARD_PLAY:
                // Set each track to be at the right volume for main menu
                MixerFXManager.instance.SetMusicParam("Menu FullChoirCrash", EX_PARA.VOLUME, 2f);
                MixerFXManager.instance.SetMusicParam("Menu Alarms", EX_PARA.VOLUME, 2f);
                MixerFXManager.instance.SetMusicParam("Menu Beeps", EX_PARA.VOLUME, 2f, 0.6f);

                // And set each track to off that shouldn't be playing
                MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, 2f, 0f);
                MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, 2f, 0f);
                break;
            default:
                Debug.LogWarning("Error, somehow got here....");
                break;
        }
    }

    public void OnBackToMainPressed()
    {
        // If any back button is pressed, allow main menu buttons to be pressed
        settings.pressed = false;
        tutorial.pressed = false;
        normal.pressed = false;
        hard.pressed = false;
        goBack.pressed = false;

        // Set each track to be at the right volume for main menu
        MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, 2f);
        MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, 2f);
        MixerFXManager.instance.SetMusicParam("Menu Hats", EX_PARA.VOLUME, 2f);
        MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, 2f);
        MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, 2f);
        MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, 2f);

        // And set each track to off that shouldn't be playing
        MixerFXManager.instance.SetMusicParam("Menu Alarms", EX_PARA.VOLUME, 2f, 0f);
        MixerFXManager.instance.SetMusicParam("Menu Beeps", EX_PARA.VOLUME, 2f, 0f);
        MixerFXManager.instance.SetMusicParam("Menu OfficeNoise", EX_PARA.VOLUME, 2f, 0f);
        MixerFXManager.instance.SetMusicParam("Menu FullChoirCrash", EX_PARA.VOLUME, 2f, 0f);
        MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.VOLUME, 2f, 0f);

        // Set high pass to normal
        MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.HIGH_PASS, 2f);
        MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.HIGH_PASS, 2f);
    }
}
