using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class TournamentModesUIManager : MonoBehaviour
{
    [field: Header("Tournament Mode UI Handler")]
    [field: SerializeField] private List<TextMeshProUGUI> tourneyPriceTexts;
    [field: SerializeField] private List<Button> tourneyButtons;

    [field: Header("For Internet Connection")]
    [field: SerializeField] private GameObject tournamentModesPanel;
    [field: SerializeField] private MainMenuUIManager mainMenuUIManager;
    private void Start()
    {
        mainMenuUIManager = GetComponent<MainMenuUIManager>();  
    }
    private void OnEnable()
    {
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { OnInternetConnectionChange(false); };
    }
    private void OnDisable()
    {
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet -= () => { OnInternetConnectionChange(false); };
    }
    public void SelectTournamentMode(int ID)
    {
        TournamentInfoDataHandler.instance.ReturnSavedValues().selected[ID] = true;
        CurrencyManager.instance.AdjustCurrency(-TournamentInfoDataHandler.instance.ReturnSavedValues().prices[ID]);
        TournamentInfoDataHandler.instance.SaveTourneyData();
    }
    public void AssignPrices()
    {
        for (int i = 0; i < TournamentInfoDataHandler.instance.ReturnSavedValues().prices.Length; i++)
        {
            tourneyPriceTexts[i].text = TournamentInfoDataHandler.instance.ReturnSavedValues().prices[i].ToString();
            if(UserDataHandler.instance.ReturnSavedValues().amountOfCurrency < TournamentInfoDataHandler.instance.ReturnSavedValues().prices[i])
            {
                tourneyButtons[i].interactable = false;
            }
            else
            {
                tourneyButtons[i].interactable = true;
            }
        }
    }
    private void OnInternetConnectionChange(bool connected)
    {
        if(tournamentModesPanel.activeInHierarchy)
        {
            tournamentModesPanel.SetActive(false);
            mainMenuUIManager.isOpen = false;
        }
    }
}
