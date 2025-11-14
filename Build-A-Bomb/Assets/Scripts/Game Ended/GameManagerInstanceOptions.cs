using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInstanceOptions : MonoBehaviour
{
    public void StartNormalMode()
    {
        GameManager.instance.PlayNormalMode();
    }
    
    public void StartHardMode()
    {
        GameManager.instance.PlayHardMode();
    }

    public void StartMainMenu()
    {
        GameManager.instance.MainMenu();
    }
}
