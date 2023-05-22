using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    [field: Header("Currency Manager")]
    [field: SerializeField] private List<TextMeshProUGUI> coinsText;

    [field: Header("Only For Main Menu")]
    [field: SerializeField] private TournamentModesUIManager tournamentModesUIManager;
    [field: SerializeField] private ShopManager shopManager;

    private void Awake()
    {
        instance = this;
    }
    public void UpdateCoinsAmount()
    {
        for(int i = 0; i < coinsText.Count; i++)
        {
            coinsText[i].text = UserDataHandler.instance.ReturnSavedValues().amountOfCurrency.ToString();
        }
    }
    
    public void AdjustCurrency(int amount)
    {
        UserDataHandler.instance.ReturnSavedValues().amountOfCurrency += amount;
        UserDataHandler.instance.SaveUserData();
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            UpdateCoinsAmount();
            tournamentModesUIManager.AssignPrices();
            shopManager.UpdateActionButton();
        }
    }
}
