using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombCameraLogic : MonoBehaviour
{
    // Inspector Adjustable Values:
    public float sizeIncreaseFromLayer = 2; // The amount of space between edge of the first bomb layer and the top/bottom of the screen

    // Initialise In Inspector:
    public GameObject bomb;
    public GameObject coreLayer;
    public GeneralCameraLogic cameraControl;

    // Runtime Variables:
    int currentLayer = 0;
    List<float> cameraSizes = new List<float>();
    float initialSize;
    float layerSizeIncrease;
    float layerSizeAcceleration;
    int totalNumOfLayers;

    private void Awake()
    {
        // Get all the information about the bomb sizings
        initialSize = coreLayer.transform.GetChild(0).localScale.x;
        layerSizeIncrease = bomb.GetComponent<BombStatus>().layerSizeIncrease;
        layerSizeAcceleration = bomb.GetComponent<BombStatus>().layerSizeAcceleration;
        totalNumOfLayers = bomb.GetComponent<BombStatus>().layersToBeSpawned.Count;

        // Initial size of the camera is based on the initial layer size
        initialSize = sizeIncreaseFromLayer + (initialSize / 2);
        cameraSizes.Add(initialSize);

        // Initialise the camera 
        cameraControl.InitialiseCameraSize(initialSize, currentLayer);

        // Set the camera size for each layer
        for (int i = 1; i < totalNumOfLayers; i++)
        {
            cameraSizes.Add(cameraSizes[i-1] * layerSizeAcceleration);
            layerSizeIncrease *= layerSizeAcceleration;
        }
    }

    private void OnEnable()
    {
        BombStatus.onLayerCreated += ChangeFocusedLayer;
        LayerButtonPress.onLayerButtonPressed += ChangeFocusedLayer;
    }

    private void OnDisable()
    {
        BombStatus.onLayerCreated -= ChangeFocusedLayer;
        LayerButtonPress.onLayerButtonPressed -= ChangeFocusedLayer;
    }

    /// <summary>
    /// Should be called everytime the player changes the layer the camera is focusing on, it
    /// </summary>
    void ChangeFocusedLayer(GameObject triggerLayer)
    {
        int layerToFocus = triggerLayer.GetComponent<LayerStatus>().layer;
        currentLayer = layerToFocus;
        cameraControl.NewCameraSize(cameraSizes[layerToFocus], layerToFocus);
    }
}
