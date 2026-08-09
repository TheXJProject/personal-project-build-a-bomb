using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoorEvent : MonoBehaviour
{
    public static event Action onDoorSlam;

    private void Start()
    {
        onDoorSlam?.Invoke();
    }
}
