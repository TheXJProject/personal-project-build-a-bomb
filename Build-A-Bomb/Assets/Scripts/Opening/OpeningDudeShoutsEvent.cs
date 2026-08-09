using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDudeShoutsEvent : MonoBehaviour
{
    public static event Action onDudeShoutsEvent;

    private void Start()
    {
        onDudeShoutsEvent?.Invoke();
    }
}
