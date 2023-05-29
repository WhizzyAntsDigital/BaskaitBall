using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class PracticeGameFlow : MonoBehaviour
{
    [field: Header("Practice Game Flow")]
    [field: SerializeField] float practiceStartingTime = 30f;
    [field: SerializeField] int startingTarget = 50;
    [field: SerializeField] int timeChangeValue = 5;
    [field: SerializeField] int delayBetweenRounds = 3;
    [field: SerializeField] TextMeshProUGUI targetScore;
    [field: SerializeField] TextMeshProUGUI timerText;
    [field: SerializeField] TMP_FontAsset greenText;
    [field: SerializeField] TMP_FontAsset redText;
    [field: SerializeField] TMP_FontAsset countDownText;
    [field: SerializeField] TMP_FontAsset practiceTimerText;
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private AudioClip countDownSFX;
    bool startedCountdownSFX = false;
    public bool dontActivateInput = false;
    private bool startPracticeTimer = false;
    private float practiceTimer;
    private float incrementCount = 0;
    private bool startCountDown = false;
    private float countDown = 3f;
    private bool changedStartingBallValue = false;

    private void Start()
    {
        practiceTimer = practiceStartingTime;
        startCountDown = true;
        ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = true;
        GameManager.instance.OnGameOver += () => { GameManager.instance.isGameOver = true; startCountDown = false; startPracticeTimer = false; };
        targetScore.text = startingTarget.ToString();
    }
    private void Update()
    {
        #region Count Down
        if (startCountDown)
        {
            countDown -= Time.deltaTime;
            if (countDown >= 1)
            {
                if (!startedCountdownSFX)
                {
                    audioSource.clip = countDownSFX;
                    audioSource.Play();
                    startedCountdownSFX = true;
                }
                timerText.font = countDownText; 
                timerText.text = (Mathf.RoundToInt(countDown)).ToString();
            }
            if (countDown < 1 && countDown >= 0)
            {
                timerText.font = greenText;
                timerText.text = "START!";
                ArcadeLevel.Instance.ballsInScene[ArcadeLevel.Instance.ballID].GetComponent<BallInput>().hasGotInput = false;
                changedStartingBallValue = true;
            }
            else if (countDown <= 0)
            {
                startCountDown = false;
                startPracticeTimer = true;
                startedCountdownSFX = false;
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

    private void freeThrowValuesUpdate()
    {
        if (ScoreCalculator.instance.scoreValue < startingTarget)
        {
            GameManager.instance.OnGameOver?.Invoke();
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
                int newScore = ScoreCalculator.instance.scoreValue + (Mathf.RoundToInt(ScoreCalculator.instance.scoreValue * UnityEngine.Random.Range(0.1f, 0.3f)));
                if (newScore > 130 && checkTimer < 50)
                {
                    newScore = UnityEngine.Random.Range(0, 91);
                }
                startingTarget = newScore;
                practiceTimer = practiceStartingTime - (timeChangeValue * incrementCount);
            }
            else
            {
                int newScore = ScoreCalculator.instance.scoreValue + (Mathf.RoundToInt(ScoreCalculator.instance.scoreValue * UnityEngine.Random.Range(0.2f, 0.45f)));
                if(newScore > 130 && checkTimer < 50)
                {
                    newScore = UnityEngine.Random.Range(0, 131);
                }
                startingTarget = newScore;
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
