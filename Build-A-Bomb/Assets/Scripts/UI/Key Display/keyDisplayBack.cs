using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyDisplayBack : MonoBehaviour
{
    [SerializeField] GameObject textBack;
    [SerializeField] GameObject keyDisplay;
    public void enableBack()
    {
        textBack.SetActive(true);
    }
    public void disableBack()
    {
        textBack.SetActive(false);
    }

    public void disableAfterAnim()
    {
        keyDisplay.SetActive(false);
    }
}
