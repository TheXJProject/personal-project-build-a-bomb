using UnityEngine;

public class GoBack : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject beginMenuLevel;
    public arrowHover2 arrow;
    [SerializeField] ButtonManager buttonManager;

    // Runtime Variables:
    [HideInInspector] public bool pressed = false;

    private void OnMouseDown()
    {
        // If a button has not been pressed
        if (!pressed)
        {
            buttonManager.DisableAllOthers();
            // Change the camera position
            cameraObject.NewCameraSizeAndPosition(cameraData.startCameraSize, cameraData.startLayer, cameraData.start);

            // Reset start screen
            beginMenuLevel.GetComponent<BeginMainMenu>().ResetButton();
        }
    }
}
