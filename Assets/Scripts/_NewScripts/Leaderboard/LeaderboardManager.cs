using System;
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

    string LeaderboardId = "bb_monthly";
    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; } = 2;
    List<string> FriendIds { get; set; }
    public List<PlayerInfo> players;
    LeaderboardEntry playerValues;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            leaderboardButton.interactable = false;
        
            AuthenticatorManager.Instance.OnLoggedInCompleted += () => { Instance = this; UnityServices.InitializeAsync(); leaderboardButton.interactable = true; CurrencyDataHandler.instance.AddValuesInStarting(); CheckAndResetValues(); };
        }
        else
        {
            Instance = this;
        }
        //CheckAndResetValues();
        //CheckIfGameGotUninstalled();
    }
    private async void FirstStartingStuff()
    {
        await Task.Delay(1000);
        CurrencyDataHandler.instance.AddValuesInStarting();
        CheckAndResetValues(); 
        CheckIfGameGotUninstalled();
        await Task.Delay(1000);
        PopulateLeaderboard(TypeOfLeaderBoard.DailyLeaderboard);
        PopulateLeaderboard(TypeOfLeaderBoard.WeeklyLeaderboard);
        PopulateLeaderboard(TypeOfLeaderBoard.MonthlyLeaderboard);
        await Task.Delay(2000);
        leaderboardButton.interactable = true;
    }
    public void PopLB(string typeOfLB)
    {
        TypeOfLeaderBoard lbType = (TypeOfLeaderBoard)Enum.Parse(typeof(TypeOfLeaderBoard), typeOfLB);
        PopulateLeaderboard(lbType);
    }
    public async void PopulateLeaderboard(TypeOfLeaderBoard typeOfLeaderboard)
    {
        players = new List<PlayerInfo>();

        switch (typeOfLeaderboard)
        {
            case TypeOfLeaderBoard.DailyLeaderboard: LeaderboardId = dailyLB; targetForInstantiating = dailyLBHolder; break;
            case TypeOfLeaderBoard.WeeklyLeaderboard: LeaderboardId = weeklyLB; targetForInstantiating = weeklyLBHolder; break;
            case TypeOfLeaderBoard.MonthlyLeaderboard: LeaderboardId = monthlyLB; targetForInstantiating = monthlyLBHolder; break;
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
        GetPlayerRange();
        await Task.Delay(1000);
        foreach (var player in players)
        {
            var playerThing = Instantiate(leaderboardPlayerInfoPrefab);
            playerThing.transform.SetParent(targetForInstantiating.transform, false);
            Sprite pfp;
            if(player.playerId == Social.localUser.id)
            {
                pfp = Sprite.Create(Social.localUser.image, new Rect(0, 0, Social.localUser.image.width, Social.localUser.image.height), Vector2.zero);
                HelperClass.DebugMessage(pfp == null);
            }
            else
            {
                pfp = null;
            }
            playerThing.GetComponent<AssignLBValues>().AssignValues(player.playerName, (int)player.score, player.rank+=1, pfp);
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
        var scoreResponse =
        await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoreResponse));
        return JsonUtility.FromJson<LeaderboardEntry>(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetPlayerRange()
    {
        LeaderboardScores scoresResponse =
            await LeaderboardsService.Instance.GetPlayerRangeAsync(LeaderboardId, new GetPlayerRangeOptions { RangeLimit = 100 });
        HelperClass.DebugMessage(JsonConvert.SerializeObject(scoresResponse));
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

    private void CheckAndResetValues()
    {
        LeaderboardId = dailyLB;
        if (GetPlayerScore().Result.Score == 0)
        {
            CurrencyDataHandler.instance.ReturnSavedValues().dailyLeaderboard = 0;
            AddScore(0, TypeOfLeaderBoard.DailyLeaderboard);
        }

        LeaderboardId = weeklyLB;
        if (GetPlayerScore().Result.Score == 0)
        {
            CurrencyDataHandler.instance.ReturnSavedValues().weeklyLeaderboard = 0;
            AddScore(0, TypeOfLeaderBoard.WeeklyLeaderboard);
        }

        LeaderboardId = monthlyLB;
        if (GetPlayerScore().Result.Score == 0)
        {
            CurrencyDataHandler.instance.ReturnSavedValues().monthlyLeaderboard = 0;
            AddScore(0, TypeOfLeaderBoard.MonthlyLeaderboard);
        }

        CurrencyDataHandler.instance.SaveCurrencyData();
    }

    private void CheckIfGameGotUninstalled()
    {
        LeaderboardId = dailyLB;
        if(CurrencyDataHandler.instance.ReturnSavedValues().dailyLeaderboard == 0 && GetPlayerScore().Result.Score != 0)
        {
            CurrencyDataHandler.instance.ReturnSavedValues().dailyLeaderboard = (int)GetPlayerScore().Result.Score;
        }

        LeaderboardId = weeklyLB;
        if (CurrencyDataHandler.instance.ReturnSavedValues().weeklyLeaderboard == 0 && GetPlayerScore().Result.Score != 0)
        {
            CurrencyDataHandler.instance.ReturnSavedValues().weeklyLeaderboard = (int)GetPlayerScore().Result.Score;
        }

        LeaderboardId = monthlyLB;
        if (CurrencyDataHandler.instance.ReturnSavedValues().monthlyLeaderboard == 0 && GetPlayerScore().Result.Score != 0)
        {
            CurrencyDataHandler.instance.ReturnSavedValues().monthlyLeaderboard = (int)GetPlayerScore().Result.Score;
        }

        CurrencyDataHandler.instance.SaveCurrencyData();
    }
}
