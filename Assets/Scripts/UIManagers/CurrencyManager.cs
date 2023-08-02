using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    [field: Header("Currency Manager")]
    [field: SerializeField] private List<TextMeshProUGUI> coinsText;
    [field: SerializeField] private List<TextMeshProUGUI> gemsText;

    [field: Header("Only For Main Menu")]
    [field: SerializeField] private TournamentModesUIManager tournamentModesUIManager;
    [field: SerializeField] private ShopManager shopManager;

    private void Awake()
    {
        instance = this;
    }
    public void UpdateCurrencysAmount()
    {
        for(int i = 0; i < coinsText.Count; i++)
        {
            coinsText[i].text = CurrencyDataHandler.instance.ReturnSavedValues().amountOfCoins.ToString();
        }
        for(int j = 0; j< gemsText.Count; j++)
        {
            gemsText[j].text = CurrencyDataHandler.instance.ReturnSavedValues().amountOfGems.ToString();
        }
    }
    
    public void AdjustCoins(int amount)
    {
        CurrencyDataHandler.instance.ReturnSavedValues().amountOfCoins += amount;
        CurrencyDataHandler.instance.SaveCurrencyData();
        if(amount > 0)
        {
            LeaderboardManager.Instance.AddScore(amount, TypeOfLeaderBoard.DailyLeaderboard);
            LeaderboardManager.Instance.AddScore(amount, TypeOfLeaderBoard.WeeklyLeaderboard);
            LeaderboardManager.Instance.AddScore(amount, TypeOfLeaderBoard.MonthlyLeaderboard);
            CurrencyDataHandler.instance.SaveCurrencyData();
        }
        UpdateOnMainMenu();
    }
    public void AdjustGems(int amount)
    {
        CurrencyDataHandler.instance.ReturnSavedValues().amountOfGems += amount;
        CurrencyDataHandler.instance.SaveCurrencyData();
        UpdateOnMainMenu();
    }
    private void UpdateOnMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            UpdateCurrencysAmount();
            tournamentModesUIManager.AssignPrices();
            shopManager.UpdateActionButton();
        }
    }
}
