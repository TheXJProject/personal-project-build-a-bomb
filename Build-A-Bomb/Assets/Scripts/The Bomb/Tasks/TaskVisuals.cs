using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskVisuals : MonoBehaviour
{
    Color normalColour;
    Color beingSolvedColour = Color.yellow;
    Color solvedColour = Color.green;
    Color goneWrongColour = Color.red;

    private void Awake()
    {
        normalColour = GetComponent<SpriteRenderer>().color;
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

    void TaskBeingSolved(GameObject task)
    {
        if (task != gameObject) { return; }
        GetComponent<SpriteRenderer>().color = beingSolvedColour;
    }
    
    void TaskFailed(GameObject task)
    {
        if (task != gameObject) { return; }
        if (task.GetComponent<TaskStatus>().isGoingWrong) 
        { 
            GetComponent<SpriteRenderer>().color = goneWrongColour; 
        }
        else
        {
            GetComponent<SpriteRenderer>().color = normalColour;
        }
    }

    void TaskCompleted(GameObject task)
    {
        if (task != gameObject) { return; }
        GetComponent<SpriteRenderer>().color = solvedColour;
    }

    void TaskGoneWrong(GameObject task)
    {
        if (task != gameObject) { return; }
        GetComponent<SpriteRenderer>().color = goneWrongColour;
    }
}
