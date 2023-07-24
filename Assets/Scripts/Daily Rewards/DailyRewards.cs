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
    private void OnEnable()
    {
        InternetConnectivityChecker.Instance.IsConnectedToInternet += () => { CheckIfDailyRewardClaimed(); };
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { dailyRewardPanel.SetActive(false); };
    }
    private void OnDisable()
    {
        InternetConnectivityChecker.Instance.IsConnectedToInternet -= () => { CheckIfDailyRewardClaimed(); };
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet -= () => { dailyRewardPanel.SetActive(false); };
    }

    public void CheckIfDailyRewardClaimed()
    {
        if (!string.IsNullOrEmpty(MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time))
        {
            DateTime lastClaim = DateTime.Parse(MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time);
            if (lastClaim.Date != GetInternetTime.Instance.GetCurrentDateTime().Date)
            {
                dailyRewardPanel.SetActive(true);
            }
            else
            {
                dailyRewardPanel.SetActive(false);
            }
        }
        else
        {
            dailyRewardPanel.SetActive(true);
        }
    }    

    public void OnClaiming()
    {
        CurrencyManager.instance.AdjustCurrency(dailyRewardAmount);
        MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time = null;
        MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time = GetInternetTime.Instance.GetCurrentDateTime().ToString();
        MiscellaneousDataHandler.instance.SaveMiscData();
        CheckIfDailyRewardClaimed();
    }
}