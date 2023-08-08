using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [field: Header("Main Menu UI Manager")]
    [field: SerializeField] private GameObject loadingScreenCanvas;

    [field: SerializeField] private GameObject mainMenuCanvas;
    [field: SerializeField] private GameObject settingsCanvas;
    [field: SerializeField] private GameObject profileCanvas;
    [field: SerializeField] private GameObject shopCanvas;
    [field: SerializeField] private GameObject tournamentModesCanvas;
    [field: SerializeField] private GameObject usernamePromptCanvas;
    [field: SerializeField] private GameObject missionsCanvas;
    [field: SerializeField] private GameObject bonusLevelCanvas;
    [field: SerializeField] private GameObject leaderboardCanvas;
    [field: SerializeField] private AudioSource audioSource;
    private bool isAudioMuted = false;

    [field: Header("Script References")]
    [field: SerializeField] private ProfileStatsManager profileStatsManager;
    [field: SerializeField] private TournamentModesUIManager tournamentModesUIManager;
    [field: SerializeField] private SettingsManager settingsManager;
    [field: SerializeField] private ShopManager shopManager;

    [field: Header("For Internet Connection")]
    [field: SerializeField] private GameObject noInternetPanel;

    [field: HideInInspector] public bool isOpen = false;
    private void Start()
    {
        Time.timeScale = 1.0f;
        profileStatsManager = GetComponent<ProfileStatsManager>();
        tournamentModesUIManager = GetComponent<TournamentModesUIManager>();
        settingsManager = GetComponent<SettingsManager>();
        shopManager = GetComponent<ShopManager>();
        CurrencyManager.instance.UpdateCurrencysAmount();
        loadingScreenCanvas.SetActive(true); ;
        mainMenuCanvas.SetActive(true); ;
        settingsCanvas.SetActive(false);
        profileCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        tournamentModesCanvas.SetActive(false);
        missionsCanvas.SetActive(false);
        leaderboardCanvas.SetActive(false);

        if (!InternetConnectivityChecker.Instance.CheckForInternetConnectionUponCommand())
        {
            Time.timeScale = 0;
            noInternetPanel.SetActive(true);
        }
        else
        {
            noInternetPanel.SetActive(false);
        }

        if (MiscellaneousDataHandler.instance.ReturnSavedValues().hasComeFromMainGame)
        {
            ADManager.Instance.ShowInterstitialAd();
            MiscellaneousDataHandler.instance.ReturnSavedValues().hasComeFromMainGame = false;
            MiscellaneousDataHandler.instance.SaveMiscData();
        }
    }
    private void OnEnable()
    {
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { OnInternetConnectionChange(false); };
    }
    private void OnDisable()
    {
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet -= () => { OnInternetConnectionChange(false); };
    }

    public void SettingsUIControl()
    {
        if(!isOpen)
        {
            settingsCanvas.SetActive(!isOpen);
            settingsManager.UpdateIcons();
            isOpen = true;
        }
        else if(isOpen)
        {
            settingsCanvas.SetActive(!isOpen);
            isOpen = false;
        }
    }

    public void DailyMissionsUIControl()
    {
        if (!isOpen)
        {
            missionsCanvas.SetActive(!isOpen);
            isOpen = true;
        }
        else if (isOpen)
        {
            missionsCanvas.SetActive(!isOpen);
            isOpen = false;
        }
    }

    public void LeaderboardUIControl()
    {
        if (!isOpen)
        {
            leaderboardCanvas.SetActive(!isOpen);
            isOpen = true;
        }
        else if (isOpen)
        {
            leaderboardCanvas.SetActive(!isOpen);
            isOpen = false;
        }
    }

    public void BonusLevelUIControl()
    {
        if (!isOpen)
        {
            bonusLevelCanvas.SetActive(!isOpen);
            isOpen = true;
        }
        else if (isOpen)
        {
            bonusLevelCanvas.SetActive(!isOpen);
            isOpen = false;
        }
    }

    public void ProfileUIControl()
    {
        if (!isOpen)
        {
            profileStatsManager.SetPlayerStats();
            profileCanvas.SetActive(!isOpen);
            isOpen = true;
        }
        else if (isOpen)
        {
            profileCanvas.SetActive(!isOpen);
            isOpen = false;
        }
    }

    public void ShopUIControl()
    {
        if (!isOpen)
        {
            shopCanvas.SetActive(!isOpen);
            mainMenuCanvas.SetActive(isOpen);
            shopManager.AssignPrices();
            shopManager.UpdateActionButton();
            isOpen = true;
        }
        else if (isOpen)
        {
            shopCanvas.SetActive(!isOpen);
            mainMenuCanvas.SetActive(isOpen);
            isOpen = false;
            SkinsOwnershipDataHandler.instance.SaveSkinData();
        }
    }

    public void TournamentModesUIControl()
    {
        if (!isOpen)
        {
            tournamentModesUIManager.AssignPrices();
            tournamentModesCanvas.SetActive(!isOpen);
            isOpen = true;
        }
        else if (isOpen)
        {
            tournamentModesCanvas.SetActive(!isOpen);
            isOpen = false;
        }
    }

    public void UserNamePromptUIControl()
    {
        if (!isOpen)
        {
            usernamePromptCanvas.SetActive(!isOpen);
            isOpen = true;
        }
        else if (isOpen)
        {

            usernamePromptCanvas.SetActive(!isOpen);
            isOpen = false;
        }
    }

    public void OpenLink(string urlLink)
    {
            Application.OpenURL(urlLink);
    }

    public void EnableAudio()
    {
        if(isAudioMuted)
        {
            audioSource.Play();
            isAudioMuted = false;
        }
        else
        {
            audioSource.Stop();
            isAudioMuted = true;
        }
    }
    private void OnInternetConnectionChange(bool connected)
    {
        Time.timeScale = 0;
        noInternetPanel.SetActive(true);
    }
}
