using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKeyInput : MonoBehaviour
{
    public static PlayerKeyInput instance;

    public PlayerInputActions playerControls;
    public InputAction[] keys = new InputAction[26];
    public int[] keysDown = new int[26];

    string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playerControls = new PlayerInputActions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < 26; i++)
        {
            keys[i] = playerControls.FindAction(alphabet[i]);
            keys[i].Enable();
            keys[i].performed += KeyDown;
            keys[i].canceled += KeyUp;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < 26; i++)
        {
            keys[i].Disable();
            keys[i].performed -= KeyDown;
            keys[i].canceled -= KeyUp;
        }
    }

    void KeyDown(InputAction.CallbackContext context)
    {
        for (int i = 0; i < 26; i++)
        {
            if (String.Equals(context.action.name, alphabet[i]))
            {
                keysDown[i] = 1;
                break;
            }
        }
    }

    void KeyUp(InputAction.CallbackContext context)
    {
        for (int i = 0; i < 26; i++)
        {
            if (String.Equals(context.action.name, alphabet[i]))
            {
                keysDown[i] = 0;
                break;
            }
        }
    }
}
