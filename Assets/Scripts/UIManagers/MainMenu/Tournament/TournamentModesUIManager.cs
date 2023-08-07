using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class TournamentModesUIManager : MonoBehaviour
{
    [field: Header("Tournament Mode UI Handler")]
    [field: SerializeField] private List<TextMeshProUGUI> tourneyPriceTexts;
    [field: SerializeField] private List<TextMeshProUGUI> tourneyPrePriceTexts;
    [field: SerializeField] private List<TextMeshProUGUI> tourneyPrizeTexts;
    [field: SerializeField] private List<Button> tourneyButtons;
    [field: SerializeField] private List<Button> tourneyUnlockButtons;

    [field: Header("For Internet Connection")]
    [field: SerializeField] private GameObject tournamentModesPanel;
    [field: SerializeField] private MainMenuUIManager mainMenuUIManager;

    [field: Header("Swipe Input")]
    [field: SerializeField] private float minSwipeDistance = 50f;
    [field: SerializeField] private float swipeThreshold = 1f;
    [field: SerializeField] private List<GameObject> mainTourney;
    [field: SerializeField] private List<GameObject> prevTourney;
    [field: SerializeField] private List<GameObject> nextTourney;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private int currentTourney = 0;
    private int prevTourneyIndex = 3;
    private int nextTourneyIndex = 1;

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

    private void Update()
    {
        if (Input.touchCount > 0 && tournamentModesPanel.activeInHierarchy)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                CheckSwipe();
            }
        }
    }

    private void CheckSwipe()
    {
        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) >= minSwipeDistance)
        {
            float deltaX = fingerUpPosition.x - fingerDownPosition.x;
            float deltaY = fingerUpPosition.y - fingerDownPosition.y;

            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) * swipeThreshold)
            {
                if (deltaX > 0)
                {
                    //Right
                    ScrollThroughTourneys(false);
                }
                else
                {
                    //Left
                    ScrollThroughTourneys(true);
                }
            }
        }
    }

    private void ScrollThroughTourneys(bool isLeft)
    {
        int previousTourney = currentTourney;
        int previousPrevTourney = prevTourneyIndex;
        int previousNextTourney = nextTourneyIndex;
        if (isLeft == true)
        {
            currentTourney++;
            prevTourneyIndex++;
            nextTourneyIndex++;
            if (currentTourney > mainTourney.Count - 1)
            {
                currentTourney = 0;
            }
            if (prevTourneyIndex > prevTourney.Count - 1)
            {
                prevTourneyIndex = 0;
            }
            if (nextTourneyIndex > nextTourney.Count - 1)
            {
                nextTourneyIndex = 0;
            }
        }
        else if (isLeft == false)
        {
            currentTourney--;
            prevTourneyIndex--;
            nextTourneyIndex--;
            if (currentTourney < 0)
            {
                currentTourney = mainTourney.Count - 1;
            }
            if (prevTourneyIndex < 0)
            {
                prevTourneyIndex = prevTourney.Count - 1;
            }
            if (nextTourneyIndex < 0)
            {
                nextTourneyIndex = nextTourney.Count - 1;
            }
        }
        GoToNextTourn(previousTourney, previousPrevTourney, previousNextTourney);
    }

    private void GoToNextTourn(int prevTour, int prevPrevTour, int prevNextTour)
    {
        mainTourney[prevTour].SetActive(false);
        mainTourney[currentTourney].SetActive(true);

        nextTourney[prevNextTour].SetActive(false);
        nextTourney[nextTourneyIndex].SetActive(true);

        prevTourney[prevPrevTour].SetActive(false);
        prevTourney[prevTourneyIndex].SetActive(true);
    }

    public void SelectTournamentMode(int ID)
    {
        if (TournamentInfoDataHandler.instance.ReturnSavedValues().unlocked[ID])
        {
            TournamentInfoDataHandler.instance.ReturnSavedValues().selected[ID] = true;
            CurrencyManager.instance.AdjustCoins(-TournamentInfoDataHandler.instance.ReturnSavedValues().prices[ID]);
            TournamentInfoDataHandler.instance.SaveTourneyData();
            SceneManager.LoadScene("TournamentLoadingScene");
        }
        else
        {
            OnUnlockTournament(ID);
        }
    }
    public void AssignPrices()
    {
        for (int i = 0; i < TournamentInfoDataHandler.instance.ReturnSavedValues().prices.Length; i++)
        {
            if (TournamentInfoDataHandler.instance.ReturnSavedValues().unlocked[i] == true)
            {
                if (i != 0) //To Skip First Free Tournament
                {
                    TournamentInfoDataHandler.instance.allGameModesSpecs[i].lockedOverlay.SetActive(false);
                    TournamentInfoDataHandler.instance.allGameModesSpecs[i].unlockedOverlay.SetActive(true);
                }
                tourneyPrizeTexts[i].text = (TournamentInfoDataHandler.instance.ReturnSavedValues().prices[i] * 2).ToString();
                tourneyPriceTexts[i].text = TournamentInfoDataHandler.instance.ReturnSavedValues().prices[i].ToString();
                tourneyPrePriceTexts[i].text = "Entry Cost: ";
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
                TournamentInfoDataHandler.instance.allGameModesSpecs[i].unlockedOverlay.SetActive(false);
                TournamentInfoDataHandler.instance.allGameModesSpecs[i].lockedOverlay.SetActive(true);
                tourneyButtons[i].interactable = false;
                tourneyPriceTexts[i].text = TournamentInfoDataHandler.instance.allGameModesSpecs[i].tournamentUnlockCost.ToString();
                tourneyPrePriceTexts[i].text = "Unlock Cost: ";
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
