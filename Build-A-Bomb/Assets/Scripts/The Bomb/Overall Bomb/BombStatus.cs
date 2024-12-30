using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStatus : MonoBehaviour
{
    // Event actions:
    public static event Action onBombFinished;
    public static event Action<GameObject> onLayerCreated;

    // Inspector Adjustable Values:
    public List<GameObject> layersToBeSpawned = new List<GameObject>();
    public float layerSizeAcceleration = 3f;
    public float taskSizeDeceleration = 0.1f;
    public int layerToStartGoingWrong = 2;

    // Initialise In Inspector:
    [SerializeField] GoneWrongController goneWrongController;

    // Runtime Variables:
    public float layerSizeIncrease = 3f;
    float taskSizeDecrease = 0.85f;
    int sortingLayerDecrease = 1;
    int layersSpawned = 0;
    int currentLayer = 0;
    int finalLayer;
    public List<GameObject> layers = new List<GameObject>();

    private void Awake()
    {
        // Game is won if final layer is reached which is set to be the last layer in layersToBeSpawned
        finalLayer = layersToBeSpawned.Count - 1; 
    }

    private void OnEnable()
    {
        LayerStatus.onLayerCompleted += AttemptNextLayer;

        SpawnCoreLayer();
    }

    private void OnDisable()
    {
        LayerStatus.onLayerCompleted -= AttemptNextLayer;
    }

    /// <summary>
    /// Core layer is already set so not many programatic changes should be made to it
    /// </summary>
    void SpawnCoreLayer()
    {
        // spawn the core layer
        GameObject coreLayer = Instantiate(layersToBeSpawned[layersSpawned], transform);

        // Make the core layer selected
        coreLayer.GetComponent<LayerStatus>().isSelected = true;

        // Set its layer number to the correct layer number (0 for core)
        coreLayer.GetComponent<LayerStatus>().layer = layersSpawned;

        // Add it to the array of layers
        layers.Add(coreLayer);

        // Update variables
        currentLayer = 0;
        layersSpawned++;

        // Signal a layer has been created and its settings set
        onLayerCreated?.Invoke(coreLayer);
    }

    /// <summary>
    /// Whenever a new layer is spawned it is a different size to other layers so it needs to be programmatically adjusted accordingly
    /// </summary>
    void SpawnNextLayer()
    {
        // Add the previous layer to the goneWrongController so that it can use it to go wrong
        goneWrongController.AddNewLayerToChooseFrom(layers[currentLayer]);

        // Spawn the next layer
        GameObject nextLayer = Instantiate(layersToBeSpawned[layersSpawned], transform);

        // Make the layer selected
        nextLayer.GetComponent<LayerStatus>().isSelected = true;

        // Set its layer number to the correct layer number
        nextLayer.GetComponent<LayerStatus>().layer = layersSpawned;

        // Set the the radius of which the tasks cannot spawn within
        nextLayer.GetComponent<LayerStatus>().layerMinRadius *= layerSizeIncrease;

        // Set the radius of the layer itself
        nextLayer.GetComponent<LayerStatus>().layerMaxRadius *= layerSizeIncrease;

        // Set the value which increases the task size appropriately for the layer
        nextLayer.GetComponent<LayerStatus>().taskScaleUp *= layerSizeIncrease;

        // Set the value which makes task appear smaller and smaller as each layer spawns
        nextLayer.GetComponent<LayerStatus>().taskSize *= taskSizeDecrease;

        // Set the actual scale of the layer to the correct size
        nextLayer.transform.localScale *= layerSizeIncrease;

        // Make sure the the layer is infront of the previous layers
        nextLayer.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder -= sortingLayerDecrease;

        // Add it to the array of core layers
        layers.Add(nextLayer);

        // Increases all the variables to increase sizeings by so that it is proportional for the next layer
        layerSizeIncrease *= layerSizeAcceleration;
        taskSizeDecrease -= taskSizeDeceleration;
        taskSizeDeceleration /= 2; // size of tasks decreases by half the amount each time a new layer spawns them
        currentLayer = layersSpawned;
        sortingLayerDecrease++;
        layersSpawned++;

        // Signal a layer has been created and its settings set
        onLayerCreated?.Invoke(nextLayer);
    }

    /// <summary>
    /// Checks that every layer is set equal to isCompleted
    /// </summary>
    bool CheckAllLayersComplete()
    {
        foreach (var layer in layers)
        {
            if (!layer.GetComponent<LayerStatus>().isCompleted)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Decision function which decides whether to create a new level or call victory <br/>
    /// - Can write into this function to use it for other layer dependant triggers such as a an event happening when half the layers have spawned etc..
    /// </summary>
    void AttemptNextLayer(GameObject triggerLayer)
    {
        if (CheckAllLayersComplete())
        {
            // If the layer that was just completed by the player was the final layer, signal the bomb is finished
            if (currentLayer == finalLayer)
            {
                onBombFinished?.Invoke();
            }
            else
            {
                // If the layer about to be spawned is the one where tasks start going wrong, then set it so
                SpawnNextLayer();
                if (currentLayer == layerToStartGoingWrong)
                {
                    goneWrongController.StartGoingWrong();
                }
            }
        }
    }
}
