using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [field: Header("Settings Manager")]
    [field: SerializeField] private GameObject settingsPanel;
    [field: SerializeField] private AudioSource musicAudioSource;
    [field: SerializeField] private AudioSource SoundAudioSource;
    [field: SerializeField] private Slider musicSlider;
    [field: SerializeField] private Slider soundSlider;

    [field: SerializeField] private Button settingsExitButton;
    [field: SerializeField] private GameObject restorePurchasesPanel;
    [field: SerializeField] private GameObject spotLight;
    [field: SerializeField] private MainMenuUIManager mainMenuUIManager;

    [field: Header("Vibration Icons")]
    [field: SerializeField] private GameObject onActive;
    [field: SerializeField] private GameObject onInactive;
    [field: SerializeField] private GameObject offActive;
    [field: SerializeField] private GameObject offInactive;
    [field: SerializeField] private Button onButton;
    [field: SerializeField] private Button offButton;


    private void Start()
    {
        mainMenuUIManager = GetComponent<MainMenuUIManager>();
        restorePurchasesPanel.SetActive(false);
        OnStartValues();
    }
    public void UpdateIcons()
    {
        onActive.SetActive(!SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled);
        onInactive.SetActive(SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled);

        offActive.SetActive(SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled);
        offInactive.SetActive(!SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled);

        onButton.interactable = SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled;
        offButton.interactable = !SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled;

    }
    public void UpdateAudioSources(int index)
    {
        if(index == 0) 
        { 
        SettingsDataHandler.instance.ReturnSavedValues().musicAmount = musicSlider.value;
        musicAudioSource.volume = musicSlider.value;
        }
        else
        {
        SettingsDataHandler.instance.ReturnSavedValues().soundAmount = soundSlider.value;
        SoundAudioSource.volume = soundSlider.value;
        }
        SettingsDataHandler.instance.SaveSettingsData();
        UpdateIcons();
    }

    private void OnStartValues()
    {
        musicSlider.value = SettingsDataHandler.instance.ReturnSavedValues().musicAmount;
        soundSlider.value = SettingsDataHandler.instance.ReturnSavedValues().soundAmount;
        musicAudioSource.volume = SettingsDataHandler.instance.ReturnSavedValues().musicAmount;
        SoundAudioSource.volume = SettingsDataHandler.instance.ReturnSavedValues().soundAmount;
    }

    public void OnVibrationButtonClick()
    {
        SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled = !SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled;
        if(!SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled)
        {
            Handheld.Vibrate();
        }
        SettingsDataHandler.instance.SaveSettingsData();
        UpdateIcons();
    }
    public void OnRestoreClicked()
    {
        settingsExitButton.interactable = false;
    }

    public void OnRestoreSuccessful()
    {
        settingsPanel.SetActive(false);
        mainMenuUIManager.isOpen = false;
        restorePurchasesPanel.SetActive(true);
        settingsExitButton.interactable = true;
    }

    public void ExitRestorePurchases()
    {
        restorePurchasesPanel.SetActive(false);
    }
}
