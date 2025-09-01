using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Runtime Variables:
    public static GameManager instance;
    public bool hardMode;

    private void OnEnable()
    {
        BombStatus.onBombFinished += WinGame;
        DeathTimer.onTimerZero += LoseGame;
        LivesTracker.onNoLives += LoseGame;
    }

    private void OnDisable()
    {
        BombStatus.onBombFinished -= WinGame;
        DeathTimer.onTimerZero -= LoseGame;
        LivesTracker.onNoLives -= LoseGame;
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
        SceneManager.LoadScene("GameplayScene");
    }

    public void PlayHardMode()
    {
        // TODO: apply transistion effects
        hardMode = true;
        SceneManager.LoadScene("GameplayScene");
    }

    void LoseGame()
    {
        // If we are cheating, we can't lose
        if (!CheatLogic.cheatTool.GetCanCheat())
        {
            SceneManager.LoadScene("LoseScene");
        }
    }

    void WinGame()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
