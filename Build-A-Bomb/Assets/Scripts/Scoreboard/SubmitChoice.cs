using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;

public class SubmitChoice : MonoBehaviour
{
    [SerializeField] private GameObject yesOrNo;
    [SerializeField] private GameObject enterName;
    [SerializeField] private GameObject submitChoiceOverall;
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private Scoreboard scoreBoardInfo;

    private string playerName = "Test Name";

    async void Start()
    {
        bool offerSubmitNewScore = await IsNewScoreHigher((int)(GameManager.instance.timeRemainingAfterWin*100));

        yesOrNo.SetActive(true);
        submitChoiceOverall.SetActive(offerSubmitNewScore);

        enterName.SetActive(false);
        scoreBoard.SetActive(!offerSubmitNewScore);
    }

    public void PlayerClickedYes()
    {
        yesOrNo.SetActive(false);
        enterName.SetActive(true);
    }

    async System.Threading.Tasks.Task<bool> IsNewScoreHigher(int newScore)
    {
        string leaderboardId = "BuildABombLeaderboard";

        if (newScore <= 0)
        {
            return false;
        }

        try
        {
            while (!AuthenticationService.Instance.IsSignedIn)
            {
                await System.Threading.Tasks.Task.Yield();
            }
            await System.Threading.Tasks.Task.Delay(200);

            var playerScore = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);

            return newScore > playerScore.Score;
        }
        catch
        {
            return true;
        }
    }


    public async void PlayerEnteredName(string name)
    {
        if (!ValidName(ref name))
        {
            Debug.Log("TODO: Make player aware: Not a valid name, name should be only letters numbers or underscores and be at least 3 characters long");
            return;
        }

        playerName = name;
        await SetPlayerName(name);
        submitChoiceOverall.SetActive(false);
        scoreBoard.SetActive(true);
        scoreBoardInfo.SubmitScore((int)(GameManager.instance.timeRemainingAfterWin * 100));
        GameManager.instance.WaitToShowScores = false;
    }

    async System.Threading.Tasks.Task SetPlayerName(string playerName)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error setting name: {e.Message}");
        }
    }

    private bool ValidName(ref string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (name.Length < 3 || name.Length > 16)
        {
            return false;
        }

        name = name.Replace("\u200B", "").Trim();

        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
            {
                return false;
            }
        }
        return true;
    }
}
