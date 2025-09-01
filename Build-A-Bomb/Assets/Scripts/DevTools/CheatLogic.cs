using Unity.VisualScripting;
using UnityEngine;

public class CheatLogic : MonoBehaviour
{
    // Inspector Adjustable Values:
    [SerializeField] bool canCheatLayers = false;
    [SerializeField] bool canToggleCheats = false;

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
        if (Input.GetKeyDown("up") && canToggleCheats)
        {
            canCheatLayers = !canCheatLayers;
        }
    }

    public bool GetCanCheat() => canCheatLayers;
}
