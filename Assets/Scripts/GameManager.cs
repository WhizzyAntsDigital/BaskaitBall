using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;
using TMPro;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: Header("General")]
    [field: SerializeField] private int targetFPS = 60;
    [field: SerializeField] public bool isMainGame { get; private set; } = true;

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

    [field: Header("Practice Game Flow")]
    [field: SerializeField] float practiceStartingTime = 30f;
    [field: SerializeField] int startingTarget = 50;
    [field: SerializeField] int timeDecrementValue = 5;
    [field: SerializeField] int targetIncrementValue = 10;
    [field: SerializeField] TextMeshProUGUI targetScore;
    private bool startPracticeTimer = false;
    private float practiceTimer = 0f;
    private float incrementCount = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Application.targetFrameRate = targetFPS;
        StartCoroutine(CheckInternetConnection());
        practiceTimer = practiceStartingTime;
        startCountDown = true;
        startingBall.hasGotInput = true;
        onGameOver += () => { isGameOver = true; };
        if(!isMainGame)
        {
            targetScore.text = startingTarget.ToString();
        }
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
                if(isMainGame)
                {
                    AIScore.instance.OnGameStart?.Invoke();
                }
                if (!changedStartingBallValue)
                {
                    startingBall.hasGotInput = false;
                    changedStartingBallValue = true;
                }
            }
            else if(countDown <-1)
            {
                startCountDown = false;
                if(isMainGame) 
                { 
                    startMatchTimer = true;
                }
                else
                {
                    startPracticeTimer = true;
                }
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

        //For FreeThrow
        if(startPracticeTimer)
        {
            practiceTimer -= Time.deltaTime;
            if(practiceTimer > 0)
            {
                timerText.text = Mathf.RoundToInt(practiceTimer).ToString();
            }
            else if(practiceTimer <= 0)
            {
                timerText.text = "END";
                startPracticeTimer = false;
                ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = true;
                freeThrowValuesUpdate();
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

    private void freeThrowValuesUpdate()
    {
        if(ScoreCalculator.instance.scoreValue < startingTarget)
        {
            onGameOver?.Invoke();
        }
        else
        {
            int randomChance = UnityEngine.Random.Range(0, 2);
            float testTimer = practiceStartingTime - (timeDecrementValue * incrementCount);
            if(testTimer <= 0)
            {
                randomChance = 1;
            }
            if (randomChance == 0)
            {
                startingTarget = ScoreCalculator.instance.scoreValue + (Mathf.RoundToInt(ScoreCalculator.instance.scoreValue * 0.2f));
                practiceTimer = practiceStartingTime - (timeDecrementValue * incrementCount);
            }
            else
            {
                startingTarget = ScoreCalculator.instance.scoreValue + (Mathf.RoundToInt(ScoreCalculator.instance.scoreValue * 0.4f));
                practiceTimer = practiceStartingTime + (timeDecrementValue * incrementCount);
            }
            incrementCount++;
            countDown = 3f;
            waitBroPlease();
        }
    }
    private async void waitBroPlease()
    {
        await Task.Delay(3000);
        timerText.text = practiceTimer.ToString();
        targetScore.text = startingTarget.ToString();
        ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = false;
        startCountDown = true;
    }
}
