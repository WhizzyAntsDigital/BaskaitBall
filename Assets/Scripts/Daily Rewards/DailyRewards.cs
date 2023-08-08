using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DailyRewards : MonoBehaviour
{
    [field: Header("Daily Rewards Manager")]
    [field: SerializeField] private Button dailyRewardButton;
    //[field: SerializeField] private TextMeshProUGUI dailyRewardAmountText;
    [field: SerializeField] private int dailyRewardAmount = 500;
    private void Start()
    {
        //dailyRewardAmountText.text = dailyRewardAmount.ToString();
       
        if (InternetConnectivityChecker.Instance.CheckForInternetConnectionUponCommand())
        {
            dailyRewardButton.interactable = true;
            CheckIfDailyRewardClaimed();
        }
        else
        {
            dailyRewardButton.interactable = false;
        }
    }
    private void OnEnable()
    {
        InternetConnectivityChecker.Instance.IsConnectedToInternet += () => { CheckIfDailyRewardClaimed(); };
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { dailyRewardButton.interactable = false; };
    }
    private void OnDisable()
    {
        InternetConnectivityChecker.Instance.IsConnectedToInternet -= () => { CheckIfDailyRewardClaimed(); };
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet -= () => { dailyRewardButton.interactable = true; };
    }

    public void CheckIfDailyRewardClaimed()
    {
        if (!string.IsNullOrEmpty(MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time_DailyReward))
        {
            DateTime lastClaim = DateTime.Parse(MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time_DailyReward);
            if (lastClaim.Date != GetInternetTime.Instance.GetCurrentDateTime().Date)
            {
                dailyRewardButton.interactable = true;
            }
            else
            {
                dailyRewardButton.interactable = false;
            }
        }
        else
        {
            dailyRewardButton.interactable = true;
        }
    }    

    public void OnClaiming()
    {
        CurrencyManager.instance.AdjustCoins(dailyRewardAmount);
        MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time_DailyReward = null;
        MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time_DailyReward = GetInternetTime.Instance.GetCurrentDateTime().ToString();
        MiscellaneousDataHandler.instance.SaveMiscData();
        CheckIfDailyRewardClaimed();
    }
}