using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UserDataHandler : MonoBehaviour
{
    public static UserDataHandler instance;
    [SerializeField] private UserData userData;
    [SerializeField] private string mainGameModeSceneName = "TournamentMode";
    private DateTime closingTime;
    private DateTime openingTime;

    private void Awake()
    {
        instance = this;
        userData = SaveLoadManager.LoadData<UserData>();

        if (userData == null)
        {
            userData = new UserData();
        }
    }
    private void Start()
    {
        if (userData.hasRequestedReview == false && userData.numberOfWins != 0 && userData.numberOfWins % 5 == 0  )
        {
            IARManager.instance.TryLoadAndShowReviewRequest();
            userData.hasRequestedReview = true;
        }
    }

    public void UpdateReviewRequested()
    {
        userData.hasRequestedReview = true;
    }

    #region Check Application Loses Focus
#if !UNITY_EDITOR
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false && SceneManager.GetActiveScene().name == mainGameModeSceneName && !GameManager.instance.isGameOver)
        {
            closingTime = System.DateTime.Now;
        }
        if (focus == true && SceneManager.GetActiveScene().name == mainGameModeSceneName && closingTime != null)
        {
            openingTime = System.DateTime.Now;
            TimeSpan timediff = openingTime - closingTime;
            if (timediff.TotalSeconds < MainGameFlow.Instance.matchLength)
            {
                MainGameFlow.Instance.matchLength -= (float)timediff.TotalSeconds;
                AIScore.instance.opponentScore += UnityEngine.Random.Range(0, 30);
            }
            else if (timediff.TotalSeconds >= MainGameFlow.Instance.matchLength)
            {
                MainGameFlow.Instance.matchLength = 0;
                MainGameFlow.Instance.timerText.text = "END";
                AIScore.instance.opponentScore += UnityEngine.Random.Range(0, 30);
                GameManager.instance.OnGameOver?.Invoke();
            }
        }
    }
#endif
    #endregion

    #region Return All Data
    public UserData ReturnSavedValues()
    {
        return userData;
    }
    #endregion

    #region Saving
    public void SaveUserData()
    {
        SaveLoadManager.SaveData(userData);
    }
    private void OnDisable()
    {
        SaveUserData();
    }
    #endregion
}

[System.Serializable]
public class UserData
{
    public string userName = null;
    public bool hasClaimedDailyRewardToday = false;
    public int practiceHighScore = 0;
    public int numberOfWins = 0;
    public int numberOfLosses = 0;
    public int numberOfBaskets = 0;
    public int numberOf3Pointers = 0;
    public int winningStreak = 0;
    public int losingStreak = 0;
    public int amountOfCurrency = 1000;
    public string Date_And_Time = null;
    public bool hasRequestedReview = false;
    public bool hasComeFromMainGame = false;
    public int playerRankPoints = 0;
    public bool hasPlayedTutorial = false;
}
