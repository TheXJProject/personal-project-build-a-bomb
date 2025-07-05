using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMain : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GeneralCameraLogic cameraObject;
    [SerializeField] MainMenuCamera cameraData;

    // Runtime Variables:
    [HideInInspector] public bool backPressed = false;

    private void OnMouseDown()
    {
        // If a button is not already pressed
        if (!backPressed)
        {
            // We've pressed a button
            backPressed = true;

            // Change the camera position
            cameraObject.GetComponent<GeneralCameraLogic>().NewCameraSizeAndPosition(cameraData.mainMenuCameraSize, cameraData.mainMenuLayer, cameraData.mainMenu);

            this.gameObject.SetActive(false);
        }
    }
}
