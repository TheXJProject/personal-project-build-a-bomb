using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLeaderboard : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.instance.timeRemainingAfterWin = -1;
        GameManager.instance.ScoreBoard();
    }
}
