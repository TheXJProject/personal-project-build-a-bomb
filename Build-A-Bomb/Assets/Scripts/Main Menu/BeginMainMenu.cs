using UnityEngine;

public class BeginMainMenu : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject mainMenuLevel;

    // Runtime Variables:
    bool pressed = false;

    private void OnMouseDown()
    {
        // If we have not pressed the button
        if (!pressed)
        {
            // Prevent it being pressed again until reset elsewhere
            pressed = true;

            MixerFXManager.instance.ForceSetParam(GROUP_OPTIONS.MUSIC_COLLECTION, EX_PARA.VOLUME, 0f);

            AudioManager.instance.PlayMusic("Main", 0.1f);

            MixerFXManager.instance.SetMusicParam("Main", EX_PARA.VOLUME, 3f, 0.5f);

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

        // Activate the next layer
        mainMenuLevel.SetActive(false);
    }
}
