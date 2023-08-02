using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrencyDataHandler : MonoBehaviour
{
    public static CurrencyDataHandler instance;
    [SerializeField] private CurrencyData currencyData;
    private void Awake()
    {
        instance = this;

        currencyData = SaveLoadManager.LoadData<CurrencyData>();
        if (currencyData == null)
        {
            currencyData = new CurrencyData();
        }
    }
    public void AddValuesInStarting()
    {
        if (currencyData.hasAddedInitialDataToLeaderBoard == false)
        {
            LeaderboardManager.Instance.AddScore(currencyData.amountOfCoins, TypeOfLeaderBoard.DailyLeaderboard);
            LeaderboardManager.Instance.AddScore(currencyData.amountOfCoins, TypeOfLeaderBoard.WeeklyLeaderboard);
            LeaderboardManager.Instance.AddScore(currencyData.amountOfCoins, TypeOfLeaderBoard.MonthlyLeaderboard);
            AuthenticationService.Instance.UpdatePlayerNameAsync(Social.localUser.userName);
            currencyData.hasAddedInitialDataToLeaderBoard = true;
        }
    }

    #region Return All Data
    public CurrencyData ReturnSavedValues()
    {
        return currencyData;
    }
    #endregion

    #region Saving
    public void SaveCurrencyData()
    {
        SaveLoadManager.SaveData(currencyData);
    }
    private void OnDisable()
    {
        SaveCurrencyData();
    }
    #endregion
}

[System.Serializable]
public class CurrencyData
{
    public int amountOfCoins = 1000;
    public int amountOfGems = 10;
    public bool hasAddedInitialDataToLeaderBoard = false;
}