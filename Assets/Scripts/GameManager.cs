using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: Header("General")]
    [field: SerializeField] private int targetFPS = 60;
    [field: SerializeField] public bool timerNeeded { get; private set; } = true;

    [field: Header("For BallInput Script")]
    [field: SerializeField] public GameObject ringObj { get; private set; } //Used as reference to calculate force for ball by BallInput Script

    [field: Header("Internet Connection Checking")]
    [field: SerializeField] private float internetConnectionCheckInterval = 3f;
    [field: SerializeField] private bool shouldCheckForInternetConnection = true;
    public Action IsConnectedToInternet, IsDisconnectedFromInternet;
    private bool whenConnectedActionsCarriedOut = false;
    private bool whenDisconnectedActionsCarriedOut = false;

    [field: Header("Main Game Flow")]
    [field: SerializeField] private float matchLength = 30f;
    [field: SerializeField] private TextMeshProUGUI timerText;
    [field: SerializeField] private BallInput startingBall;
    [field: SerializeField] public bool isGameOver {get; private set;} = false;
    public Action onGameOver;
    private float countDown = 3f;
    private bool startCountDown = false;
    private bool startMatchTimer = false;
    bool changedStartingBallValue = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Application.targetFrameRate = targetFPS;
        StartCoroutine(CheckInternetConnection());
        if (timerNeeded)
        {
            startCountDown = true;
            startingBall.hasGotInput = true;
        }
        onGameOver += () => { isGameOver = true; };
    }

    private void Update()
    {
        if(startCountDown)
        {
            countDown -= Time.deltaTime;
            if(countDown >=1)
            {
                timerText.text = (Mathf.RoundToInt(countDown)).ToString();
            }
            if(countDown <1 && countDown >= 0)
            {
                timerText.text = "START!";
                AIScore.instance.OnGameStart?.Invoke();
                if (!changedStartingBallValue)
                {
                    startingBall.hasGotInput = false;
                    changedStartingBallValue = true;
                }
            }
            else if(countDown <-1)
            {
                startCountDown = false;
                startMatchTimer = true;
            }
        }
        if(startMatchTimer)
        {
            matchLength -= Time.deltaTime;
            if (matchLength > 0)
            {
                timerText.text = Mathf.RoundToInt(matchLength).ToString();
            }
            if(matchLength <= 0)
            {
                timerText.text = "END";
                onGameOver?.Invoke();
                startMatchTimer =false;
            }
        }
    }

    private IEnumerator CheckInternetConnection()
    {
        while (shouldCheckForInternetConnection)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if(!whenDisconnectedActionsCarriedOut)
                {
                    IsDisconnectedFromInternet?.Invoke();
                    whenDisconnectedActionsCarriedOut = true;
                    whenConnectedActionsCarriedOut = false;
                }
            }
            else
            {
                if(!whenConnectedActionsCarriedOut)
                {
                    IsConnectedToInternet?.Invoke();
                    whenConnectedActionsCarriedOut = true;
                    whenDisconnectedActionsCarriedOut = false;
                }
            }
            yield return new WaitForSeconds(internetConnectionCheckInterval);
        }
    }
}
