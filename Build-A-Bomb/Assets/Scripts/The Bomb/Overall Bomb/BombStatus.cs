using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStatus : MonoBehaviour
{
    // To be adjusted as seen fit
    public List<GameObject> typesofLayers = new List<GameObject>();

    // Current information
    int layersSpawned = 0;
    int currentLayer = 0;
    public List<GameObject> layers = new List<GameObject>();



    private void OnEnable()
    {
        TaskStatus.onTaskCompleted += TryLayerFinished;

        //SpawnCoreLayer();
    }

    private void OnDisable()
    {
        TaskStatus.onTaskCompleted -= TryLayerFinished;
    }

    void SpawnCoreLayer()
    {
        GameObject coreLayer = Instantiate(typesofLayers[currentLayer]);
        coreLayer.GetComponent<LayerStatus>().layer = currentLayer;
        layers.Add(coreLayer);
        currentLayer = 0;
        layersSpawned++;
    }

    void TryLayerFinished(GameObject triggerTask)
    {
        
    }
}
