using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TypeOfLeaderBoard
{
    DailyLeaderboard,
    WeeklyLeaderboard,
    MonthlyLeaderboard
}
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [field: Header("Leaderboard Manager")]
    [field: SerializeField] private GameObject leaderboardPlayerInfoPrefab;
    [field: SerializeField] private GameObject targetForInstantiating;
    [field: SerializeField] private int playerRangeToGetValues;
    [field: SerializeField] private Button leaderboardButton;

    [field: Header("Leaderboard Names")]
    [field: SerializeField] private string dailyLB = "bb_daily";
    [field: SerializeField] private GameObject dailyLBHolder;
    [field: SerializeField] private string weeklyLB = "bb_weekly";
    [field: SerializeField] private GameObject weeklyLBHolder;
    [field: SerializeField] private string monthlyLB = "bb_monthly";
    [field: SerializeField] private GameObject monthlyLBHolder;
    [field: SerializeField] private RawImage img;
    [field: SerializeField] private LoadingManager loadingManager;

    [field: Header("Leaderboard Names")]
    [field: SerializeField] private GameObject dailyPlayerValues;
    [field: SerializeField] private GameObject weeklyPlayerValues;
    [field: SerializeField] private GameObject monthlyPlayerValues;
    private GameObject playerValuesObject;

    string LeaderboardId = "bb_monthly";
    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; } = 2;
    List<string> FriendIds { get; set; }
    private void Start()
    {
        
        UnityServices.InitializeAsync();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            leaderboardButton.interactable = false;

            AuthenticatorManager.Instance.OnLoggedInCompleted += () => { Instance = this; FirstStartingStuff(); };
        }
        else
        {
            Instance = this;
        }
    }
    private async void FirstStartingStuff()
    {
        await Task.Delay(300);
        CurrencyDataHandler.instance.AddValuesInStarting();
        await Task.Delay(300);
        await PopulateLeaderboard(TypeOfLeaderBoard.DailyLeaderboard);
        await PopulateLeaderboard(TypeOfLeaderBoard.WeeklyLeaderboard);
        await PopulateLeaderboard(TypeOfLeaderBoard.MonthlyLeaderboard);
        await Task.Delay(300);
        leaderboardButton.interactable = true;
        loadingManager.CheckWhatToDo();
    }

    public async Task PopulateLeaderboard(TypeOfLeaderBoard typeOfLeaderboard)
    {

        List<PlayerInfo> players = new List<PlayerInfo>();

        switch (typeOfLeaderboard)
        {
            case TypeOfLeaderBoard.DailyLeaderboard: LeaderboardId = dailyLB; targetForInstantiating = dailyLBHolder; playerValuesObject = dailyPlayerValues; break;
            case TypeOfLeaderBoard.WeeklyLeaderboard: LeaderboardId = weeklyLB; targetForInstantiating = weeklyLBHolder; playerValuesObject = weeklyPlayerValues; break;
            case TypeOfLeaderBoard.MonthlyLeaderboard: LeaderboardId = monthlyLB; targetForInstantiating = monthlyLBHolder; playerValuesObject = monthlyPlayerValues ; break;
            default: HelperClass.DebugError("Type Of Leaderboard Not Specified In Populating!"); break;
        }
        //Deleting Existing Entries If Any :)
        if (targetForInstantiating.gameObject.transform.childCount > 0)
        {
            foreach (Transform child in targetForInstantiating.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }
        try
        {
            await GetPlayerRange(players);
        }
        catch(Exception e) 
        {
            HelperClass.DebugError("Debugged Error Cuz No Values In Leaderboard: " + e.Message);
            players = new List<PlayerInfo>();
        }

        if(players.Count > 0) 
        { 
            await Task.Delay(1000);
            LeaderboardEntry playerScoreThing = await GetPlayerScore();
            foreach (var player in players)
            {
                var playerThing = Instantiate(leaderboardPlayerInfoPrefab);
                playerThing.transform.SetParent(targetForInstantiating.transform, false);
                bool isPlayer = false;
                if (player.playerId == playerScoreThing.PlayerId.ToString())
                {
                    isPlayer = true;
                }
                playerThing.GetComponent<AssignLBValues>().AssignValues(player.playerName, (int)player.score, (player.rank+1), null, isPlayer);
            }
            playerValuesObject.GetComponent<AssignLBValues>().AssignValues(playerScoreThing.PlayerName, (int)playerScoreThing.Score, (playerScoreThing.Rank+1), null, true);
        }
        else
        {
            playerValuesObject.GetComponent<AssignLBValues>().AssignValues(CurrencyDataHandler.instance.ReturnSavedValues().playerUsername, 0, 0 , null, true);
        }
    }
    public async void AddScore(int score, TypeOfLeaderBoard typeOfLeaderBoard)
    {
        switch (typeOfLeaderBoard)
        {
            case TypeOfLeaderBoard.DailyLeaderboard: LeaderboardId = dailyLB; break;
            case TypeOfLeaderBoard.WeeklyLeaderboard: LeaderboardId = weeklyLB; break;
            case TypeOfLeaderBoard.MonthlyLeaderboard: LeaderboardId = monthlyLB; break;
            default: HelperClass.DebugError("Type Of Leaderboard Not Specified In Adding!"); break;
        }

        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
        HelperClass.DebugMessage("Score Added: " + JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScores()
    {
        Offset = 10;
        Limit = 10;
        var scoresResponse =
        await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = Offset, Limit = Limit });
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoresResponse));
    }

    public async Task<LeaderboardEntry> GetPlayerScore()
    {
        var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        return scoreResponse;
    }

    public async Task<List<PlayerInfo>> GetPlayerRange(List<PlayerInfo> players)
    {
        LeaderboardScores scoresResponse =
            await LeaderboardsService.Instance.GetPlayerRangeAsync(LeaderboardId, new GetPlayerRangeOptions { RangeLimit = 100 });
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoresResponse));
        //List<LeaderboardEntry> entry = JsonUtility.FromJson<List<LeaderboardEntry>>(JsonConvert.SerializeObject(scoresResponse));
            foreach (LeaderboardEntry lEntry in scoresResponse.Results)
            {
                players.Add(new PlayerInfo(lEntry.PlayerId, lEntry.PlayerName, lEntry.Rank, lEntry.Score));
            }
            return players;
    }

    public async void GetScoresByPlayerIds()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresByPlayerIdsAsync(LeaderboardId, FriendIds);
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoresResponse));
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
        HelperClass.DebugMessage(JsonConvert.SerializeObject(versionResponse));
    }

    public async void GetVersionScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId);
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedVersionScores()
    {
        Offset = 10;
        Limit = 10;
        var scoresResponse =
            await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardId, VersionId, new GetVersionScoresOptions { Offset = Offset, Limit = Limit });
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerVersionScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetVersionPlayerScoreAsync(LeaderboardId, VersionId);
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoreResponse));
    }

    #region For Reset
    //private async void CheckAndResetValues()
    //{
    //    Debug.Log("reset");
    //    if (await GetPlayerScore(dailyLB) == 0)
    //    {
    //        Debug.Log("Daily " + await GetPlayerScore(dailyLB));
    //        CurrencyDataHandler.instance.ReturnSavedValues().dailyLeaderboard = 0;
    //        AddScore(0, TypeOfLeaderBoard.DailyLeaderboard);
    //    Debug.Log("reset d");
    //    }

    //    if (await GetPlayerScore(weeklyLB) == 0)
    //    {
    //        Debug.Log("Daily " + await GetPlayerScore(weeklyLB));
    //        CurrencyDataHandler.instance.ReturnSavedValues().weeklyLeaderboard = 0;
    //        AddScore(0, TypeOfLeaderBoard.WeeklyLeaderboard);
    //    Debug.Log("reset w");
    //    }

    //    if (await GetPlayerScore(monthlyLB) == 0)
    //    {
    //        Debug.Log("Daily " + await GetPlayerScore(monthlyLB));
    //        CurrencyDataHandler.instance.ReturnSavedValues().monthlyLeaderboard = 0;
    //        AddScore(0, TypeOfLeaderBoard.MonthlyLeaderboard);
    //    Debug.Log("reset m");
    //    }

    //    CurrencyDataHandler.instance.SaveCurrencyData();
    //}

    //private async void CheckIfGameGotUninstalled()
    //{
    //    Debug.Log("Unins");
    //    if (CurrencyDataHandler.instance.ReturnSavedValues().dailyLeaderboard != await GetPlayerScore(dailyLB))
    //    {
    //        CurrencyDataHandler.instance.ReturnSavedValues().dailyLeaderboard = await GetPlayerScore(dailyLB);
    //        Debug.Log("Unins daily");
    //    }

    //    if (CurrencyDataHandler.instance.ReturnSavedValues().weeklyLeaderboard != await GetPlayerScore(weeklyLB))
    //    {
    //        CurrencyDataHandler.instance.ReturnSavedValues().weeklyLeaderboard = await GetPlayerScore(weeklyLB);
    //        Debug.Log("Unins week");
    //    }

    //    if (CurrencyDataHandler.instance.ReturnSavedValues().monthlyLeaderboard != await GetPlayerScore(monthlyLB))
    //    {
    //        CurrencyDataHandler.instance.ReturnSavedValues().monthlyLeaderboard = await GetPlayerScore(monthlyLB);
    //        Debug.Log("Unins month");
    //    }

    //    CurrencyDataHandler.instance.SaveCurrencyData();
    //}
    #endregion
}
