using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerButtonPress : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Event actions:
    public static event Action<GameObject> onLayerButtonPressed;

    // Runtime Variables:
    public GameObject correspondingLayer;

    private void OnEnable()
    {
        LayerButtonPress.onLayerButtonPressed += layerSwitch;
        BombStatus.onEachGoingWrongTasksSolved += layerSwitch;
        LayerButtonController.onLayerButtonSpawned += layerSwitch;
    }

    private void OnDisable()
    {
        LayerButtonPress.onLayerButtonPressed -= layerSwitch;
        BombStatus.onEachGoingWrongTasksSolved -= layerSwitch;
        LayerButtonController.onLayerButtonSpawned -= layerSwitch;
    }

    /// <summary>
    /// Function to be set in the inspector, when the layer button is pressed, it signals that it has been pressed, referencing itself
    /// </summary>
    public void PressedButton()
    {
        onLayerButtonPressed?.Invoke(correspondingLayer);
    }

    public void layerSwitch(GameObject inCorrespondingLayer)
    {
        if (inCorrespondingLayer == correspondingLayer)
        {
            animator.SetBool("showing", true);
        }
        else
        {
            animator.SetBool("showing", false);
        }
    }
}
