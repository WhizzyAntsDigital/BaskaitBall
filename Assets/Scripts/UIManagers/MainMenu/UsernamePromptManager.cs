using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UsernamePromptManager : MonoBehaviour
{
    [field: Header("Username Pop Up Manager")]
    [field: SerializeField] private MainMenuUIManager mainMenuUIManager;
    [field: SerializeField] private Button confirmButton;
    [field: SerializeField] private TMP_InputField inputField;
    [field: SerializeField] private DailyBonusLevelManager dailyBonusLevelManager;
    void Start()
    {
        mainMenuUIManager = GetComponent<MainMenuUIManager>();
        confirmButton.interactable = false;
        if (string.IsNullOrEmpty(UserDataHandler.instance.ReturnSavedValues().userName))
        {
            dailyBonusLevelManager.enabled = false;
            mainMenuUIManager.UserNamePromptUIControl();
        }
        else
        {
            dailyBonusLevelManager.enabled = true;
            mainMenuUIManager.isOpen = true;
            mainMenuUIManager.UserNamePromptUIControl();
        }
    }

    public void EnableConfirmButton()
    {
        if(!string.IsNullOrWhiteSpace(inputField.text))
        {
            confirmButton.interactable = true;
        }
        else
        {
            confirmButton.interactable = false;
        }
    }
    public void OnConfirm()
    {
        dailyBonusLevelManager.enabled = true;
        UserDataHandler.instance.ReturnSavedValues().userName = inputField.text;
        UserDataHandler.instance.SaveUserData();
        mainMenuUIManager.UserNamePromptUIControl();
    }
}
