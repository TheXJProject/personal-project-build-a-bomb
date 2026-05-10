using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenButtonPress : MonoBehaviour
{
    bool played = false;
    public static event Action onEndScreenButtonPressed;

    public void EndScreenButtonPressed()
    {
        onEndScreenButtonPressed?.Invoke();

        if (!played)
        {
            played = true;
            AudioManager.instance.PlaySFX("Every Button", true, null, true);
        }
    }
}
