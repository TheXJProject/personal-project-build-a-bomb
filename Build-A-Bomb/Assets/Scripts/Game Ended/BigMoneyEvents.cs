using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMoneyEvents : MonoBehaviour
{
    public static event Action onBigTextEnters;
    public static event Action onMoneyTextEnters;

    public void TriggerBigTextEvent()
    {
        onBigTextEnters?.Invoke();
    }

    public void TriggerMoneyTextEvent()
    {
        onMoneyTextEnters?.Invoke();
    }
}
