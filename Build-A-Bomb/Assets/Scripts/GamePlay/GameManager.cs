using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Runtime Variables:
    public static GameManager instance;
    public bool hardMode;

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

    public void LooseGame()
    {
        SceneManager.LoadScene("LooseScene");
    }

    public void WinGame()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
