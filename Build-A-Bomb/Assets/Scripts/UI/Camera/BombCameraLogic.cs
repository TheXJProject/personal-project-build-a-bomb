using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombCameraLogic : MonoBehaviour
{
    // Initial layer to base scaling size on
    public GameObject bomb;
    public GameObject coreLayer;
    public GeneralCameraLogic cameraControl;

    // Current information
    int currentLayer = 0;
    List<float> cameraSizes = new List<float>();

    // Settable information for us to adjust camera settings
    public float sizeIncreaseFromLayer = 2; // The amount that the camera is bigger than the first layer by above and below the task

    // Information about camera sizes and amount to increase it by, taken from main bomb status script and other calculations made in Awake()
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

    // Needs to be called whenever the player changes the layer they are looking at 
    void ChangeFocusedLayer(GameObject triggerLayer)
    {
        int layerToFocus = triggerLayer.GetComponent<LayerStatus>().layer;
        currentLayer = layerToFocus;
        cameraControl.NewCameraSize(cameraSizes[layerToFocus], layerToFocus);
    }
}
