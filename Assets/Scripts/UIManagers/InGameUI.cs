using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum MatchResult
{
    PlayerWon,
    PlayerLost
}
public class InGameUI : MonoBehaviour
{
    [field: Header("In Game UI Manager")]
    [field: SerializeField] private GameObject touchArea;
    [field: SerializeField] private GameObject gameOverScene;
    [field: SerializeField] private TextMeshProUGUI gameResult;
    [field: SerializeField] private TextMeshProUGUI finalScore;
    [field: SerializeField] private TextMeshProUGUI timerText;
    [field: SerializeField] private int timeToGoToNextScene = 3;
    [field: SerializeField] private Color winColour;
    [field: SerializeField] private Color loseColour;
    [field: SerializeField] private MainGameFlow mainGameFlow;
    [field: HideInInspector] public MatchResult matchResult;

    private float timer;
    private bool startTimer = false;
    void Start()
    {
        touchArea.SetActive(true);
        gameOverScene.SetActive(false);
        GameManager.instance.OnGameOver += () =>
        {
            if (GameManager.instance.isMainGame)
            {
                GameOverUI();
            }
            else
            {
                FreeThrowUI();
                if (ScoreCalculator.instance.scoreValue > UserDataHandler.instance.ReturnSavedValues().practiceHighScore)
                {
                    UserDataHandler.instance.ReturnSavedValues().practiceHighScore = ScoreCalculator.instance.scoreValue;
                    UserDataHandler.instance.SaveUserData();
                }
            }
        };
    }

    private void Update()
    {
        if(startTimer)
        {
            timer-=Time.deltaTime;
            timerText.text = "Continuing in " + Mathf.RoundToInt(timer) + "...";
            if(timer <= 0)
            {
                timerText.text = "Loading...";
            }
        }
    }

    private void GameOverUI()
    {
        if(matchResult == MatchResult.PlayerWon)
        {
            gameResult.text = "YOU WIN!";
            gameResult.color = winColour;
        }
        else if (matchResult == MatchResult.PlayerLost)
        {
            gameResult.text = "YOU LOSE...";
            gameResult.color = loseColour;
        }
        touchArea.SetActive(false);
        gameOverScene.SetActive(true);
        finalScore.text = ScoreCalculator.instance.scoreValue.ToString();
        if(UserDataHandler.instance.ReturnSavedValues().firstRound == true && UserDataHandler.instance.ReturnSavedValues().secondRound == false && matchResult == MatchResult.PlayerWon)
        {
            if (matchResult == MatchResult.PlayerWon)
            {
                SceneChange("TournamentLoadingScene");
            }
            else
            {
                UserDataHandler.instance.ReturnSavedValues().numberOfLosses++;
                UserDataHandler.instance.ReturnSavedValues().losingStreak++;
                UserDataHandler.instance.ReturnSavedValues().winningStreak = 0;
                UserDataHandler.instance.SaveUserData();
                SceneChange("MainMenu");
            }
        }
        else if(UserDataHandler.instance.ReturnSavedValues().firstRound == true && UserDataHandler.instance.ReturnSavedValues().secondRound == true)
        {
            UserDataHandler.instance.ReturnSavedValues().firstRound = false;
            UserDataHandler.instance.ReturnSavedValues().secondRound = false;
            if(matchResult == MatchResult.PlayerWon)
            {
                UserDataHandler.instance.ReturnSavedValues().numberOfWins++;
                UserDataHandler.instance.ReturnSavedValues().winningStreak++;
                UserDataHandler.instance.ReturnSavedValues().losingStreak = 0;
            }
            else
            {
                UserDataHandler.instance.ReturnSavedValues().numberOfLosses++;
                UserDataHandler.instance.ReturnSavedValues().losingStreak++;
                UserDataHandler.instance.ReturnSavedValues().winningStreak = 0;
            }
            UserDataHandler.instance.SaveUserData();
            SceneChange("MainMenu");
        }
    }
    private async void SceneChange(string sceneName)
    {
        timer = timeToGoToNextScene;
        startTimer = true;
        await Task.Delay(timeToGoToNextScene * 1000);
        SceneManager.LoadScene(sceneName);
    }
    private void FreeThrowUI()
    {
        gameResult.text = "TRAINING OVER!";
        gameResult.color = winColour;
        touchArea.SetActive(false);
        gameOverScene.SetActive(true);
        finalScore.text = ScoreCalculator.instance.scoreValue.ToString();
    }

    public void InvokeGameOverButton()
    {
        GameManager.instance.OnGameOver?.Invoke();
    }

    public void CheckMatchResult()
    {
        if (ScoreCalculator.instance.scoreValue > AIScore.instance.opponentScore)
        {
            matchResult = MatchResult.PlayerWon;
            GameManager.instance.OnGameOver?.Invoke();
        }
        else if (ScoreCalculator.instance.scoreValue < AIScore.instance.opponentScore)
        {
            matchResult = MatchResult.PlayerLost;
            GameManager.instance.OnGameOver?.Invoke();
        }
        else if ((ScoreCalculator.instance.scoreValue == AIScore.instance.opponentScore))
        {
            mainGameFlow.WhenMatchTies();
        }
    }

}
