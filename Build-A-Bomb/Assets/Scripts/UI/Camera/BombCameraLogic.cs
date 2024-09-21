using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCameraLogic : MonoBehaviour
{
    // Initial layer to base scaling size on
    public GameObject bomb;
    public GameObject coreLayer;

    // Information about camera sizes and amount to increase it by give from main bomb status script
    public float initialSize;
    public float layerSizeIncrease;
    public float layerSizeAcceleration;

    private void Awake()
    {
        initialSize = coreLayer.transform.localScale.x;
        layerSizeIncrease = bomb.GetComponent<BombStatus>().layerSizeIncrease;
        layerSizeAcceleration = bomb.GetComponent<BombStatus>().layerSizeAcceleration;
    }

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
