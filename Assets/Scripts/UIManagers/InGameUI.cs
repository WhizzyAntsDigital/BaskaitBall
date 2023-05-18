using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    void Start()
    {
        touchArea.SetActive(true);
        gameOverScene.SetActive(false);
        GameManager.instance.onGameOver += () => { GameOverUI(); };
    }

    private void GameOverUI()
    {
        if(ScoreCalculator.instance.scoreValue > AIScore.instance.opponentScore)
        {
            gameResult.text = "YOU WIN!";
            gameResult.color = winColour;
        }
        else if (ScoreCalculator.instance.scoreValue < AIScore.instance.opponentScore)
        {
            gameResult.text = "YOU LOSE...";
            gameResult.color = loseColour;
        }
        else if ((ScoreCalculator.instance.scoreValue == AIScore.instance.opponentScore))
        {
            gameResult.text = "TIE!";
            gameResult.color = tieColour;
        }
        touchArea.SetActive(false);
        gameOverScene.SetActive(true);
        finalScore.text = ScoreCalculator.instance.scoreValue.ToString();
    }

}
