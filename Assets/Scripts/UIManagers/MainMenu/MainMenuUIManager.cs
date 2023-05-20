using System.Collections;
using System.Collections.Generic;
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

    [field: Header("Script References")]
    [field: SerializeField] private ProfileStatsManager profileStatsManager;
    [field: SerializeField] private TournamentModesUIManager tournamentModesUIManager;

    public bool isOpen = false;
    private void Start()
    {
        profileStatsManager = GetComponent<ProfileStatsManager>();
        tournamentModesUIManager = GetComponent<TournamentModesUIManager>();

        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        profileCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        tournamentModesCanvas.SetActive(false);

        
    }
    public void SettingsUIControl()
    {
        if(!isOpen)
        {
            settingsCanvas.SetActive(!isOpen);
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
            isOpen = true;
        }
        else if (isOpen)
        {
            shopCanvas.SetActive(!isOpen);
            isOpen = false;
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
}
