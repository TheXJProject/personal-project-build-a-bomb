using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Leaderboards;
using System;

public class Scoreboard : MonoBehaviour
{
    public static event Action<MUSIC_TRACKS, bool, double> onScoreBoardBegin;

    [SerializeField] float startMusicTime;

    private void Start()
    {
        onScoreBoardBegin?.Invoke(MUSIC_TRACKS.SCOREBOARD, true, startMusicTime);
    }

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
