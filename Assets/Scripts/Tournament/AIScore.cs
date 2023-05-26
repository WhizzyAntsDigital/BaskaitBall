using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AIScore : MonoBehaviour
{
    public static AIScore instance;

    [field: Header("AI Score Calculations")]
    [field: SerializeField] float scoreCalculationInterval = 1f;
    [field: Range(0, 101)] int playerWinPercentage = 50;
    [field: SerializeField][field: Range(0, 101)] int percentageOfHittingShot;
    [field: SerializeField][field: Range(0, 101)] int percentageofHittingDirectShot;
    [field: SerializeField] TextMeshProUGUI AIScoreDisplay;
    [field: SerializeField] private int lossStreakThreshold = 2;
    [field: SerializeField] private int winStreakThreshold = 4;
    public int opponentScore = 0;
    public Action OnGameStart;

    private bool hasCoroutineStarted = false;
    private float timerRandomized;
    private bool firstTimeIgnored = false;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AIScoreDisplay = GameManager.instance.neededGameObjects.AIScore;
        AIScoreDisplay.text = opponentScore.ToString();
        int willPlayerWin = UnityEngine.Random.Range(0, 101);
        int shouldFollowStreak = UnityEngine.Random.Range(0, 101);
        if(playerWinPercentage < 1)
        {
            playerWinPercentage = 50;
        }
        if(UserDataHandler.instance.ReturnSavedValues().winningStreak >= winStreakThreshold)
        {
            if (shouldFollowStreak >= 0 && shouldFollowStreak <= 75f)
            {
                willPlayerWin = UnityEngine.Random.Range(1, 30);
            }
        }
        else if(UserDataHandler.instance.ReturnSavedValues().losingStreak >= lossStreakThreshold)
        {
            if (shouldFollowStreak >= 0f && shouldFollowStreak <= 0.7f)
            {
                willPlayerWin = UnityEngine.Random.Range(70,100);
            }
        }
        if (willPlayerWin >= 0f && willPlayerWin <= playerWinPercentage)
        {
            percentageOfHittingShot = UnityEngine.Random.Range(20,50);
            percentageofHittingDirectShot = UnityEngine.Random.Range(20,50);
        }
        else
        {
            percentageOfHittingShot = UnityEngine.Random.Range(51,90);
            percentageofHittingDirectShot = UnityEngine.Random.Range(51, 75);
        }

        OnGameStart += () => { if (!hasCoroutineStarted) { StartCoroutine(ScoreCalculator()); hasCoroutineStarted = true; } };
        GameManager.instance.onOvertime += () => { StopCoroutine(ScoreCalculator()); hasCoroutineStarted = false; firstTimeIgnored = false; };
        GameManager.instance.OnGameOver += () => { StopCoroutine(ScoreCalculator()); hasCoroutineStarted = false; firstTimeIgnored = false; };
    }

    private IEnumerator ScoreCalculator()
    {
        while (!GameManager.instance.isGameOver)
        {
            if (firstTimeIgnored)
            {
                AIScoringChances();
                AIScoreDisplay.text = opponentScore.ToString();
                timerRandomized = UnityEngine.Random.Range((scoreCalculationInterval - 0.5f), (scoreCalculationInterval + 0.5f));
            }
            else
            {
                firstTimeIgnored = true;
            }
            yield return new WaitForSeconds(timerRandomized);
        }
    }

    private void AIScoringChances()
    {
        int scoringChance = UnityEngine.Random.Range(0, 101);
        int threePointerChance = UnityEngine.Random.Range(0, 101);
        if(scoringChance >=0 && scoringChance <= percentageOfHittingShot && !GameManager.instance.isGameOver)
        {
            if(threePointerChance >= 0 && threePointerChance <= percentageofHittingDirectShot && !GameManager.instance.isGameOver)
            {
                opponentScore += 3;
            }
            else if (threePointerChance > percentageofHittingDirectShot && !GameManager.instance.isGameOver)
            {
                opponentScore += 2;
            }
        }
    }
}
