using UnityEngine;

public class BeginMainMenu : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GameObject cameraObject;
    [SerializeField] GameObject mainMenuLevel;

    // Runtime Variables:
    bool pressed = false;
    [HideInInspector] MainMenuCamera cameraData;

    void Start()
    {
        // Get the data to use
        cameraData = cameraObject.GetComponent<MainMenuCamera>();
    }

    private void OnMouseDown()
    {
        // If we have not pressed the button
        if (!pressed)
        {
            // Prevent it being pressed again until reset elsewhere
            pressed = true;

            // Change the camera position
            cameraObject.GetComponent<GeneralCameraLogic>().NewCameraSizeAndPosition(cameraData.mainMenuCameraSize, cameraData.mainMenuLayer, cameraData.mainMenu);

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
