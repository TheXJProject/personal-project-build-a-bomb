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
    bool refreshing = false;

    async void Start()
    {
        await SpawnTopScores();
        StartRefreshing();
    }

    private void OnEnable()
    {
        refreshing = true;
    }

    private void OnDisable()
    {
        refreshing = false;
    }

    public async void StartRefreshing()
    {
        string leaderboardId = "BuildABombLeaderboard";

        while (refreshing)
        {
            try
            {
                if (refreshing)
                {
                    var response = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
                    UpdateLeaderboardUI(response.Results);
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }

            await System.Threading.Tasks.Task.Delay(500);
        }
    }

    async void UpdateLeaderboardUI(System.Collections.Generic.List<Unity.Services.Leaderboards.Models.LeaderboardEntry> results)
    {
        if (results.Count != entries.Count)
        {
            DestroyAllEntries();
            await SpawnTopScores();
        }
        for (int i = 0; i < results.Count; i++)
        {
            Entry entryInfo = entries[i].GetComponent<Entry>();

            entryInfo.SetName(RemoveHashtag(results[i].PlayerName));
            entryInfo.SetPosition(i+1);
            entryInfo.SetScore((int)(results[i].Score));
        }
    }

    private void DestroyAllEntries()
    {
        foreach (GameObject entry in entries)
        {
            Destroy(entry);
        }
        entries.Clear();
    }

    async System.Threading.Tasks.Task<int> SpawnTopScores()
    {
        string leaderboardId = "BuildABombLeaderboard";

        print("Before sign in");

        while (GameManager.instance.WaitToShowScores)
        {
            await System.Threading.Tasks.Task.Yield();
        }

        print("waitshow score true");
        while (!AuthenticationService.Instance.IsSignedIn)
        {
            await System.Threading.Tasks.Task.Yield();
        }

        print("signed in");

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
