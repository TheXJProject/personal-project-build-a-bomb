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

    [SerializeField] float fadeInc = 0.01f;
    [SerializeField] float fadeTime = 0.3f;

    // Current information
    public GameObject correspondingLayer;
    public bool buttonIsFocused = false;
    GameObject lastSelectedTask = null;

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
        TaskStatus.onTaskFailed -= DetermineCorrectColour;
        TaskStatus.onTaskBegan -= DetermineCorrectColour;
    }

    public void DetermineCorrectColour(GameObject trigger)
    {
        // If the same task triggers the function as the last one that was selected
        if (lastSelectedTask == trigger)
        {
            // If the trigger task is now solved
            if (trigger.GetComponent<TaskStatus>().isSolved)
            {
                StartCoroutine(FadeCurrentSolvedTask());
            }
        }
        else if (trigger.tag.Equals("Task"))
        {
            // If triggering game object is a task which is selected but not solved, then put it up as the last trigger task that was selected
            TaskStatus taskInfo = trigger.GetComponent<TaskStatus>();
            if (taskInfo.isSelected && !taskInfo.isSolved)
            {
                lastSelectedTask = trigger;
            }
        }
        if (buttonIsFocused && correspondingLayer.GetComponent<LayerStatus>().ContainsTaskBeingFocused() == false)
        {
            buttonIsFocused = false;
            //Color fillColour = fill.GetComponent<Image>().color;
            //fillColour.a = 0.0f;
            //fill.GetComponent<Image>().color = fillColour;
            //fill.SetActive(false);
        }

        if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskBeingFocused() && trigger.tag.Equals("Task"))
        {
            if (correspondingLayer.GetComponent<LayerStatus>().layer == trigger.GetComponent<TaskStatus>().taskLayer)
            {
                StopCoroutine(FadeCurrentSolvedTask());
                fill.SetActive(true);
                Color fillColour = fill.GetComponent<Image>().color;
                fillColour.a = 1.0f;
                fill.GetComponent<Image>().color = fillColour;
                buttonIsFocused = true;
                fill.GetComponent<CompletionVisualisation>().taskToVisualise = trigger;
            }
        }
        
        
        if (correspondingLayer.GetComponent<LayerStatus>().ContainsTaskBeingSolved())
        {
            smallLightOut.GetComponent<Image>().color = smallLightCol;
            smallLightGlowOut.GetComponent<Image>().color = smallLightGlowCol;
            smallLightInnerShadeOut.GetComponent<Image>().color = smallLightInnerShadeCol;
            smallLightInnerGlowOut.GetComponent<Image>().enabled = true;

            smallLightIn.GetComponent<Image>().color = smallLightCol;
            smallLightGlowIn.GetComponent<Image>().color = smallLightGlowCol;
            smallLightInnerShadeIn.GetComponent<Image>().color = smallLightInnerShadeCol;
            smallLightInnerGlowIn.GetComponent<Image>().enabled = true;
        }
        else
        {
            smallLightOut.GetComponent<Image>().color = smallLightOffCol;
            smallLightGlowOut.GetComponent<Image>().color = smallLightOffGlowCol;
            smallLightInnerShadeOut.GetComponent<Image>().color = smallLightOffInnerShadeCol;
            smallLightInnerGlowOut.GetComponent<Image>().enabled = false;

            smallLightIn.GetComponent<Image>().color = smallLightOffCol;
            smallLightGlowIn.GetComponent<Image>().color = smallLightOffGlowCol;
            smallLightInnerShadeIn.GetComponent<Image>().color = smallLightOffInnerShadeCol;
            smallLightInnerGlowIn.GetComponent<Image>().enabled = false;
        }

        if (correspondingLayer.GetComponent<LayerStatus>().ContainsGoingWrongAndNotBeingSolved())
        {
            bigLightOut.GetComponent<Image>().color = bigLightColWrong;
            bigLightGlowOut.GetComponent<Image>().color = bigLightGlowColWrong;

            bigLightIn.GetComponent<Image>().color = bigLightColWrong;
            bigLightGlowIn.GetComponent<Image>().color = bigLightGlowColWrong;
        }
        else if (correspondingLayer.GetComponent<LayerStatus>().IsLayerCompleted())
        {
            bigLightOut.GetComponent<Image>().color = bigLightColSolve;
            bigLightGlowOut.GetComponent<Image>().color = bigLightGlowColSolve;

            bigLightIn.GetComponent<Image>().color = bigLightColSolve;
            bigLightGlowIn.GetComponent<Image>().color = bigLightGlowColSolve;
        }
        else
        {
            bigLightOut.GetComponent<Image>().color = bigLightColWork;
            bigLightGlowOut.GetComponent<Image>().color = bigLightGlowColWork;

            bigLightIn.GetComponent<Image>().color = bigLightColWork;
            bigLightGlowIn.GetComponent<Image>().color = bigLightGlowColWork;
        }
    }

    IEnumerator FadeCurrentSolvedTask()
    {
        Color c = fill.GetComponent<Image>().color;
        for (float alpha = 1f; alpha >= 0; alpha -= fadeInc)
        {
            c.a = alpha;
            fill.GetComponent<Image>().color = c;
            // Wait for 0.1 seconds before the next iteration
            yield return new WaitForSeconds(fadeInc * fadeTime);
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

    // Hide the 
    void screenOffOut()
    {
        fillBackOut.SetActive(false);
    }
    void lightLinesAppearIn()
    {
        lightsLinesIn.SetActive(true);
    }
    void lightLinesDisappearIn()
    {
        lightsLinesIn.SetActive(false);
    }
    void bigLightOnIn()
    {
        bigLightIn.SetActive(true);
        bigLightGlowIn.SetActive(true);
        bigLightInnerGlowIn.SetActive(true);
    }
    void bigLightOffIn()
    {
        bigLightIn.SetActive(false);
        bigLightGlowIn.SetActive(false);
        bigLightInnerGlowIn.SetActive(false);
    }

    void smallLightOnIn()
    {
        smallLightIn.SetActive(true);
        smallLightGlowIn.SetActive(true);
        smallLightInnerShadeIn.SetActive(true);
        smallLightInnerGlowIn.SetActive(true);
    }
    void smallLightOffIn()
    {
        smallLightIn.SetActive(false);
        smallLightGlowIn.SetActive(false);
        smallLightInnerShadeIn.SetActive(false);
        smallLightInnerGlowIn.SetActive(false);
    }
    //void screenOnIn()
    //{
    //    fillBackIn.SetActive(true);
    //    fillBackIn.GetComponent<Image>().color = fillBackCol;
    //}
    //void screenOffIn()
    //{
    //    fillBackIn.SetActive(false);
    //}
}
