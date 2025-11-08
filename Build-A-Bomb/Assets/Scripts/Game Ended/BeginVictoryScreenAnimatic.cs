using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginVictoryScreenAnimatic : MonoBehaviour
{
    public static event Action onVictoryScreenAnimaticStart;
    public static event Action onVictoryScreenButtonsAppear;
    [Header("Set variables for the end screen animatic")]
    [SerializeField] float timeForAnimaticToStart = 3f;
    [SerializeField] float timeForButtonsToAppear = 5f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(timeForAnimaticToStart);
        onVictoryScreenAnimaticStart?.Invoke();
        yield return new WaitForSeconds(timeForButtonsToAppear);
        onVictoryScreenButtonsAppear?.Invoke();
    }
}
