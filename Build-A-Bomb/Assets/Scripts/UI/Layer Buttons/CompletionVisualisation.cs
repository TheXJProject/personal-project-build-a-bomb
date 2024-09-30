using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionVisualisation : MonoBehaviour
{
    // Set in inspector
    [SerializeField] GameObject layerButton;

    // Set during runtime
    [HideInInspector] public GameObject taskToVisualise;

    private void Update()
    {
        if (taskToVisualise != null)
        {
            layerButton.GetComponent<Slider>().value = taskToVisualise.GetComponent<TaskStatus>().taskCompletion;
        }
    }
}
