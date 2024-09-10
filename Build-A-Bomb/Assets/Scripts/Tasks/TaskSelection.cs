using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSelection : MonoBehaviour
{
    TaskStatus task;
    private void OnEnable()
    {
        task = GetComponent<TaskStatus>();
    }
    private void OnMouseDown()
    {
        task.TaskSelected();
    }
}
