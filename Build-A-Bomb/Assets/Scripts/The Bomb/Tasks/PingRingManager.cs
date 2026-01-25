using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingRingManager : MonoBehaviour
{
    [Header("Initialise ping ring in inspector:")]
    [SerializeField] GameObject pingRing;
    PingRing pingLogic;

    private void Awake()
    {
        pingLogic = pingRing.GetComponent<PingRing>();
    }

    public void DoPingComplete()
    {
        pingRing.SetActive(false);
        pingRing.SetActive(true);
    }

    public void DoPingGoingWrong()
    {
        pingRing.SetActive(false);
        pingLogic.goingWrong = true;
        pingRing.SetActive(true);
    }
}
