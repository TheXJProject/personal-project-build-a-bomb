using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginMainMenu : MonoBehaviour
{
    public static Action onBeginMainMenu;

    public bool pressed = false;
    private void OnMouseDown()
    {
        if (!pressed)
        {
            onBeginMainMenu?.Invoke();
            pressed = true;
        }
    }
}
