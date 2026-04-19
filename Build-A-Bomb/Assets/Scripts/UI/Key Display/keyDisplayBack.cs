using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyDisplayBack : MonoBehaviour
{
    public static event Action onKeyDisplayShown;   

    [SerializeField] GameObject textBack;
    [SerializeField] GameObject keyDisplay;

    private void OnEnable()
    {
        onKeyDisplayShown?.Invoke();
    }

    public void enableKeyBack()
    {
        textBack.SetActive(true);
    }
    public void disableKeyBack()
    {
        textBack.SetActive(false);
    }

    public void disableAfterAnim()
    {
        keyDisplay.SetActive(false);
    }
}
