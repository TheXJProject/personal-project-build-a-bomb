using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class EntryController : MonoBehaviour
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform contentRegion;
    private List<GameObject> entries = new List<GameObject>();

    LeaderboardScoresPage test;

    async void Start()
    {
        await SpawnTopScores();
    }

    async System.Threading.Tasks.Task<int> SpawnTopScores()
    {
        string leaderboardId = "BuildABombLeaderboard";

        while (GameManager.instance.WaitToShowScores)
        {
            await System.Threading.Tasks.Task.Yield();
        }
        while (!AuthenticationService.Instance.IsSignedIn)
        {
            await System.Threading.Tasks.Task.Yield();
        }


        // Get top scores (adjust limit as needed)
        var scores = await LeaderboardsService.Instance.GetScoresAsync(
            leaderboardId,
            new GetScoresOptions { Limit = 100 }
        );

        int rank = 1;

        foreach (var result in scores.Results)
        {
            GameObject newEntry = Instantiate(entryPrefab, contentRegion);
            Entry entryInfo = newEntry.GetComponent<Entry>();

            entryInfo.SetName(RemoveHashtag(result.PlayerName));
            entryInfo.SetPosition(rank);
            entryInfo.SetScore((int)(result.Score));

            entries.Add(newEntry);
            
            rank++;
        }

        return rank;
    }

    string RemoveHashtag(string input)
    {
        int index = input.IndexOf('#');
        return index >= 0 ? input.Substring(0, index) : input;
    }
}
