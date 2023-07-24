using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [field: Header("Main Menu UI Manager")]
    [field: SerializeField] private GameObject mainMenuCanvas;
    [field: SerializeField] private GameObject settingsCanvas;
    [field: SerializeField] private GameObject profileCanvas;
    [field: SerializeField] private GameObject shopCanvas;
    [field: SerializeField] private GameObject tournamentModesCanvas;
    [field: SerializeField] private GameObject usernamePromptCanvas;
    [field: SerializeField] private AudioSource audioSource;
    private bool isAudioMuted = false;

    [field: Header("Script References")]
    [field: SerializeField] private ProfileStatsManager profileStatsManager;
    [field: SerializeField] private TournamentModesUIManager tournamentModesUIManager;
    [field: SerializeField] private SettingsManager settingsManager;


    [field: Header("For Internet Connection")]
    [field: SerializeField] private Button tournamentButton;
    [field: SerializeField] private Button noADsButton;

    [field: HideInInspector] public bool isOpen = false;
    private void Start()
    {
        Time.timeScale = 1.0f;
        profileStatsManager = GetComponent<ProfileStatsManager>();
        tournamentModesUIManager = GetComponent<TournamentModesUIManager>();
        settingsManager = GetComponent<SettingsManager>();
        CurrencyManager.instance.UpdateCoinsAmount();
        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        profileCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        tournamentModesCanvas.SetActive(false);

        if (!InternetConnectivityChecker.Instance.CheckForInternetConnectionUponCommand())
        {
            tournamentButton.interactable = false;
            noADsButton.interactable = false;
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
        InternetConnectivityChecker.Instance.IsConnectedToInternet += () => { OnInternetConnectionChange(true); };
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { OnInternetConnectionChange(false); };
    }
    private void OnDisable()
    {
        InternetConnectivityChecker.Instance.IsConnectedToInternet -= () => { OnInternetConnectionChange(true); };
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
        tournamentButton.interactable = connected;
        noADsButton.interactable = connected;
    }
}
