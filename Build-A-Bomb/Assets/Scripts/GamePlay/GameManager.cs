using System;
using System.Collections;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

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
    public static event Action onLevelStartedLoading;
    public static event Action onLevelFinishedLoading;

    [SerializeField] Animator sceneTransitions;
    [SerializeField] float animationTime;
    [SerializeField] float musicTime;

    [HideInInspector] public bool gameplayStartsFromWithinGameplayScene = false;

    // Runtime Variables:
    [HideInInspector] public bool waitForAnimation = false;
    public static GameManager instance;

    [Header("Other:")]
    [SerializeField] public bool hardMode;
    public FrameRateSetting TargetFrameRate;
    public float timeRemainingAfterWin = -1;
    public bool WaitToShowScores = false;
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

    async void Awake()
    {
        // If we haven't already initialised an instance of the game manager
        if (instance == null)
        {
            // Make this instance a singleton
            DontDestroyOnLoad(gameObject);
            instance = this;
            // If loading the scenemanager within the gameplay scene, set this variable, since usually the scene plays after sceneload between scenes has finished
            if (SceneManager.GetActiveScene().name.Equals("GameplayScene")) 
                gameplayStartsFromWithinGameplayScene = true;

            // Setup leaderboard anonymous login
            await InitializeAndLogin();

        }
        else
        {
            // Destroy this gameobject if another game manager already exists (singleton)
            Destroy(gameObject);
        }
    }

    public async System.Threading.Tasks.Task InitializeAndLogin()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }


    public void PlayNormalMode()
    {
        hardMode = false;
        LoadSceneWithAnim("GameplayScene");
    }

    public void PlayHardMode()
    {
        hardMode = true;
        LoadSceneWithAnim("GameplayScene");
    }

    public void PlayTutorial()
    {
        hardMode = true;
        LoadSceneWithAnim("TutorialScene");
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
        timeRemainingAfterWin = -1;
        LoadSceneWithAnim("Main Menu");
    }
    public void MainMenuFromOpening()
    {
        LoadSceneWithAnim("Main Menu", true);
    }
    
    public void ScoreBoard()
    {
        GameManager.instance.WaitToShowScores = true;
        LoadSceneWithAnim("ScoreBoard");
    }

    void LoadSceneWithAnim(String sceneName, bool leaveSceneAlternate = false)
    {
        if (midSceneTransition) return;
        midSceneTransition = true;

        // Start fading out music
        // Set all music groups to zero volume
        MixerFXManager.instance.SetMusicOverallParam(EX_PARA.VOLUME, musicTime, 0);
        MixerFXManager.instance.SetSfxOverallParam(EX_PARA.VOLUME, musicTime, 0);

        StartCoroutine(WaitForTransition(sceneName, leaveSceneAlternate));
    }

    IEnumerator WaitForTransition(String sceneName, bool leaveSceneAlternate = false)
    {
        onLevelStartedLoading?.Invoke();

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

        onLevelFinishedLoading?.Invoke();
    }

    void determineGameStarted(GameObject triggerLayer)
    {
        if (triggerLayer.GetComponent<LayerStatus>().layer == 0)
        {
            onGameBegan?.Invoke();
        }
    }
}
