using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCameraLogic : MonoBehaviour
{
    // Initial layer to base scaling size on
    public GameObject coreLayer;

    // Information about camera sizes and amount to increase it by give from main bomb status script
    public float initialSize;
    private void OnEnable()
    {
        BombStatus.onLayerSettingsSet += ChangeFocusedLayer;
    }

    private void OnDisable()
    {
        BombStatus.onLayerSettingsSet -= ChangeFocusedLayer;
    }

    void ChangeFocusedLayer(int layerToFocus)
    {

    }
}
