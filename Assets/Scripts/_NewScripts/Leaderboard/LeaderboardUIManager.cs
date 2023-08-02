using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIManager : MonoBehaviour
{
    [field: Header("------ Active Tabs ------")]
    [field: SerializeField] private GameObject dailyActive;
    [field: SerializeField] private GameObject weeklyActive;
    [field: SerializeField] private GameObject monthlyActive;

    [field: Header("------ Inactive Tabs ------")]
    [field: SerializeField] private GameObject dailyInactive;
    [field: SerializeField] private GameObject weeklyInactive;
    [field: SerializeField] private GameObject monthlyInactive;

    [field: Header("------ Main Panels ------")]
    [field: SerializeField] private GameObject dailyLBPanel;
    [field: SerializeField] private GameObject weeklyLBPanel;
    [field: SerializeField] private GameObject monthlyLBPanel;

    [field: Header("------ Buttons ------")]
    [field: SerializeField] private Button dailyButton;
    [field: SerializeField] private Button weeklyButton;
    [field: SerializeField] private Button monthlyButton;

    private void OnEnable()
    {
        dailyButton.onClick.AddListener(SwitchToDailyLB);
        weeklyButton.onClick.AddListener(SwitchToWeeklyLB);
        monthlyButton.onClick.AddListener(SwitchToMonthlyLB);
    }

    private void OnDisable()
    {
        dailyButton.onClick.RemoveAllListeners();
        weeklyButton.onClick.RemoveAllListeners();
        monthlyButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        SwitchToDailyLB();
    }

    private void SwitchToDailyLB()
    {
        dailyActive.SetActive(true);
        weeklyActive.SetActive(false);
        monthlyActive.SetActive(false);

        dailyInactive.SetActive(false);
        weeklyInactive.SetActive(true);
        monthlyInactive.SetActive(true);

        dailyLBPanel.SetActive(true);
        weeklyLBPanel.SetActive(false);
        monthlyLBPanel.SetActive(false);

        dailyButton.interactable = false;
        weeklyButton.interactable = true;
        monthlyButton.interactable = true;
    }

    private void SwitchToWeeklyLB()
    {
        dailyActive.SetActive(false);
        weeklyActive.SetActive(true);
        monthlyActive.SetActive(false);

        dailyInactive.SetActive(true);
        weeklyInactive.SetActive(false);
        monthlyInactive.SetActive(true);

        dailyLBPanel.SetActive(false);
        weeklyLBPanel.SetActive(true);
        monthlyLBPanel.SetActive(false);

        dailyButton.interactable = true;
        weeklyButton.interactable = false;
        monthlyButton.interactable = true;
    }

    private void SwitchToMonthlyLB()
    {
        dailyActive.SetActive(false);
        weeklyActive.SetActive(false);
        monthlyActive.SetActive(true);

        dailyInactive.SetActive(true);
        weeklyInactive.SetActive(true);
        monthlyInactive.SetActive(false);

        dailyLBPanel.SetActive(false);
        weeklyLBPanel.SetActive(false);
        monthlyLBPanel.SetActive(true);

        dailyButton.interactable = true;
        weeklyButton.interactable = true;
        monthlyButton.interactable = false;
    }
}
