using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct FrameRateSetting
    {
        public bool enabled;
        [Range(1, 1000)] public int fps;
    }

    // Events
    public static event Action onGameBegan;
    public static event Action onLevelFinshedLoading;

    [SerializeField] Animator sceneTransitions;
    [SerializeField] float animationTime;

    // Runtime Variables:
    [HideInInspector] public bool waitForAnimation = false;
    public static GameManager instance;
    public bool hardMode;
    public FrameRateSetting TargetFrameRate;
    public float timeRemainingAfterWin;
    public int currentLayer = 0;
    bool midSceneTransition = false;

    private void Start()
    {
        if (TargetFrameRate.enabled)
        {
            Application.targetFrameRate = TargetFrameRate.fps;
        }
    }

    private void OnEnable()
    {
        BombStatus.onLayerCreated += determineGameStarted;
    }

    private void OnDisable()
    {
        BombStatus.onLayerCreated -= determineGameStarted;
    }

    private void Awake()
    {
        // If we haven't already initialised an instance of the game manager
        if (instance == null)
        {
            // Make this instance a singleton
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            // Destroy this gameobject if another game manager already exists (singleton)
            Destroy(gameObject);
        }
    }

    // Will control scenes
    // And, audio playing?
    // TODO: this

    public void PlayNormalMode()
    {
        // TODO: apply transistion effects
        hardMode = false;
        LoadSceneWithAnim("GameplayScene");
    }

    public void PlayHardMode()
    {
        // TODO: apply transistion effects
        hardMode = true;
        LoadSceneWithAnim("GameplayScene");
    }

    void LoseGame()
    {
        // If we are cheating, we can't lose
        if (!CheatLogic.cheatTool.GetCanCheatLayers())
        {
            LoadSceneWithAnim("GameplayScene");
        }
    }

    void WinGame()
    {
        LoadSceneWithAnim("WinScene");
    }

    public void MainMenu()
    {
        LoadSceneWithAnim("Main Menu");
    }
    public void MainMenuFromOpening()
    {
        LoadSceneWithAnim("Main Menu", true);
    }

    void LoadSceneWithAnim(String sceneName, bool leaveSceneAlternate = false)
    {
        if (midSceneTransition) return;
        midSceneTransition = true;
        StartCoroutine(WaitForTransition(sceneName, leaveSceneAlternate));
    }

    IEnumerator WaitForTransition(String sceneName, bool leaveSceneAlternate = false)
    {
        waitForAnimation = true;
        if (leaveSceneAlternate) sceneTransitions.SetTrigger("leaveSceneHole");
        else sceneTransitions.SetTrigger("leaveScene");
        while (waitForAnimation) yield return null;

        SceneManager.LoadScene(sceneName);
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone) yield return null;

        sceneTransitions.SetTrigger("enterScene");
        while (waitForAnimation) yield return null;
        midSceneTransition = false;
        onLevelFinshedLoading?.Invoke();
    }

    void determineGameStarted(GameObject triggerLayer)
    {
        if (triggerLayer.GetComponent<LayerStatus>().layer == 0)
        {
            onGameBegan?.Invoke();
        }
    }
}
