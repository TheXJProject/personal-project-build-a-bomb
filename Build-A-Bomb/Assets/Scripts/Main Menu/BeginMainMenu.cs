using UnityEngine;

public class BeginMainMenu : MonoBehaviour
{
    // Inspector Adjustable Values:
    [SerializeField] float startMusicTime;
    [SerializeField] float musicTransitonTime;

    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject mainMenuLevel;

    // Runtime Variables:
    bool pressed = false;

    private void Start()
    {
        // TODO:: readd music
        //double startTime = AudioSettings.dspTime + startMusicTime;

        //// Set all music groups to zero volume
        //MixerFXManager.instance.ForceSetParam(GROUP_OPTIONS.MUSIC_COLLECTION, EX_PARA.VOLUME, 0f);

        //// Play all main menu music tracks at the same time
        //AudioManager.instance.PlayMusic("Menu Alarms", startTime);
        //AudioManager.instance.PlayMusic("Menu Bass", startTime);
        //AudioManager.instance.PlayMusic("Menu Beeps", startTime);
        //AudioManager.instance.PlayMusic("Menu Choir", startTime);
        //AudioManager.instance.PlayMusic("Menu FullChoirCrash", startTime);
        //AudioManager.instance.PlayMusic("Menu Hats", startTime);
        //AudioManager.instance.PlayMusic("Menu KickSnare", startTime);
        //AudioManager.instance.PlayMusic("Menu OfficeNoise", startTime);
        //AudioManager.instance.PlayMusic("Menu Organ", startTime);
        //AudioManager.instance.PlayMusic("Menu StartMelody", startTime);
        //AudioManager.instance.PlayMusic("Menu StringsXyphone", startTime);

        //// Fade in the start menu tracks
        //MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, 0.1f);
        //MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, 0.1f);
    }

    private void OnMouseDown()
    {
        // If we have not pressed the button
        if (!pressed)
        {
            // Prevent it being pressed again until reset elsewhere
            pressed = true;

            // Fade in the Main menu tracks
            MixerFXManager.instance.SetMusicParam("Menu Hats", EX_PARA.VOLUME, musicTransitonTime);
            MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, musicTransitonTime);
            MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, musicTransitonTime);
            MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, musicTransitonTime);

            // Change the camera position
            cameraObject.NewCameraSizeAndPosition(cameraData.mainMenuCameraSize, cameraData.mainMenuLayer, cameraData.mainMenu);

            // Activate the next layer
            mainMenuLevel.SetActive(true);
        }
    }

    public void ResetButton()
    {
        // We can now press the start button again
        pressed = false;

        // Fade out the Main menu tracks
        MixerFXManager.instance.SetMusicParam("Menu Hats", EX_PARA.VOLUME, musicTransitonTime, 0f);
        MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, musicTransitonTime, 0f);
        MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, musicTransitonTime, 0f);
        MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, musicTransitonTime, 0f);

        // Activate the next layer
        mainMenuLevel.SetActive(false);
    }
}
