using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialTaskGoesWrong : MonoBehaviour
{
    private List<GameObject> tasksToGoWrong = new List<GameObject>();
    private System.Random rnd = new System.Random();

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha9)) MakeTaskGoWrong();
    }

    public void AddNewLayerToChooseFrom(GameObject newLayerToGoWrong)
    {
        LayerStatus layerStats = newLayerToGoWrong.GetComponent<LayerStatus>();

        // Add every task in the layer to the list of tasks that could go wrong and set them to not be going wrong
        for (int i = 0; i < layerStats.tasks.Count; i++)
        {
            tasksToGoWrong.Add(layerStats.tasks[i]);
        }
    }

    void MakeTaskGoWrong()
    {
        int totalTasksToChoosefrom = tasksToGoWrong.Count;
        int taskToGoWrong = rnd.Next() % totalTasksToChoosefrom;

        tasksToGoWrong[taskToGoWrong].GetComponent<TaskStatus>().TaskGoneWrong();
    }
}
