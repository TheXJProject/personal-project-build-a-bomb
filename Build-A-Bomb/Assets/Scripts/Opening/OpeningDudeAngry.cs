using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDudeAngry : MonoBehaviour
{
    public static event Action onDudeAngry;

    private void Start()
    {
        onDudeAngry?.Invoke();
    }
}
