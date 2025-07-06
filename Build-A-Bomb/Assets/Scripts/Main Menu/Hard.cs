using UnityEngine;

public class Hard : MonoBehaviour
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
            cameraObject.NewCameraSizeAndPosition(cameraData.hardCameraSize, cameraData.hardLayer, cameraData.hard);

            // Tell the manager we've pressed a button in the main menu
            manager.OnAnyButtonPressed();

            // Show the back button
            backButton.SetActive(true);
        }
    }
}
