using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerVisuals : MonoBehaviour
{
    [SerializeField] HammerTask taskLogic;

    [HideInInspector] public bool stillHitting = false;


    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void animationLooksFinished()
    {
        taskLogic.CheckFinished();
    }

    public void checkStillHitting()
    {
        if (stillHitting)
        {
            stillHitting = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
