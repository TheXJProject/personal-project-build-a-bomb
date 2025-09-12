using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RefuelerLogic : MonoBehaviour
{
    // Initialise In Inspector:
    [SerializeField] GameObject refulerVisual;
    [SerializeField] GameObject dock;
    [SerializeField] GameObject dockCover;
    [SerializeField] FuelingLogic fuelingLogic;
    [SerializeField] RectTransform background;

    [SerializeField] Color dockNormalCol;
    [SerializeField] Color dockDockedCol;

    [SerializeField] Vector2 offsetMousePickup;

    // Runtime Variables:
    [HideInInspector] public bool docked = false;
    bool follow = false;

    private void Update()
    {
        docked = IsOverDock();

        // If we are following the mouse and left mouse button is held
        if (follow && Input.GetMouseButton(0))
        {
            // Convert screen position to local position within the background rect
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, Input.mousePosition, null, out Vector2 localPoint))
            {
                // Clamp the position to stay within boundaries
                float clampedX = Mathf.Clamp(localPoint.x, fuelingLogic.topBottomRefulerLimits.x, fuelingLogic.topBottomRefulerLimits.y);
                float clampedY = Mathf.Clamp(localPoint.y, fuelingLogic.leftRightRefulerLimits.x, fuelingLogic.leftRightRefulerLimits.y);
                gameObject.transform.localPosition = new Vector2(clampedX, clampedY) + offsetMousePickup;
            }
        }
        else
        {
            // If the refueler is over the dock
            if (follow && docked)
            {
                // Lock it in place
                gameObject.transform.position = dock.transform.position;
            }

            // Otherwise we are no following the mouse
            follow = false;
        }

        // If the fueler is over the dock
        if (docked && follow)
        {
            // Show it as green
            dockCover.SetActive(false);
            dock.GetComponent<Image>().color = dockDockedCol;
            dockCover.GetComponent<Image>().color = dockDockedCol;
            refulerVisual.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (docked && !follow)
        {
            dockCover.SetActive(true);
            dock.GetComponent<Image>().color = dockDockedCol;
            dockCover.GetComponent<Image>().color = dockDockedCol;
            refulerVisual.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 65.64f);
        }
        else 
        {
            // Otherwise show it as red
            dockCover.SetActive(false);
            dock.GetComponent<Image>().color = dockNormalCol;
            dockCover.GetComponent<Image>().color = dockNormalCol;
            refulerVisual.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    bool IsOverDock()
    {
        // Get the rect transforms and check for overlap
        Rect fuelerRect = GetWorldRect(gameObject.GetComponent<RectTransform>());
        Rect dockRect = GetWorldRect(dock.GetComponent<RectTransform>());

        // Return true if there is overlap
        return dockRect.Overlaps(fuelerRect);
    }

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];

        // Gets bottom-left, top-left, top-right, bottom-right
        rt.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        return new Rect(bottomLeft, topRight - bottomLeft);
    }

    public void ResetFuelerDock()
    {
        // TODO: Replace with animation maybe
        // Set colours for dock
        dock.GetComponent<Image>().color = Color.white;
    }

    public void FollowMouse(BaseEventData data)
    {
        // Check if the player is using left click
        PointerEventData newData = (PointerEventData)data;
        if (newData.button.Equals(PointerEventData.InputButton.Left))
        {
            // Follow the mouse
            follow = true;
        }
    }
}
