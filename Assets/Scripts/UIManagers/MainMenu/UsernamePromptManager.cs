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
    void Start()
    {
        mainMenuUIManager = GetComponent<MainMenuUIManager>();
        confirmButton.interactable = false;
        if (string.IsNullOrEmpty(UserDataHandler.instance.ReturnSavedValues().userName))
        {
            mainMenuUIManager.UserNamePromptUIControl();
        }
        else
        {
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
        UserDataHandler.instance.ReturnSavedValues().userName = inputField.text;
        UserDataHandler.instance.SaveUserData();
        mainMenuUIManager.UserNamePromptUIControl();
    }
}
