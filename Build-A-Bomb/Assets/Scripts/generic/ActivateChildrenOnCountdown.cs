using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChildrenOnCountdown : MonoBehaviour
{
    private void OnEnable()
    {
        GameStartCount.onCountdownFinished += ActivateChildren;
    }

    private void OnDisable()
    {
        GameStartCount.onCountdownFinished -= ActivateChildren;
    }

    void ActivateChildren()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(true);
        }
    }
}
