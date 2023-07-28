using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [field: Header("Leaderboard Manager")]
    [field: SerializeField] private GameObject leaderboardPlayerInfoPrefab;
    [field: SerializeField] private GameObject targetForInstantiating;
    [field: SerializeField] private int playerRangeToGetValues;
    [field: SerializeField] private ScrollRect scrollRect;

    const string LeaderboardId = "bb_monthly";
    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; } = 2;
    List<string> FriendIds { get; set; }
    public List<PlayerInfo> players;

    async void Awake()
    {
        Instance = this;
        await UnityServices.InitializeAsync();

    }

    private void Start()
    {
        AuthenticatorManager.Instance.OnLoggedInCompleted += () => { GetPlayerRange(); };
    }
    public void PopulateLeaderboard()
    {
        foreach (var player in players)
        {
            var playerThing = Instantiate(leaderboardPlayerInfoPrefab);
            playerThing.transform.SetParent(targetForInstantiating.transform, false);
            playerThing.GetComponent<AssignLBValues>().AssignValues(player.playerName, (int)player.score);
        }
        GetPlayerScore();
        print("Username: " + Social.localUser.userName);
    }
    public async void AddScore(int score)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
        Debug.Log("Score Added: " + JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScores()
    {
        Offset = 10;
        Limit = 10;
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions{Offset = Offset, Limit = Limit});
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetPlayerRange()
    {
        LeaderboardScores scoresResponse =
            await LeaderboardsService.Instance.GetPlayerRangeAsync(LeaderboardId, new GetPlayerRangeOptions{RangeLimit = RangeLimit});
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        List<LeaderboardEntry> entry = JsonUtility.FromJson<List<LeaderboardEntry>>(JsonConvert.SerializeObject(scoresResponse));
        foreach (LeaderboardEntry lEntry in scoresResponse.Results)
        {
            players.Add(new PlayerInfo(lEntry.PlayerId, lEntry.PlayerName, lEntry.Rank, lEntry.Score));
        }
    }

    public async void GetScoresByPlayerIds()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresByPlayerIdsAsync(LeaderboardId, FriendIds);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    // If the Leaderboard has been reset and the existing scores were archived,
    // this call will return the list of archived versions available to read from,
    // in reverse chronological order (so e.g. the first entry is the archived version
    // containing the most recent scores)
    public async void GetVersions()
    {
        var versionResponse =
            await LeaderboardsService.Instance.GetVersionsAsync(LeaderboardId);

        // As an example, get the ID of the most recently archived Leaderboard version
        VersionId = versionResponse.Results[0].Id;
        Debug.Log(JsonConvert.SerializeObject(versionResponse));
    }

    public async void GetVersionScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedVersionScores()
    {
        Offset = 10;
        Limit = 10;
        var scoresResponse =
            await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId, new GetVersionScoresOptions{Offset = Offset, Limit = Limit});
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerVersionScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetVersionPlayerScoreAsync(LeaderboardId, VersionId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    //public PlayerInfoArray CreatePlayersFromJSON(string json)
    //{
    //    PlayerInfoArray playerInfoArray = new PlayerInfoArray();
    //    playerInfoArray= playerInfoArray.CreateFromJSON(json);


    //        Debug.Log(playerInfoArray.players.Count);


    //    // Use JsonUtility to deserialize the JSON string
    //    return playerInfoArray;
    //}


}
