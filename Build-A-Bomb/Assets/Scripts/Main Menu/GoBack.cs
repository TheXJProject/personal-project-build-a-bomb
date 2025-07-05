using UnityEngine;

public class GoBack : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GameObject cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject beginMenuLevel;

    // Runtime Variables:
    [HideInInspector] public bool pressed = false;

    private void OnMouseDown()
    {
        // If a button has not been pressed
        if (!pressed)
        {
            // We've pressed a button
            pressed = true;

            // Change the camera position
            cameraObject.GetComponent<GeneralCameraLogic>().NewCameraSizeAndPosition(cameraData.startCameraSize, cameraData.startLayer, cameraData.start);

            // Reset start screen
            beginMenuLevel.GetComponent<BeginMainMenu>().ResetButton();
        }
    }
}
