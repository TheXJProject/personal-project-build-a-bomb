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
    // Inspector Adjustable Values:
    [SerializeField] float musicTransitonTime;
    [SerializeField] float fxTransitonTimeIn;

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
        settings.arrow.canShow = false;
        tutorial.arrow.canShow = false;
        normal.arrow.canShow = false;
        hard.arrow.canShow = false;
        goBack.arrow.canShow = false;

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
                MixerFXManager.instance.SetMusicParam("Menu OfficeNoise", EX_PARA.VOLUME, musicTransitonTime);
                MixerFXManager.instance.SetMusicParam("Menu Beeps", EX_PARA.VOLUME, musicTransitonTime);
                MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, musicTransitonTime, 0.7f);
                MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, musicTransitonTime, 0.85f);
                MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, musicTransitonTime, 0.85f);

                // And set each track to off that shouldn't be playing
                MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, musicTransitonTime, 0f);
                MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, musicTransitonTime, 0f);

                // Set high pass filter, goes into it quick
                MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.HIGH_PASS, fxTransitonTimeIn, 0.5f);
                MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.HIGH_PASS, fxTransitonTimeIn, 0.7f);
                MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.HIGH_PASS, fxTransitonTimeIn, 0.7f);
                break;
            case MENUS.TUTORIAL:
                // Set each track to be at the right volume for tutorial
                MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.VOLUME, musicTransitonTime, 0.8f);
                MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, musicTransitonTime, 0.45f);

                // And set each track to off that shouldn't be playing
                MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, musicTransitonTime, 0f);
                MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, musicTransitonTime, 0f);
                MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, musicTransitonTime, 0f);

                // Set high pass filter, goes into it quick
                MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.HIGH_PASS, fxTransitonTimeIn, 0.65f);
                break;
            case MENUS.NORMAL_PLAY:
                // Set each track to be at the right volume for normal play
                MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.VOLUME, musicTransitonTime);
                MixerFXManager.instance.SetMusicParam("Menu FullChoirCrash", EX_PARA.VOLUME, musicTransitonTime);
                break;
            case MENUS.HARD_PLAY:
                // Set each track to be at the right volume for main menu
                MixerFXManager.instance.SetMusicParam("Menu FullChoirCrash", EX_PARA.VOLUME, musicTransitonTime);
                MixerFXManager.instance.SetMusicParam("Menu Alarms", EX_PARA.VOLUME, musicTransitonTime);
                MixerFXManager.instance.SetMusicParam("Menu Beeps", EX_PARA.VOLUME, musicTransitonTime, 0.6f);

                // And set each track to off that shouldn't be playing
                MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, musicTransitonTime, 0f);
                MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, musicTransitonTime, 0f);
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
        settings.arrow.canShow = true;
        tutorial.arrow.canShow = true;
        normal.arrow.canShow = true;
        hard.arrow.canShow = true;
        goBack.arrow.canShow = true;

        // Set each track to be at the right volume for main menu
        MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, musicTransitonTime);
        MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, musicTransitonTime);
        MixerFXManager.instance.SetMusicParam("Menu Hats", EX_PARA.VOLUME, musicTransitonTime);
        MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, musicTransitonTime);
        MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, musicTransitonTime);
        MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, musicTransitonTime);

        // And set each track to off that shouldn't be playing
        MixerFXManager.instance.SetMusicParam("Menu Alarms", EX_PARA.VOLUME, musicTransitonTime, 0f);
        MixerFXManager.instance.SetMusicParam("Menu Beeps", EX_PARA.VOLUME, musicTransitonTime, 0f);
        MixerFXManager.instance.SetMusicParam("Menu OfficeNoise", EX_PARA.VOLUME, musicTransitonTime, 0f);
        MixerFXManager.instance.SetMusicParam("Menu FullChoirCrash", EX_PARA.VOLUME, musicTransitonTime, 0f);
        MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.VOLUME, musicTransitonTime, 0f);

        // Set high pass to normal
        MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.HIGH_PASS, musicTransitonTime);
        MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.HIGH_PASS, musicTransitonTime);
        MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.HIGH_PASS, musicTransitonTime);
    }
}
