using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStatus : MonoBehaviour
{
    // Bomb event actions
    public static event Action onBombFinished;
    public static event Action<int> onLayerSettingsSet;

    // To be adjusted as seen fit
    public List<GameObject> layersToBeSpawned = new List<GameObject>();
    public float layerSizeAcceleration = 3f;
    public float taskSizeDeceleration = 0.1f;

    // Current information
    float layerSizeIncrease = 3f;
    float taskSizeDecrease = 0.85f;
    int sortingLayerDecrease = 1;
    int layersSpawned = 0;
    int currentLayer = 0;
    int finalLayer;
    public List<GameObject> layers = new List<GameObject>();

    private void Awake()
    {
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

    void SpawnCoreLayer()
    {
        GameObject coreLayer = Instantiate(layersToBeSpawned[layersSpawned], transform);
        coreLayer.GetComponent<LayerStatus>().layer = layersSpawned;
        layers.Add(coreLayer);

        onLayerSettingsSet?.Invoke(layersSpawned);

        currentLayer = 0;
        layersSpawned++;
    }

    void SpawnNextLayer()
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

        onLayerSettingsSet?.Invoke(layersSpawned);

        layerSizeIncrease *= layerSizeAcceleration;
        taskSizeDecrease -= taskSizeDeceleration;
        taskSizeDeceleration /= 2;
        currentLayer = layersSpawned;
        sortingLayerDecrease++;
        layersSpawned++;
    }

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

    void AttemptNextLayer(GameObject triggerLayer)
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
