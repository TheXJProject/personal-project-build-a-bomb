using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKeyInput : MonoBehaviour
{
    // Singleton instance
    public static PlayerKeyInput instance;

    // Reference alphabet
    public readonly string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    // Event Actions for key presses
    public static event Action<int> onKeyPressed;
    public static event Action<int> onKeyReleased;

    // Input actions reference
    public PlayerInputActions playerControls;

    // Arrays for tracking the keys the player uses
    public InputAction[] keys = new InputAction[26];
    public int[] keysDown = new int[26];
    public int[] keysInUse = new int[26];

    // Miscellaneous variables
    System.Random rnd = new System.Random();
    public int whileLoopLimit = 100;

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
        TaskStatus.onTaskBegan += AddKeysUsed;
        TaskStatus.onTaskFailed += RemoveKeysUsed;
        TaskStatus.onTaskCompleted += RemoveKeysUsed;

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
        TaskStatus.onTaskBegan -= AddKeysUsed;
        TaskStatus.onTaskFailed -= RemoveKeysUsed;
        TaskStatus.onTaskCompleted -= RemoveKeysUsed;

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
                onKeyPressed?.Invoke(i);
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
                onKeyReleased?.Invoke(i);
                break;
            }
        }
    }

    void AddKeysUsed(GameObject task)
    {
        foreach (var key in task.GetComponent<TaskStatus>().keys)
        {
            keysInUse[key] = 1;
        }
    }

    void RemoveKeysUsed(GameObject task)
    {
        foreach (var key in task.GetComponent<TaskStatus>().keys)
        {
            keysInUse[key] = 0;
        }
    }

    public List<int> DetermineFreeKeys(int keysRequired)
    {
        List<int> freeKeys = new List<int>();
        List<int> uniqueUnpressed = new List<int>();
        int newKey;
        bool isUnique;

        while (freeKeys.Count < keysRequired)
        {
            for (global::System.Int32 i = 0; i < 26; i++)
            {
                isUnique = true;
                foreach (var item in freeKeys)
                {
                    if (item == i)
                    {
                        isUnique = false;
                        break;
                    }
                }
                if (keysInUse[i] == 0 && isUnique)
                {
                    uniqueUnpressed.Add(i);
                }
            }
            if (uniqueUnpressed.Count > 0) { newKey = uniqueUnpressed[rnd.Next(uniqueUnpressed.Count)]; }
            else 
            { 
                newKey = rnd.Next(26);
                Debug.LogWarning("All 26 keys are in use you flippin idiot");
            }
            freeKeys.Add(newKey); 
        }
        return freeKeys;
    }
}
