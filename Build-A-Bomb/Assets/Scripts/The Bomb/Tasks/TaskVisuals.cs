using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskVisuals : MonoBehaviour
{
    [SerializeField] Material xRayFinished;
    [SerializeField] Material xRayUnfinished;
    [SerializeField] Material xRayWorking;
    [SerializeField] SpriteRenderer OuterRing;
    [SerializeField] SpriteRenderer InnerRing;

    Color normalColourInner;
    Color normalColourOuter;
    [SerializeField] Color beingSolvedColour = Color.yellow;
    [SerializeField] Color solvedColour = Color.green;
    [SerializeField] Color goneWrongColour = Color.red;

    private void Awake()
    {
        normalColourOuter = OuterRing.color; // Normal colour is set to whatever the colour is when the games starts
        normalColourInner = InnerRing.color; // Normal colour is set to whatever the colour is when the games starts
    }

    private void OnEnable()
    {
        TaskStatus.onTaskBegan += TaskBeingSolved;
        TaskStatus.onTaskFailed += TaskFailed;
        TaskStatus.onTaskCompleted += TaskCompleted;
        TaskStatus.onTaskGoneWrong += TaskGoneWrong;
    }

    private void OnDisable()
    {
        TaskStatus.onTaskBegan -= TaskBeingSolved;
        TaskStatus.onTaskFailed -= TaskFailed;
        TaskStatus.onTaskCompleted -= TaskCompleted;
        TaskStatus.onTaskGoneWrong -= TaskGoneWrong;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.parent.transform.rotation.z * -1.0f);
    }

    void TaskBeingSolved(GameObject task) // Sets the colour for when the task is being solved
    {
        if (task != gameObject) { return; }
        OuterRing.color = beingSolvedColour; // TODO: change this to effect it with xray coding
        GetComponent<SpriteRenderer>().material = xRayWorking;
    }

    void TaskFailed(GameObject task) // Sets the tasks colour back to the orignal colour for when the task went wrong
    {
        if (task != gameObject) { return; }
        if (task.GetComponent<TaskStatus>().isGoingWrong) 
        {
            OuterRing.color = goneWrongColour; // TODO: change this to effect it with xray coding
        }
        else
        {
            OuterRing.color = normalColourOuter; // TODO: change this to effect it with xray coding
            GetComponent<SpriteRenderer>().material = xRayUnfinished;
        }
    }

    void TaskCompleted(GameObject task) // Sets the colour for when the task is completed
    {
        if (task != gameObject) { return; }
        OuterRing.color = solvedColour; // TODO: change this to effect it with xray coding
        GetComponent<SpriteRenderer>().material = xRayFinished;
    }

    void TaskGoneWrong(GameObject task) // Sets the colour for when the task goes wrong
    {
        if (task != gameObject) { return; }
        OuterRing.color = goneWrongColour; // TODO: change this to effect it with xray coding
        GetComponent<SpriteRenderer>().material = xRayUnfinished;
    }
}
