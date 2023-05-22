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
}
