using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerButtonAppearance : MonoBehaviour
{
    // Set up before running
    [SerializeField] GameObject fill; // CONTINUE WORKING ON THIS, MAKE A VARIABLE THAT KNOWS WHETHER THE BUTTON IS BEING FOCUSED OR NOT SO IT KNOWS WHEN TO ENABLE OR DISABLE
                     // THE FILL BAR, THEN HAVE IT PASS A GAMEOBJECT TO THE FILL BAR WHEN IT ACTIVATES IT, THE GAMEOBJECT IS THE TASK THAT IS BEING FOCUSED
                     // FILL BAR THEN IS CODED TO CONSTANTLY DISPLAY FILL AMOUNT WHEN IT IS ACTIVE 

    // Current information
    public GameObject correspondingLayer;
    public bool buttonIsFocused = false;

    private void OnEnable()
    {
        LayerButtonController.onLayerButtonSpawned += DetermineCorrectColour;
        LayerStatus.onLayerSelected += DetermineCorrectColour;
        LayerStatus.onLayerUnSelected += DetermineCorrectColour;
        TaskStatus.onTaskSelected += DetermineCorrectColour;
        TaskStatus.onTaskDeSelected += DetermineCorrectColour;
        TaskStatus.onTaskCompleted += DetermineCorrectColour;
        TaskStatus.onTaskGoneWrong += DetermineCorrectColour;
    }
    private void OnDisable()
    {
        LayerButtonController.onLayerButtonSpawned -= DetermineCorrectColour;
        LayerStatus.onLayerSelected -= DetermineCorrectColour;
        LayerStatus.onLayerUnSelected -= DetermineCorrectColour;
        TaskStatus.onTaskSelected -= DetermineCorrectColour;
        TaskStatus.onTaskDeSelected -= DetermineCorrectColour;
        TaskStatus.onTaskCompleted -= DetermineCorrectColour;
        TaskStatus.onTaskGoneWrong -= DetermineCorrectColour;
    }

    public void DetermineCorrectColour(GameObject trigger)
    {
        if (buttonIsFocused && correspondingLayer.GetComponent<LayerStatus>().ContainsTaskBeingFocused() == false)
        {
            buttonIsFocused = false;
            fill.SetActive(false);
        }

        if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskBeingFocused() && trigger.tag.Equals("Task"))
        {
            if (correspondingLayer.GetComponent<LayerStatus>().layer == trigger.GetComponent<TaskStatus>().taskLayer)
            {
                buttonIsFocused = true;
                fill.SetActive(true);
                fill.GetComponent<CompletionVisualisation>().taskToVisualise = trigger;
                GetComponent<Image>().color = Color.blue;
            }
        }
        else if (correspondingLayer.GetComponent<LayerStatus>().isSelected)
        {
            GetComponent<Image>().color = Color.magenta;
        }
        else if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskBeingSolved())
        {
            GetComponent<Image>().color = Color.yellow;
        }
        else if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskGoneWrong())
        {
            GetComponent<Image>().color = Color.red;
        }
        else if (correspondingLayer.GetComponent<LayerStatus>().IsLayerCompleted())
        {
            GetComponent<Image>().color = Color.green;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
