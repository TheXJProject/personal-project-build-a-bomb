using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMain : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;
    [SerializeField] ButtonManager manager;

    // Runtime Variables:
    [HideInInspector] public bool backPressed = true;

    private void OnMouseDown()
    {
        // If a button is not already pressed
        if (!backPressed)
        {
            // Change the camera position
            cameraObject.NewCameraSizeAndPosition(cameraData.mainMenuCameraSize, cameraData.mainMenuLayer, cameraData.mainMenu);

            // Tell the manager this back to main menu button has been pressed
            manager.OnBackToMainPressed();

            // This object then disappears
            this.gameObject.SetActive(false);
        }
    }
}
