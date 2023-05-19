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
    public float matchLength = 30f;
    public TextMeshProUGUI timerText;
    [field: SerializeField] private BallInput startingBall;
    [field: SerializeField] public bool isGameOver { get; private set; } = false;
    public Action onGameOver;
    private float countDown = 3f;
    private bool startCountDown = false;
    private bool startMatchTimer = false;
    bool changedStartingBallValue = false;

    [field: Header("Practice Game Flow")]
    [field: SerializeField] float practiceStartingTime = 30f;
    [field: SerializeField] int startingTarget = 50;
    [field: SerializeField] int timeChangeValue = 5;
    [field: SerializeField] int delayBetweenRounds = 3;
    [field: SerializeField] TextMeshProUGUI targetScore;
    [field: SerializeField] TMP_FontAsset greenText;
    [field: SerializeField] TMP_FontAsset redText;
    [field: SerializeField] TMP_FontAsset countDownText;
    [field: SerializeField] TMP_FontAsset practiceTimerText;
    public bool dontActivateInput = false;
    private bool startPracticeTimer = false;
    private float practiceTimer;
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
        if (isMainGame)
        {
            startingBall.hasGotInput = true;
        }
        else
        {
            ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = true;
        }
        onGameOver += () => { isGameOver = true; startMatchTimer = false; startCountDown = false; startPracticeTimer = false; };
        if (!isMainGame)
        {
            targetScore.text = startingTarget.ToString();
        }
    }

    private void Update()
    {
        if (startCountDown)
        {
            countDown -= Time.deltaTime;
            if (countDown >= 1)
            {
                if (!isMainGame) { timerText.font = countDownText; }
                timerText.text = (Mathf.RoundToInt(countDown)).ToString();
            }
            if (countDown < 1 && countDown >= 0)
            {
                if (!isMainGame)
                {
                    timerText.font = greenText;
                }
                timerText.text = "START!";
                if (isMainGame)
                {
                    AIScore.instance.OnGameStart?.Invoke();
                }
                if (isMainGame && !changedStartingBallValue)
                {
                    startingBall.hasGotInput = false;
                    changedStartingBallValue = true;
                }
                else if (!isMainGame && !changedStartingBallValue)
                {
                    ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = false;
                    changedStartingBallValue = true;
                }
            }
            else if (countDown <= 0)
            {
                startCountDown = false;
                if (isMainGame)
                {
                    startMatchTimer = true;
                }
                else
                {
                    startPracticeTimer = true;
                }
            }
        }
        if (startMatchTimer)
        {
            matchLength -= Time.deltaTime;
            if (matchLength > 0)
            {
                timerText.text = Mathf.RoundToInt(matchLength).ToString();
            }
            if (matchLength <= 0)
            {
                timerText.text = "END";
                onGameOver?.Invoke();
                startMatchTimer = false;
            }
        }

        //For FreeThrow
        if (startPracticeTimer)
        {
            practiceTimer -= Time.deltaTime;
            if (practiceTimer > 0)
            {
                timerText.font = practiceTimerText;
                timerText.text = Mathf.RoundToInt(practiceTimer).ToString();
            }
            else if (practiceTimer <= 0)
            {
                dontActivateInput = true;
                timerText.font = redText;
                timerText.text = "END";
                startPracticeTimer = false;
                int ballCount = ArcadeLevel.Instance.ballsInScene.Length;
                for (int i = 0; i < ballCount; i++)
                {
                    ArcadeLevel.Instance.ballsInScene[i].GetComponent<BallInput>().hasGotInput = true;
                }
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
                if (!whenDisconnectedActionsCarriedOut)
                {
                    IsDisconnectedFromInternet?.Invoke();
                    whenDisconnectedActionsCarriedOut = true;
                    whenConnectedActionsCarriedOut = false;
                }
            }
            else
            {
                if (!whenConnectedActionsCarriedOut)
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
        if (ScoreCalculator.instance.scoreValue < startingTarget)
        {
            onGameOver?.Invoke();
        }
        else
        {
            int randomChance = UnityEngine.Random.Range(0, 2);
            float checkTimer = practiceStartingTime - (timeChangeValue * incrementCount);
            if (checkTimer <= 0)
            {
                randomChance = 1;
            }
            if (randomChance == 0)
            {
                startingTarget = ScoreCalculator.instance.scoreValue + (Mathf.RoundToInt(ScoreCalculator.instance.scoreValue * UnityEngine.Random.Range(0.1f, 0.3f)));
                practiceTimer = practiceStartingTime - (timeChangeValue * incrementCount);
            }
            else
            {
                startingTarget = ScoreCalculator.instance.scoreValue + (Mathf.RoundToInt(ScoreCalculator.instance.scoreValue * UnityEngine.Random.Range(0.2f, 0.45f)));
                practiceTimer = practiceStartingTime + (timeChangeValue * incrementCount);
            }
            incrementCount++;
            countDown = 3f;
            StartNextRoundAfterDelay();
        }
    }
    private async void StartNextRoundAfterDelay()
    {
        await Task.Delay(delayBetweenRounds * 1000);
        timerText.text = practiceTimer.ToString();
        targetScore.text = startingTarget.ToString();
        startCountDown = true;
        changedStartingBallValue = false;
        dontActivateInput = false;
    }
}
