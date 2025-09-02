using UnityEngine;

public class Normal : MonoBehaviour
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
            // Fade in the normal play tracks
            MixerFXManager.instance.SetMusicParam("Menu FullChoirCrash", EX_PARA.VOLUME, 2f);
            MixerFXManager.instance.SetMusicParam("Menu Organ", EX_PARA.VOLUME, 2f);

            // Change the camera position
            cameraObject.NewCameraSizeAndPosition(cameraData.normalCameraSize, cameraData.normalLayer, cameraData.normal);

            // Tell the manager we've pressed a button in the main menu
            manager.OnAnyButtonPressed(MENUS.NORMAL_PLAY);

            // Show the back button
            backButton.SetActive(true);

            // Make sprite disappear when clicked to reveal button underneath
            spriteRenderer.enabled = false;
        }
    }
}
