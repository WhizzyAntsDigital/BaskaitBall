using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Threading.Tasks;


public class MainGameFlow : MonoBehaviour
{
    public static MainGameFlow Instance;
    [field: Header("Main Game Flow")]
    public float matchLength = 29f;
    [field: SerializeField] public float overTimeLength = 5f;
    public TextMeshProUGUI timerText;
    [field: SerializeField] private BallInput startingBall;
    [field: SerializeField] private InGameUI inGameUI;
    private float countDown = 3f;
    private bool startCountDown = false;
    private bool startMatchTimer = false;
    bool changedStartingBallValue = false;
    bool tieStarted = false;

    [field: Header("For Internet Connectivity")]
    [field: SerializeField] private GameObject noInternetPopup;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        startCountDown = true;
        startingBall.hasGotInput = true;
        GameManager.instance.OnGameOver += () => { GameManager.instance.isGameOver = true; startMatchTimer = false; startCountDown = false; };
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
        #region Count Down
        if (startCountDown)
        {
            countDown -= Time.deltaTime;
            if (countDown >= 1)
            {
                timerText.text = (Mathf.RoundToInt(countDown)).ToString();
            }
            if (countDown < 1 && countDown >= 0)
            {
                timerText.text = "START!";
                AIScore.instance.OnGameStart?.Invoke();
                if (!changedStartingBallValue)
                {
                    startingBall.hasGotInput = false;
                    changedStartingBallValue = true;
                }
            }
            else if (countDown <= 0)
            {
                startCountDown = false;
                startMatchTimer = true;
                if (tieStarted)
                {
                    ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = false;
                    tieStarted = false;
                }
            }
        }
        #endregion

        #region Main Match Timer
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

                inGameUI.CheckMatchResult();
                startMatchTimer = false;
            }
        }
        #endregion
    }

    public void WhenMatchTies()
    {
        tieStarted = true;
        int numberofballs = ArcadeLevel.Instance.ballsInScene.Length;
        for (int i = 0; i < numberofballs - 1; i++)
        {
            ArcadeLevel.Instance.ballsInScene[i].GetComponent<BallInput>().enabled = false;
        }
        GameManager.instance.onOvertime?.Invoke();
        timerText.text = "TIE";
        matchLength = overTimeLength;
        countDown = 3f;
        AfterTieDelay();

    }
    private async void AfterTieDelay()
    {
        await Task.Delay(2000);
        timerText.text = countDown.ToString();
        startCountDown = true;
        int numberofballs = ArcadeLevel.Instance.ballsInScene.Length;
        for (int i = 0; i < numberofballs - 1; i++)
        {
            ArcadeLevel.Instance.ballsInScene[i].GetComponent<BallInput>().enabled = true;
        }
        ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = true;
    }
    private void OnInternetConnectionChange(bool connected)
    {
        if (!connected)
        {
            Time.timeScale = 0f;
            noInternetPopup.SetActive(true);
        }
    }
}
