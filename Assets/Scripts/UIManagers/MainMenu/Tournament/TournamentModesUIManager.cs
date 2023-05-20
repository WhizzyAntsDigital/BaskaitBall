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
        TournamentInfoDataHandler.instance.SaveTourneyData();
    }
    public void AssignPrices()
    {
        for (int i = 0; i < TournamentInfoDataHandler.instance.ReturnSavedValues().prices.Length - 1; i++)
        {
            tourneyPriceTexts[i].text = TournamentInfoDataHandler.instance.ReturnSavedValues().prices[i].ToString();
        }
    }
}
