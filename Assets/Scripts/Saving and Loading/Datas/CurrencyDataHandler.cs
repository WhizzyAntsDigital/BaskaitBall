using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CurrencyDataHandler : MonoBehaviour
{
    public static CurrencyDataHandler instance;
    [SerializeField] private CurrencyData currencyData;
    public Image profilePic;
    public Image profileProfilePic;
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
            string usernameForPlayer = AINamesGenerator.Utils.GetRandomName();
            currencyData.playerUsername = usernameForPlayer;
            SaveCurrencyData();
            AuthenticationService.Instance.UpdatePlayerNameAsync(usernameForPlayer);
            GetImage.Instance.StartImageDownload(profilePic, true);
            //AssignImg(profilePic, true);
            //StartCoroutine(KeepCheckingAvatar());
            currencyData.hasAddedInitialDataToLeaderBoard = true;
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                AssignImg(profilePic, true);
                AssignImg(profileProfilePic, true);
            }
        }
    }

    private IEnumerator KeepCheckingAvatar()
    {
        float secondsOfTrying = 20;
        float secondsPerAttempt = 0.2f;
        while (secondsOfTrying > 0)
        {
            if (Social.localUser.image != null)
            {
                //imageTemp = Sprite.Create(Social.localUser.image, new Rect(0, 0, Social.localUser.image.width, Social.localUser.image.height), new Vector2(0.5f, 0.5f));
                currencyData.playerPFP = EncodePreview(Social.localUser.image);
                GetImage.Instance.StartImageDownload(profilePic, true);
                AssignImg(profilePic, true);
                SaveCurrencyData();
                break;
            }

            secondsOfTrying -= secondsPerAttempt;
            yield return new WaitForSeconds(secondsPerAttempt);
        }
    }

    public void AssignImg(Image imagePfp, bool forPlayer)
    {
        if (!String.IsNullOrEmpty(currencyData.playerPFP) && forPlayer)
        {
            Texture2D tex = DecodePreview(currencyData.playerPFP);
            imagePfp.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        else if (!String.IsNullOrEmpty(currencyData.opponentPFP) && forPlayer == false)
        {
            Texture2D tex = DecodePreview(currencyData.opponentPFP);
            imagePfp.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

    }

    public string EncodePreview(Texture2D previewImage)
    {
        byte[] previewBytes = previewImage.EncodeToJPG();
        string previewBase64 = Convert.ToBase64String(previewBytes);
        return previewBase64;
    }

    private Texture2D DecodePreview(string previewBase64)
    {
        byte[] previewBytes = Convert.FromBase64String(previewBase64);

        Texture2D previewImage = new Texture2D(0, 0);
        if (ImageConversion.LoadImage(previewImage, previewBytes))
            return previewImage;
        else
            return null;
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
    public int amountOfCoins = 300;
    public int amountOfGems = 10;
    public bool hasAddedInitialDataToLeaderBoard = false;
    public string playerPFP;
    public string opponentPFP;
    public string opponentName;
    public string playerUsername;
}