using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerButtonAppearance : MonoBehaviour
{
    // Set up before running
    [SerializeField] GameObject fill;
    GameObject fillBack;
    [SerializeField] GameObject fillBackOut;
    [SerializeField] GameObject fillBackIn;
    [SerializeField] Color fillBackCol;

    GameObject lightsLines;
    [SerializeField] GameObject lightsLinesOut;
    [SerializeField] GameObject lightsLinesIn;
    GameObject smallLightInnerGlow;
    [SerializeField] GameObject smallLightInnerGlowOut;
    [SerializeField] GameObject smallLightInnerGlowIn;
    GameObject bigLightInnerGlow;
    [SerializeField] GameObject bigLightInnerGlowOut;
    [SerializeField] GameObject bigLightInnerGlowIn;
    GameObject smallLight;
    [SerializeField] GameObject smallLightOut;
    [SerializeField] GameObject smallLightIn;
    [SerializeField] Color smallLightCol;
    [SerializeField] Color smallLightOffCol;
    GameObject smallLightGlow;
    [SerializeField] GameObject smallLightGlowOut;
    [SerializeField] GameObject smallLightGlowIn;
    [SerializeField] Color smallLightGlowCol;
    [SerializeField] Color smallLightOffGlowCol;
    GameObject smallLightInnerShade;
    [SerializeField] GameObject smallLightInnerShadeOut;
    [SerializeField] GameObject smallLightInnerShadeIn;
    [SerializeField] Color smallLightInnerShadeCol;
    [SerializeField] Color smallLightOffInnerShadeCol;
    GameObject bigLight;
    [SerializeField] GameObject bigLightOut;
    [SerializeField] GameObject bigLightIn;
    [SerializeField] Color bigLightColSolve;
    [SerializeField] Color bigLightColWrong;
    [SerializeField] Color bigLightColWork;
    GameObject bigLightGlow;
    [SerializeField] GameObject bigLightGlowOut;
    [SerializeField] GameObject bigLightGlowIn;
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
            smallLightOut.GetComponent<Image>().color = smallLightCol;
            smallLightGlowOut.GetComponent<Image>().color = smallLightGlowCol;
            smallLightInnerShadeOut.GetComponent<Image>().color = smallLightInnerShadeCol;
            smallLightInnerGlowOut.GetComponent<Image>().enabled = true;
        }
        else
        {
            smallLightOut.GetComponent<Image>().color = smallLightOffCol;
            smallLightGlowOut.GetComponent<Image>().color = smallLightOffGlowCol;
            smallLightInnerShadeOut.GetComponent<Image>().color = smallLightOffInnerShadeCol;
            smallLightInnerGlowOut.GetComponent<Image>().enabled = false;
        }

        if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskGoneWrong())
        {
            bigLightOut.GetComponent<Image>().color = bigLightColWrong;
            bigLightGlowOut.GetComponent<Image>().color = bigLightGlowColWrong;
        }
        else if (correspondingLayer.GetComponent<LayerStatus>().IsLayerCompleted())
        {
            bigLightOut.GetComponent<Image>().color = bigLightColSolve;
            bigLightGlowOut.GetComponent<Image>().color = bigLightGlowColSolve;
        }
        else
        {
            bigLightOut.GetComponent<Image>().color = bigLightColWork;
            bigLightGlowOut.GetComponent<Image>().color = bigLightGlowColWork;
        }
    }

    void lightLinesAppearOut()
    {
        lightsLinesOut.SetActive(true);
    }
    void lightLinesDisappearOut()
    {
        lightsLinesOut.SetActive(false);
    }
    void bigLightOnOut()
    {
        bigLightOut.SetActive(true);
        bigLightGlowOut.SetActive(true);
        bigLightInnerGlowOut.SetActive(true);
    }
    void bigLightOffOut()
    {
        bigLightOut.SetActive(false);
        bigLightGlowOut.SetActive(false);
        bigLightInnerGlowOut.SetActive(false);
    }

    void smallLightOnOut()
    {
        smallLightOut.SetActive(true);
        smallLightGlowOut.SetActive(true);
        smallLightInnerShadeOut.SetActive(true);
        smallLightInnerGlowOut.SetActive(true);
    }
    void smallLightOffOut()
    {
        smallLightOut.SetActive(false);
        smallLightGlowOut.SetActive(false);
        smallLightInnerShadeOut.SetActive(false);
        smallLightInnerGlowOut.SetActive(false);
    }
    void screenOnOut()
    {
        fillBackOut.SetActive(true);
        fillBackOut.GetComponent<Image>().color = fillBackCol;
    }
    void screenOffOut()
    {
        fillBackOut.SetActive(false);
    }
    //void lightLinesAppearIn()
    //{
    //    lightsLines.SetActive(true);
    //}
    //void bigLightOnIn()
    //{
    //    bigLight.SetActive(true);
    //    bigLightGlow.SetActive(true);
    //    bigLightInnerGlow.SetActive(true);
    //}
    //void smallLightOInn()
    //{
    //    smallLight.SetActive(true);
    //    smallLightGlow.SetActive(true);
    //    smallLightInnerShade.SetActive(true);
    //    smallLightInnerGlow.SetActive(true);
    //}
    //void screenOnIn()
    //{
    //    fillBack.SetActive(true);
    //    fillBack.GetComponent<Image>().color = fillBackCol;
    //}
}
