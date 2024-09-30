using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerButtonPress : MonoBehaviour
{
    // Event actions
    public static event Action<GameObject> onLayerButtonPressed;

    // Current information
    public GameObject correspondingLayer;

    public void PressedButton()
    {
        onLayerButtonPressed?.Invoke(correspondingLayer);
    }
}
