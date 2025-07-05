using UnityEngine;

public class Settings : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GameObject cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject backButton;

    // Runtime Variables:
    [HideInInspector] public bool pressed = false;

    private void OnMouseDown()
    {
        // If a button is not already pressed
        if (!pressed)
        {
            // We've pressed a button
            pressed = true;

            // Change the camera position
            cameraObject.GetComponent<GeneralCameraLogic>().NewCameraSizeAndPosition(cameraData.SettingsCameraSize, cameraData.settingsLayer, cameraData.settings);

            // Show the back button, it can be pressed
            backButton.SetActive(true);
            backButton.GetComponent<BackToMain>().backPressed = false;
        }
    }
}
