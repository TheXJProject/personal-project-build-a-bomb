using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;

public class GetAndShowPosition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI position;

    async void Start()
    {
        while (!AuthenticationService.Instance.IsSignedIn)
        {
            await System.Threading.Tasks.Task.Yield();
        }

        int score = ((int)(GameManager.instance.timeRemainingAfterWin * 100));
        int rank = await GetPredictedRank(score);
        position.text = rank.ToString();
    }

    async System.Threading.Tasks.Task<int> GetPredictedRank(int newScore)
    {
        string leaderboardId = "BuildABombLeaderboard";

        // Get top scores (adjust limit as needed)
        var scores = await LeaderboardsService.Instance.GetScoresAsync(
            leaderboardId,
            new GetScoresOptions { Limit = 100 }
        );

        int rank = 1;

        foreach (var entry in scores.Results)
        {
            if (newScore < entry.Score)
            {
                rank++;
            }
            else
            {
                break;
            }
        }

        return rank;
    }
}
