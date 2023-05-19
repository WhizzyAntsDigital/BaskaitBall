using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public enum MatchResult
{
    PlayerWon,
    PlayerLost,
    Tie
}
public class InGameUI : MonoBehaviour
{
    [field: Header("In Game UI Manager")]
    [field: SerializeField] private GameObject touchArea;
    [field: SerializeField] private GameObject gameOverScene;
    [field: SerializeField] private TextMeshProUGUI gameResult;
    [field: SerializeField] private TextMeshProUGUI finalScore;
    [field: SerializeField] private Color winColour;
    [field: SerializeField] private Color loseColour;
    [field: SerializeField] private Color tieColour;
    public MatchResult matchResult;
    void Start()
    {
        touchArea.SetActive(true);
        gameOverScene.SetActive(false);
        GameManager.instance.onGameOver += () => { 
            if (GameManager.instance.isMainGame) 
            {
                GameOverUI(); 
            } 
            else 
            { 
                FreeThrowUI(); 
                if(ScoreCalculator.instance.scoreValue > UserDataHandler.instance.ReturnSavedValues().practiceHighScore)
                {
                    UserDataHandler.instance.ReturnSavedValues().practiceHighScore = ScoreCalculator.instance.scoreValue;
                    UserDataHandler.instance.SaveUserData();
                }
            } };
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
        else if (matchResult == MatchResult.Tie)
        {
            gameResult.text = "TIE!";
            gameResult.color = tieColour;
        }
        touchArea.SetActive(false);
        gameOverScene.SetActive(true);
        finalScore.text = ScoreCalculator.instance.scoreValue.ToString();
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
        GameManager.instance.onGameOver?.Invoke();
    }

    public void CheckMatchResult()
    {
        if (ScoreCalculator.instance.scoreValue > AIScore.instance.opponentScore)
        {
            matchResult = MatchResult.PlayerWon;
            GameManager.instance.onGameOver?.Invoke();
        }
        else if (ScoreCalculator.instance.scoreValue < AIScore.instance.opponentScore)
        {
            matchResult = MatchResult.PlayerLost;
            GameManager.instance.onGameOver?.Invoke();
        }
        else if ((ScoreCalculator.instance.scoreValue == AIScore.instance.opponentScore))
        {
            matchResult = MatchResult.Tie;
            GameManager.instance.WhenMatchTies();
        }
    }

}
