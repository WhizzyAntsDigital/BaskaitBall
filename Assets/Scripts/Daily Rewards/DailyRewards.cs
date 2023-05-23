using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DailyRewards : MonoBehaviour
{
    [field: Header("Daily Rewards Manager")]
    [field: SerializeField] private GameObject dailyRewardPanel;
    [field: SerializeField] private TextMeshProUGUI dailyRewardAmountText;
    [field: SerializeField] private int dailyRewardAmount = 500;
    private void Start()
    {
        dailyRewardAmountText.text = dailyRewardAmount.ToString();
        InternetConnectivityChecker.Instance.IsConnectedToInternet += () => { CheckIfDailyRewardClaimed(); };
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { dailyRewardPanel.SetActive(false); };
        if (InternetConnectivityChecker.Instance.CheckForInternetConnectionUponCommand())
        {
            dailyRewardPanel.SetActive(true);
            CheckIfDailyRewardClaimed();
        }
        else
        {
            dailyRewardPanel.SetActive(false);
        }
    }

    public void CheckIfDailyRewardClaimed()
    {
        if (!string.IsNullOrEmpty(UserDataHandler.instance.ReturnSavedValues().Date_And_Time))
        {
            DateTime lastClaim = DateTime.Parse(UserDataHandler.instance.ReturnSavedValues().Date_And_Time);
            if (lastClaim.Date != GetInternetTime.Instance.GetCurrentDateTime().Date)
            {
                dailyRewardPanel.SetActive(true);
            }
            else
            {
                dailyRewardPanel.SetActive(false);
            }
        }
    }    

    public void OnClaiming()
    {
        CurrencyManager.instance.AdjustCurrency(dailyRewardAmount);
        UserDataHandler.instance.ReturnSavedValues().Date_And_Time = null;
        UserDataHandler.instance.ReturnSavedValues().Date_And_Time = GetInternetTime.Instance.GetCurrentDateTime().ToString();
        UserDataHandler.instance.SaveUserData();
        CheckIfDailyRewardClaimed();
    }
}