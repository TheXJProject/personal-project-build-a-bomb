using UnityEngine;

public class Tutorial : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject backButton;
    [SerializeField] ButtonManager manager;

    // Runtime Variables:
    [HideInInspector] public bool pressed = false;

    private void OnMouseDown()
    {
        // If a button is not already pressed
        if (!pressed)
        {
            // Change the camera position
            cameraObject.NewCameraSizeAndPosition(cameraData.tutorialCameraSize, cameraData.tutorialLayer, cameraData.tutorial);

            // Tell the manager we've pressed a button in the main menu
            manager.OnAnyButtonPressed(MENUS.TUTORIAL);

            // Show the back button
            backButton.SetActive(true);
        }
    }
}
