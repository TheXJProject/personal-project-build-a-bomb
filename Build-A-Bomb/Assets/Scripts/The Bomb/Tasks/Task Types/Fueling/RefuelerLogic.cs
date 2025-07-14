using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RefuelerLogic : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] Image dockImage;
    [SerializeField] FuelingLogic fuelingLogic;
    [SerializeField] RectTransform backGroundTransform;

    // Runtime Variables:
    [HideInInspector] public bool canFollow = false;
    bool following = false;

    public void ResetFuelerAndDock()
    {
        // Set colours for this object


        // Set colours for dock
        dockImage.color = Color.red;
    }

    public void FollowMouse(BaseEventData data)
    {
        // Checks if the function call is allowed
        if (canFollow)
        {
            // Check if the player is using left click
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                // Follow mouse within the boundries of the background
                gameObject.transform.localPosition = Input.mousePosition;


                //backGroundTransform

            }
        }
    }
}
