using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIfNotFromMainMenu : MonoBehaviour
{
    private void Awake()
    {
        bool cameFromMainMenu = GameManager.instance.timeRemainingAfterWin < 0;
        gameObject.SetActive(!cameFromMainMenu);
    }
}
