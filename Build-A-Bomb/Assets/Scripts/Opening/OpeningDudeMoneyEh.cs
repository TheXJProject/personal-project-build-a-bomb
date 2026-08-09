using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDudeMoneyEh : MonoBehaviour
{
    public static event Action onDudeMoneyEh;

    private void Start()
    {
        onDudeMoneyEh?.Invoke();
    }
}
