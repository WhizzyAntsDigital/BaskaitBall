using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TournamentModesUIManager : MonoBehaviour
{
    [field: Header("Tournament Mode UI Handler")]
    [field: SerializeField] private List<TextMeshProUGUI> tourneyPriceTexts;
    [field: SerializeField] private List<Button> tourneyButtons;
    [field: SerializeField] private List<Button> tourneyUnlockButtons;

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
        CurrencyManager.instance.AdjustCoins(-TournamentInfoDataHandler.instance.ReturnSavedValues().prices[ID]);
        TournamentInfoDataHandler.instance.SaveTourneyData();
    }
    public void AssignPrices()
    {
        for (int i = 0; i < TournamentInfoDataHandler.instance.ReturnSavedValues().prices.Length; i++)
        {
            if (TournamentInfoDataHandler.instance.ReturnSavedValues().unlocked[i] == true)
            {
                TournamentInfoDataHandler.instance.allGameModesSpecs[i].lockedOverlay.SetActive(false);
                tourneyPriceTexts[i].text = TournamentInfoDataHandler.instance.ReturnSavedValues().prices[i].ToString();
                if (CurrencyDataHandler.instance.ReturnSavedValues().amountOfCoins < TournamentInfoDataHandler.instance.ReturnSavedValues().prices[i])
                {
                    tourneyButtons[i].interactable = false;
                }
                else
                {
                    tourneyButtons[i].interactable = true;
                }
            }
            else if(TournamentInfoDataHandler.instance.ReturnSavedValues().unlocked[i] == false)
            {
                TournamentInfoDataHandler.instance.allGameModesSpecs[i].lockedOverlay.SetActive(true);
                tourneyButtons[i].interactable = false;
                tourneyPriceTexts[i].text = TournamentInfoDataHandler.instance.allGameModesSpecs[i].tournamentUnlockCost.ToString();
                if(CurrencyDataHandler.instance.ReturnSavedValues().amountOfCoins >= TournamentInfoDataHandler.instance.allGameModesSpecs[i].tournamentUnlockCost)
                {
                    tourneyUnlockButtons[i].interactable = true;
                }
                else
                {
                    tourneyUnlockButtons[i].interactable = false;
                }
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
    public void OnUnlockTournament(int ID)
    {
        CurrencyManager.instance.AdjustCoins(-TournamentInfoDataHandler.instance.allGameModesSpecs[ID].tournamentUnlockCost);
        TournamentInfoDataHandler.instance.ReturnSavedValues().unlocked[ID] = true;
        TournamentInfoDataHandler.instance.SaveTourneyData();
        TournamentInfoDataHandler.instance.allGameModesSpecs[ID].unlockAnimation.UnlockIcon();
        //AssignPrices();
    }    
}
