using UnityEngine;

public class BeginMainMenu : MonoBehaviour
{
    // Inspector Adjustable Values:
    [SerializeField] float startMusicTime;

    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject mainMenuLevel;

    // Runtime Variables:
    bool pressed = false;

    private void Start()
    {
        // Set all music groups to zero volume
        MixerFXManager.instance.ForceSetParam(GROUP_OPTIONS.MUSIC_COLLECTION, EX_PARA.VOLUME, 0f);

        // Play all main menu music tracks at the same time
        AudioManager.instance.PlayMusic("Menu Alarms", startMusicTime);
        AudioManager.instance.PlayMusic("Menu Bass", startMusicTime);
        AudioManager.instance.PlayMusic("Menu Beeps", startMusicTime);
        AudioManager.instance.PlayMusic("Menu Choir", startMusicTime);
        AudioManager.instance.PlayMusic("Menu FullChoirCrash", startMusicTime);
        AudioManager.instance.PlayMusic("Menu Hats", startMusicTime);
        AudioManager.instance.PlayMusic("Menu KickSnare", startMusicTime);
        AudioManager.instance.PlayMusic("Menu OfficeNoise", startMusicTime);
        AudioManager.instance.PlayMusic("Menu Organ", startMusicTime);
        AudioManager.instance.PlayMusic("Menu StartMelody", startMusicTime);
        AudioManager.instance.PlayMusic("Menu StringsXyphone", startMusicTime);

        // Fade in the start menu tracks
        MixerFXManager.instance.SetMusicParam("Menu StartMelody", EX_PARA.VOLUME, 8f);
        MixerFXManager.instance.SetMusicParam("Menu Bass", EX_PARA.VOLUME, 8f);
    }

    private void OnMouseDown()
    {
        // If we have not pressed the button
        if (!pressed)
        {
            // Prevent it being pressed again until reset elsewhere
            pressed = true;

            // Fade in the Main menu tracks
            MixerFXManager.instance.SetMusicParam("Menu Hats", EX_PARA.VOLUME, 2f);
            MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, 2f);
            MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, 2f);
            MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, 2f);

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
        MixerFXManager.instance.SetMusicParam("Menu Hats", EX_PARA.VOLUME, 2f, 0f);
        MixerFXManager.instance.SetMusicParam("Menu KickSnare", EX_PARA.VOLUME, 2f, 0f);
        MixerFXManager.instance.SetMusicParam("Menu Choir", EX_PARA.VOLUME, 2f, 0f);
        MixerFXManager.instance.SetMusicParam("Menu StringsXyphone", EX_PARA.VOLUME, 2f, 0f);

        // Activate the next layer
        mainMenuLevel.SetActive(false);
    }
}
