using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerButtonPress : MonoBehaviour
{
    // Event actions:
    public static event Action<GameObject> onLayerButtonPressed;

    // Runtime Variables:
    public GameObject correspondingLayer;

    /// <summary>
    /// Function to be set in the inspector, when the layer button is pressed, it signals that it has been pressed, referencing itself
    /// </summary>
    public void PressedButton()
    {
        onLayerButtonPressed?.Invoke(correspondingLayer);
    }
}
