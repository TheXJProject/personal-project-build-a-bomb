using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLeaderboard : MonoBehaviour
{
    bool played = false;

    private void OnMouseDown()
    {
        GameManager.instance.timeRemainingAfterWin = -1;
        GameManager.instance.ScoreBoard();

        if (!played)
        {
            played = true;

            //play sfx
            AudioManager.instance.PlaySFX("ScoreBoard", true, null, true);
        }
    }
}
