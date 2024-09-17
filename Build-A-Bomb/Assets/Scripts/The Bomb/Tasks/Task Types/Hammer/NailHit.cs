using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NailHit : MonoBehaviour
{
    public HammerTask hammerTask;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
    }
}
