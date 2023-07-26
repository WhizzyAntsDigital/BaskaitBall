using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DailyBonusLevelManager : MonoBehaviour
{
    public static DailyBonusLevelManager instance;
    [field: Header("Daily Bonus Level Controller")]
    [field: SerializeField] private GameObject DailyBonusLevelPanel;
    [field: SerializeField] private Button playButton;
    [field: SerializeField] private GameObject normalPlayText;
    [field: SerializeField] private GameObject adPlayText;
    [field: SerializeField] private string bonusLevelSceneName;
    [field: SerializeField] private TextMeshProUGUI timerText;
    [field: SerializeField] private MainMenuUIManager mainMenuUIManager;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CheckIfBonusLevelHasBeenPlayed();
        SetButtonValues();
    }
    private void Update()
    {
      CalculateTimeUntilNextDay();
    }
    public void PlayBonusLevel()
    {
        if (MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedDailyBonusLevel)
        {
            ADManager.Instance.ShowRewardedAd(TypeOfRewardedAD.BonusLevel);
        }
        else
        {
            MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedDailyBonusLevel = true;
            MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time_Bonus = GetInternetTime.Instance.GetCurrentDateTime().ToString();
            MiscellaneousDataHandler.instance.SaveMiscData();
            LoadBonusLevel();
        }
    }

    public void LoadBonusLevel()
    {
        SceneManager.LoadScene(bonusLevelSceneName);
    }

    private void CalculateTimeUntilNextDay()
    {
        DateTime currentTime = GetInternetTime.Instance.GetCurrentDateTime();
        DateTime nextDay = currentTime.AddDays(1).Date;
        TimeSpan timeSpan = nextDay - currentTime;
        timerText.text = "Resets in: " + timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
    }

    private void SetButtonValues()
    {
        if (MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedDailyBonusLevel)
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                playButton.interactable = true;
                normalPlayText.SetActive(false);
                adPlayText.SetActive(true);
            }
            else
            {
                playButton.interactable = true;
                normalPlayText.SetActive(true);
                normalPlayText.GetComponentInChildren<TextMeshProUGUI>().text = "No AD Available";
                adPlayText.SetActive(false);
            }
        }
        else
        {
            playButton.interactable = true;
            normalPlayText.SetActive(true);
            normalPlayText.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
            adPlayText.SetActive(false);
        }
    }

    private void CheckIfBonusLevelHasBeenPlayed()
    {
        if (!string.IsNullOrEmpty(MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time_Bonus))
        {
            DateTime lastClaim = DateTime.Parse(MiscellaneousDataHandler.instance.ReturnSavedValues().Date_And_Time_Bonus);
            if (lastClaim.Date < GetInternetTime.Instance.GetCurrentDateTime().Date)
            {
                DailyBonusLevelPanel.SetActive(true);
                mainMenuUIManager.isOpen = true;
                MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedDailyBonusLevel = false;
            }
            else
            {
                mainMenuUIManager.isOpen = false;
                DailyBonusLevelPanel.SetActive(false);
            }
        }
        else
        {
            mainMenuUIManager.isOpen = true;
            DailyBonusLevelPanel.SetActive(true);
            MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedDailyBonusLevel = false;
        }
    }
}
