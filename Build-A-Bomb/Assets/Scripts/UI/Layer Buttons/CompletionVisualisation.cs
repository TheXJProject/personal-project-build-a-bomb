using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionVisualisation : MonoBehaviour
{
    // Set in inspector
    [SerializeField] GameObject layerButton;

    // Set during runtime
    public GameObject taskToVisualise;

    private void OnEnable()
    {
        TaskStatus.onTaskDeSelected += StopShowingTaskCompletion;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskDeSelected -= StopShowingTaskCompletion;
    }

    private void Update()
    {
        if (taskToVisualise != null)
        {
            layerButton.GetComponent<Slider>().value = taskToVisualise.GetComponent<TaskStatus>().taskCompletion;
        }
    }

    private void StopShowingTaskCompletion(GameObject trigger)
    {
        layerButton.GetComponent<Slider>().value = 0;
        taskToVisualise = null;
    }
}
