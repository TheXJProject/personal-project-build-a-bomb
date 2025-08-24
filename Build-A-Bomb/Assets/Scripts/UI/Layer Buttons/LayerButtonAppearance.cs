using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerButtonAppearance : MonoBehaviour
{
    // Set up before running
    [SerializeField] GameObject fill;
    [SerializeField] GameObject fillBack;
    [SerializeField] Color fillBackCol;

    [SerializeField] GameObject lightsLines;
    [SerializeField] GameObject smallLightInnerGlow;
    [SerializeField] GameObject bigLightInnerGlow;
    [SerializeField] GameObject smallLight;
    [SerializeField] Color smallLightCol;
    [SerializeField] Color smallLightOffCol;
    [SerializeField] GameObject smallLightGlow;
    [SerializeField] Color smallLightGlowCol;
    [SerializeField] Color smallLightOffGlowCol;
    [SerializeField] GameObject smallLightInnerShade;
    [SerializeField] Color smallLightInnerShadeCol;
    [SerializeField] Color smallLightOffInnerShadeCol;
    [SerializeField] GameObject bigLight;
    [SerializeField] Color bigLightColSolve;
    [SerializeField] Color bigLightColWrong;
    [SerializeField] Color bigLightColWork;
    [SerializeField] GameObject bigLightGlow;
    [SerializeField] Color bigLightGlowColSolve;
    [SerializeField] Color bigLightGlowColWrong;
    [SerializeField] Color bigLightGlowColWork;

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
        TaskStatus.onTaskFailed += DetermineCorrectColour;
        TaskStatus.onTaskBegan += DetermineCorrectColour;
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
        TaskStatus.onTaskFailed += DetermineCorrectColour;
        TaskStatus.onTaskBegan += DetermineCorrectColour;
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
                //GetComponent<Image>().color = Color.blue;
            }
        }
        //else if (correspondingLayer.GetComponent<LayerStatus>().isSelected) // CHANGE THIS TO SIGNAL ANIMATION TO BE CORRECT
        //{
        //    GetComponent<Image>().color = Color.magenta;
        //}
        
        
        if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskBeingSolved())
        {
            smallLight.GetComponent<Image>().color = smallLightCol;
            smallLightGlow.GetComponent<Image>().color = smallLightGlowCol;
            smallLightInnerShade.GetComponent<Image>().color = smallLightInnerShadeCol;
            smallLightInnerGlow.GetComponent<Image>().enabled = true;
        }
        else
        {
            smallLight.GetComponent<Image>().color = smallLightOffCol;
            smallLightGlow.GetComponent<Image>().color = smallLightOffGlowCol;
            smallLightInnerShade.GetComponent<Image>().color = smallLightOffInnerShadeCol;
            smallLightInnerGlow.GetComponent<Image>().enabled = false;
        }

        if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskGoneWrong())
        {
            bigLight.GetComponent<Image>().color = bigLightColWrong;
            bigLightGlow.GetComponent<Image>().color = bigLightGlowColWrong;
        }
        else if (correspondingLayer.GetComponent<LayerStatus>().IsLayerCompleted())
        {
            bigLight.GetComponent<Image>().color = bigLightColSolve;
            bigLightGlow.GetComponent<Image>().color = bigLightGlowColSolve;
        }
        else
        {
            bigLight.GetComponent<Image>().color = bigLightColWork;
            bigLightGlow.GetComponent<Image>().color = bigLightGlowColWork;
        }
    }

    void lightLinesAppear()
    {
        lightsLines.SetActive(true);
    }
    void bigLightOn()
    {
        bigLight.SetActive(true);
        bigLightGlow.SetActive(true);
        bigLightInnerGlow.SetActive(true);
    }
    void smallLightOn()
    {
        smallLight.SetActive(true);
        smallLightGlow.SetActive(true);
        smallLightInnerShade.SetActive(true);
        smallLightInnerGlow.SetActive(true);
    }
    void screenOn()
    {
        fillBack.SetActive(true);
        fillBack.GetComponent<Image>().color = fillBackCol;
    }
}
