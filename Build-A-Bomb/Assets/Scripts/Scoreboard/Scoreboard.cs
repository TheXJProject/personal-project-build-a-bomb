using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Leaderboards;

public class Scoreboard : MonoBehaviour
{
    public async void SubmitScore(int score)
    {
        await SubmitScoreAsync(score);
    }

    async System.Threading.Tasks.Task SubmitScoreAsync(int score)
    {
        try
        {
            string leaderboardId = "BuildABombLeaderboard";

            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error submitting score: {e.Message}");
        }
    }

}
