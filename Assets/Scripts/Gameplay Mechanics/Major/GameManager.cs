using UnityEngine;
using System;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: Header("General")]
    [field: SerializeField] private int targetFPS = 60;
    [field: SerializeField] public bool isMainGame { get; private set; } = true;
    [field: SerializeField] private InGameUI inGameUI;

    [field: Header("For BallInput Script")]
    [field: SerializeField] public GameObject ringObj { get; private set; } //Used as reference to calculate force for ball by BallInput Script

    [field: Header("Main Game Flow")]
    public float matchLength = 29f;
    [field: SerializeField] public float overTimeLength = 5f;
    public TextMeshProUGUI timerText;
    [field: SerializeField] private BallInput startingBall;
    [field: SerializeField] public bool isGameOver { get; private set; } = false;
    public Action onGameOver, onOvertime;
    private float countDown = 3f;
    private bool startCountDown = false;
    private bool startMatchTimer = false;
    bool changedStartingBallValue = false;
    bool tieStarted = false;

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
        #region Count Down
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
                    if(tieStarted)
                    {
                        ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = false;
                        tieStarted = false;
                    }
                }
                else
                {
                    startPracticeTimer = true;
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

        #region Practice Timer
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
        #endregion
    }
    
    public void WhenMatchTies()
    {
        tieStarted = true;
        int numberofballs = ArcadeLevel.Instance.ballsInScene.Length;
        for(int i = 0; i < numberofballs-1; i++)
        {
            ArcadeLevel.Instance.ballsInScene[i].GetComponent<BallInput>().enabled = false;
        }
        onOvertime?.Invoke();
        timerText.text = "TIE";
        matchLength = overTimeLength;
        countDown = 3f;
        AfterTieDelay();
        
    }
    private async void AfterTieDelay()
    {
        await Task.Delay(2000);
        timerText.text = practiceTimer.ToString();
        startCountDown = true;
        int numberofballs = ArcadeLevel.Instance.ballsInScene.Length;
        for (int i = 0; i < numberofballs - 1; i++)
        {
            ArcadeLevel.Instance.ballsInScene[i].GetComponent<BallInput>().enabled = true;
        }
        ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = true;
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
