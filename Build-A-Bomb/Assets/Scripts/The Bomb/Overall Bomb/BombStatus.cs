using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStatus : MonoBehaviour
{
    // Bomb event actions
    public static event Action onBombFinished;
    public static event Action<int> onLayerSettingsSet; //So layers know to spawn tasks after they have been correctly set

    // To be adjusted as seen fit
    public List<GameObject> layersToBeSpawned = new List<GameObject>();
    public float layerSizeAcceleration = 3f;
    public float taskSizeDeceleration = 0.1f;

    // Current information
    public float layerSizeIncrease = 3f;
    float taskSizeDecrease = 0.85f;
    int sortingLayerDecrease = 1;
    int layersSpawned = 0;
    int currentLayer = 0;
    int finalLayer;
    public List<GameObject> layers = new List<GameObject>();

    private void Awake()
    {
        finalLayer = layersToBeSpawned.Count - 1; // Game is won if final layer is reached which is set to be the last layer in layersToBeSpawned
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

    void SpawnCoreLayer() // Core layer is already set so not many programatic changes should be made to it
    {
        GameObject coreLayer = Instantiate(layersToBeSpawned[layersSpawned], transform);
        coreLayer.GetComponent<LayerStatus>().layer = layersSpawned;
        layers.Add(coreLayer);

        onLayerSettingsSet?.Invoke(layersSpawned);

        currentLayer = 0;
        layersSpawned++;
    }

    void SpawnNextLayer() // Whenever a new layer is spawned it is a different size to other layers so it needs to be programmatically adjusted accordingly
    {
        GameObject nextLayer = Instantiate(layersToBeSpawned[layersSpawned], transform);
        nextLayer.GetComponent<LayerStatus>().layer = layersSpawned;
        nextLayer.GetComponent<LayerStatus>().layerMinRadius *= layerSizeIncrease;
        nextLayer.GetComponent<LayerStatus>().layerMaxRadius *= layerSizeIncrease;
        nextLayer.GetComponent<LayerStatus>().taskScaleUp *= layerSizeIncrease;
        nextLayer.GetComponent<LayerStatus>().taskSize *= taskSizeDecrease;
        nextLayer.transform.localScale *= layerSizeIncrease;
        nextLayer.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder -= sortingLayerDecrease;
        layers.Add(nextLayer);

        // Event action which is mainly used to indicate when all the pre-task setup is complete so that tasks can spawn with the required settings
        onLayerSettingsSet?.Invoke(layersSpawned);

        // Increases all the variables to increase sizeings by so that it is proportional for the next layer
        layerSizeIncrease *= layerSizeAcceleration;
        taskSizeDecrease -= taskSizeDeceleration;
        taskSizeDeceleration /= 2; // size of tasks decreases by half the amount each time a new layer spawns them
        currentLayer = layersSpawned;
        sortingLayerDecrease++;
        layersSpawned++;
    }

    bool CheckAllLayersComplete() // Checks that every layer is set equal to isCompleted
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

    void AttemptNextLayer(GameObject triggerLayer) // Decision function which decides whether to create a new level or call victory - can be used for other layer dependant triggers
    {
        if (CheckAllLayersComplete())
        {
            if (currentLayer == finalLayer)
            {
                onBombFinished?.Invoke();
            }
            else
            {
                SpawnNextLayer();
            }
        }
    }
}
