using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTaskGoesWrong : MonoBehaviour
{

    private List<GameObject> tasksToGoWrong = new List<GameObject>();
    private System.Random rnd = new System.Random();

    public void AddNewLayerToChooseFrom(GameObject newLayerToGoWrong)
    {
        LayerStatus layerStats = newLayerToGoWrong.GetComponent<LayerStatus>();

        // Add every task in the layer to the list of tasks that could go wrong and set them to not be going wrong
        for (int i = 0; i < layerStats.tasks.Count; i++)
        {
            tasksToGoWrong.Add(layerStats.tasks[i]);
        }
    }

    public void MakeTaskGoWrong()
    {
        // Just get the first one I don't want to spend more time programming this 
        tasksToGoWrong[0].GetComponent<TaskStatus>().TaskGoneWrong();
    }
}
