using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyDisplayBack : MonoBehaviour
{
    [SerializeField] GameObject textBack;
    [SerializeField] GameObject keyDisplay;
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
