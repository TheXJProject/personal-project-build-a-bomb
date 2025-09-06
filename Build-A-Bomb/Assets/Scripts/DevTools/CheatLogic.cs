using Unity.VisualScripting;
using UnityEngine;

public class CheatLogic : MonoBehaviour
{
    // Inspector Adjustable Values:
    [SerializeField] bool canToggleCheats = false;

    [Header("(Cheat List)\n")]
    [SerializeField] bool canCheatLayers = false;
    [SerializeField] bool pausedTimer = false;

    // Runtime Variables:
    public static CheatLogic cheatTool;

    private void Awake()
    {
        // If cheatTool doesn't exist
        if (cheatTool == null)
        {
            // Make this a singleton
            cheatTool = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Otherwise, remove this instance
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && canToggleCheats)
        {
            canCheatLayers = !canCheatLayers;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && canToggleCheats)
        {
            pausedTimer = !pausedTimer;
        }
    }

    public bool GetCanCheatLayers() => canCheatLayers;
    public bool GetPauseTimer() => pausedTimer;
}
