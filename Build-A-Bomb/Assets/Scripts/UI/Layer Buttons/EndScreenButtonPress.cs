using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenButtonPress : MonoBehaviour
{
    public static event Action onEndScreenButtonPressed;

    public void EndScreenButtonPressed()
    {
        onEndScreenButtonPressed?.Invoke();
    }
}
