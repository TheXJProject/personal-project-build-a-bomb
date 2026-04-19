using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEventTrigger : MonoBehaviour
{
    public static event Action onArrowAppear;

    private void OnEnable()
    {
        onArrowAppear?.Invoke();
    }
}
