using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSelection : MonoBehaviour
{
    private void OnMouseDown()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
