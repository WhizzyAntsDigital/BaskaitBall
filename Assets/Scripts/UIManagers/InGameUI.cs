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
        else
        {
            gameResult.text = "YOU LOSE...";
            gameResult.color = loseColour;
        }
        touchArea.SetActive(false);
        gameOverScene.SetActive(true);
        finalScore.text = ScoreCalculator.instance.scoreValue.ToString();
    }

}
