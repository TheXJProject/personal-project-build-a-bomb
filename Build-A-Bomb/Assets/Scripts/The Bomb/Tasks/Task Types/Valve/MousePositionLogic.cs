using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePositionLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ==== For Debugging ====
    readonly bool Msg = false;

    // Runtime Variables:
    public bool isMouseOver = false;

    /// FUNCTION DESCRIPTION <summary>
    /// Triggered when the mouse enteres the object's area. <br />
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Msg) Debug.Log("Mouse is Over.");
        isMouseOver = true;
    }

    /// FUNCTION DESCRIPTION <summary>
    /// Triggered when the mouse exits the object's area. <br />
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Msg) Debug.Log("Mouse is NOT Over.");
        isMouseOver = false;
    }
}
