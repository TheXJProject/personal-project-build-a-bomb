using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKeyInput : MonoBehaviour
{
    // ==== For Debugging ====
    bool Msg = false;

    // Event Actions:
    public static event Action<int> onKeyPressed;
    public static event Action<int> onKeyReleased;

    // Constant values:
    public readonly string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    // Static values:
    public static PlayerKeyInput instance;

    // Initialise In Inspector:
    public PlayerInputActions playerControls;

    // Runtime Variables:
    public InputAction[] keys = new InputAction[26];
    public int[] keysDown = new int[26];
    public int[] keysInUse = new int[26];
    System.Random rnd = new System.Random();
    [HideInInspector] public int keysInUseTotal = 0;

    private void Awake()
    {
        // Instantiate singleton instance
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
        playerControls.Mouse.RightClick.Enable();
        // Instantiate Input actions for all the keys
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
        playerControls.Mouse.RightClick.Disable();

        for (int i = 0; i < 26; i++)
        {
            keys[i].Disable();
            keys[i].performed -= KeyDown;
            keys[i].canceled -= KeyUp;
        }
    }

    /// <summary>
    /// Depending on key calling this function, sets that key to down (1)
    /// </summary>
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

    /// <summary>
    /// Depending on key calling this function, sets that key to up (0)
    /// </summary>
    /// <param name="context"></param>
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

    /// <summary>
    /// Keeps track of which keys are currently in use for tasks
    /// </summary>
    void AddKeysUsed(GameObject task)
    {
        foreach (var key in task.GetComponent<TaskStatus>().keys)
        {
            keysInUse[key] = 1;
            TotalKeysInUse();
        }
    }

    /// <summary>
    /// Keeps track of which keys are currently NOT in use for tasks
    /// </summary>
    void RemoveKeysUsed(GameObject task)
    {
        foreach (var key in task.GetComponent<TaskStatus>().keys)
        {
            keysInUse[key] = 0;
            TotalKeysInUse();
        }
    }

    /// <summary>
    /// Returns a list of keys (between 0 and 25) by checking which arn't currently being used for tasks and if they are unique (if possible)
    /// </summary>
    public List<int> DetermineFreeKeys(int keysRequired) 
    {
        List<int> freeKeys = new List<int>();
        List<int> uniqueUnpressed = new List<int>();
        int newKey;
        bool isUnique;

        // While loop goes until it has the number of keys the function is required to return in a list
        while (freeKeys.Count < keysRequired)
        {
            // For each letter in the alphabet
            for (global::System.Int32 i = 0; i < 26; i++)
            {
                isUnique = true;

                // If the letter is already in keys, then assign it to not being unique
                foreach (var item in freeKeys)
                {
                    if (item == i)
                    {
                        isUnique = false;
                        break;
                    }
                }

                // If the letter isn't being pressed and is unique to every letter already in the list, then add it to the list
                if (keysInUse[i] == 0 && isUnique)
                {
                    uniqueUnpressed.Add(i);
                }
            }

            // Now that all the unique and unpresed keys are in a list, we generate a random key from one of these
            if (uniqueUnpressed.Count > 0) { newKey = uniqueUnpressed[rnd.Next(uniqueUnpressed.Count)]; }
            else 
            {
                // Or just generate a random key if there doesn't exist any unpressed unique keys in uniqueUnpressed
                newKey = rnd.Next(26);
                Debug.LogWarning("All 26 keys are in use you flippin idiot! >:(");
            }
            freeKeys.Add(newKey); 
        }
        return freeKeys;
    }

    /// <summary>
    /// Keeps track of the number of keys are currently in use for tasks
    /// </summary>
    void TotalKeysInUse()
    {
        int total = 0;
        foreach (var key in keysInUse)
        {
            total += key;
        }
        keysInUseTotal = total;

        if (Msg) Debug.Log("Total keys in use: " + keysInUseTotal);
    }
}
