using UnityEngine;

public class Hard : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] GameObject backButton;
    [SerializeField] ButtonManager manager;
    [SerializeField] SpriteRenderer spriteRenderer;
    public arrowHover2 arrow;

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
            manager.OnAnyButtonPressed(MENUS.HARD_PLAY);

            // Show the back button
            backButton.SetActive(true);

            // Make sprite disappear when clicked to reveal button underneath
            spriteRenderer.enabled = false;

        }
    }
}
