using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerButtonMovement : MonoBehaviour
{
    // Set before running
    [SerializeField] float transitionTime = 0.3f;
    [SerializeField] float percentDoneWhenButtonEnters = 1.0f;

    // current statistics of the movement
    public bool justSpawned = true;
    bool doingTransition = false;
    bool waitingTransition = false;
    float timeSinceMove = 0f;
    [HideInInspector] public float oldYPosition = 0f;
    float newYPosition = 0f;
    float ydifference = 0f;

    private void OnEnable()
    {
        LayerButtonController.onNonInitialLayerButtonSpawned += MoveAddedDistance;
    }

    private void OnDisable()
    {
        LayerButtonController.onNonInitialLayerButtonSpawned -= MoveAddedDistance;
    }

    private void Update()
    {
        // If layer button is doingTransition, then it will move to its target until it is withing a set distance of it and then the transition ends and the button is moved to its destination
        if (doingTransition)
        {
            if (timeSinceMove < transitionTime)
            {
                timeSinceMove += Time.deltaTime;
                transform.localPosition = new Vector3(transform.localPosition.x, oldYPosition - (ydifference * (timeSinceMove / transitionTime)), 0f);
            }
            else
            {
                oldYPosition = oldYPosition - ydifference;
                transform.localPosition = new Vector3(transform.localPosition.x, oldYPosition, 0f);
                doingTransition = false;
            }
        }
        if (waitingTransition)
        {
            if (timeSinceMove < transitionTime * percentDoneWhenButtonEnters)
            {
                timeSinceMove += Time.deltaTime;
            }
            else
            {
                waitingTransition = false;
                GetComponent<LayerButtonInfo>().animator.SetTrigger("enter");
            }

        }
    }

    void MoveAddedDistance(float distanceMoved) // Script is designed to move buttons to correct position even if they are called to move while they are mid transition
    {
        if (justSpawned) // If this button has just spawmed in then it is at the correct position already and doesn't need to be moved
        {
            oldYPosition = transform.localPosition.y;
            justSpawned = false;
            waitingTransition = true;
            return;
        }

        newYPosition = oldYPosition - distanceMoved;
        ydifference = distanceMoved;

        timeSinceMove = 0;
        doingTransition = true;
    }
}
