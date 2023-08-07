using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [field: SerializeField] private TextMeshProUGUI finalScore;
    [field: SerializeField] private TextMeshProUGUI timerText;
    [field: SerializeField] private TextMeshProUGUI playerCoins;
    [field: SerializeField] private TextMeshProUGUI playerUsername;
    [field: SerializeField] private Image playerPFP;
    [field: SerializeField] private TextMeshProUGUI opponentCoins;
    [field: SerializeField] private TextMeshProUGUI opponentUsername;
    [field: SerializeField] private Image opponentPFP;
    [field: SerializeField] private TextMeshProUGUI currentTourneyReward;
    [field: SerializeField] GameObject playerWonIcon;
    [field: SerializeField] GameObject opponentWonIcon;
    [field: SerializeField] GameObject confirmExit;
    [field: SerializeField] private int timeToGoToNextScene = 3;
    [field: SerializeField] private Color winColour;
    [field: SerializeField] private Color loseColour;
    [field: SerializeField] private MainGameFlow mainGameFlow;
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private AudioClip winSound;
    [field: SerializeField] private AudioClip loseSound;
    [field: HideInInspector] public MatchResult matchResult;

    [field: SerializeField] private TextMeshProUGUI highScore;

    [field: Header("Bonus Level Stuff")]
    [field: SerializeField] TextMeshProUGUI amountOfGemsEarned;

    private float timer;
    private bool startTimer = false;
    private string SceneToLoad;

    private int playersInvestingCoins;
    private float tempPlayersInvestingCoins;
    private int tournamentReward;
    private float tempTournamentReward;


    private float coinReductionRate;
    private bool startCoinChange = false;


    void Start()
    {
        touchArea.SetActive(true);
        gameOverScene.SetActive(false);
        confirmExit.SetActive(false);
        if (GameManager.instance.isMainGame)
        {
            currentTourneyReward.text = tournamentReward.ToString();
        }
        GameManager.instance.OnGameOver += () =>
        {
            if (GameManager.instance.isMainGame)
            {
                GameOverUI();
                confirmExit.SetActive(false);
            }
            else
            {
                FreeThrowUI();
                if (ScoreCalculator.instance.scoreValue > UserDataHandler.instance.ReturnSavedValues().practiceHighScore)
                {
                    UserDataHandler.instance.ReturnSavedValues().practiceHighScore = ScoreCalculator.instance.scoreValue;
                    UserDataHandler.instance.SaveUserData();
                }
                highScore.text = UserDataHandler.instance.ReturnSavedValues().practiceHighScore.ToString();
            }
        };
    }

    private void Update()
    {
        if (startTimer)
        {
            timerText.gameObject.SetActive(true);
            timer -= Time.deltaTime;
            timerText.text = "Continuing in " + Mathf.RoundToInt(timer) + "...";
            if (timer <= 0)
            {
                timerText.text = "Loading...";
            }
        }

        if (startCoinChange)
        {
            tempPlayersInvestingCoins += (Time.deltaTime * coinReductionRate * 2);
            tempTournamentReward -= (Time.deltaTime * (coinReductionRate * 4));
            if (tempPlayersInvestingCoins <= tournamentReward && tempTournamentReward >= 0)
            {
                if (matchResult == MatchResult.PlayerWon)
                {
                    playerCoins.text = Mathf.RoundToInt(tempPlayersInvestingCoins).ToString();
                }
                else
                {
                    opponentCoins.text = Mathf.RoundToInt(tempPlayersInvestingCoins).ToString();
                }
                currentTourneyReward.text = Mathf.RoundToInt(tempTournamentReward).ToString();
            }
            else
            {
                if (matchResult == MatchResult.PlayerWon)
                {
                    playerCoins.text = tournamentReward.ToString();
                }
                else
                {
                    opponentCoins.text = tournamentReward.ToString();
                }
                currentTourneyReward.text = "0";
                SceneChange(SceneToLoad);
                startCoinChange = false;
            }
        }
    }
    private void AnimateCoins()
    {
        int selectedTournamentID = 0;
        for (int i = 0; i <= 3; i++)
        {
            if (TournamentInfoDataHandler.instance.ReturnSavedValues().selected[i] == true)
            {
                selectedTournamentID = i;
                break;
            }
        }
        if (matchResult == MatchResult.PlayerWon)
        {
            CurrencyManager.instance.AdjustCoins((TournamentInfoDataHandler.instance.ReturnSavedValues().prices[selectedTournamentID] * 2));
        }
        playersInvestingCoins = TournamentInfoDataHandler.instance.ReturnSavedValues().prices[selectedTournamentID];
        coinReductionRate = Mathf.Pow(10, ((TournamentInfoDataHandler.instance.ReturnSavedValues().prices[selectedTournamentID] * 4).ToString().Length - 2));
        playerCoins.text = "0";
        opponentCoins.text = "0";
        tournamentReward = playersInvestingCoins * 2;
        tempPlayersInvestingCoins = 0;
        tempTournamentReward = tournamentReward;
        currentTourneyReward.text = tournamentReward.ToString();
        StartAnimatingCoins();

    }
    private void GameOverUI()
    {
        playerUsername.text = Social.localUser.userName;
        opponentUsername.text = CurrencyDataHandler.instance.ReturnSavedValues().opponentName;
        CurrencyDataHandler.instance.AssignImg(playerPFP, true);
        CurrencyDataHandler.instance.AssignImg(opponentPFP, false);
        
        touchArea.SetActive(false);
        gameOverScene.SetActive(true);

        timerText.gameObject.SetActive(false);
        if (matchResult == MatchResult.PlayerWon)
        {
            UserDataHandler.instance.ReturnSavedValues().numberOfWins++;
            if (UserDataHandler.instance.ReturnSavedValues().numberOfWins % 5 == 0 && MiscellaneousDataHandler.instance.ReturnSavedValues().hasRequestedReview == true)
            {
                MiscellaneousDataHandler.instance.ReturnSavedValues().hasRequestedReview = false;
            }
            UserDataHandler.instance.ReturnSavedValues().winningStreak++;
            UserDataHandler.instance.ReturnSavedValues().losingStreak = 0;
            playerWonIcon.SetActive(true);
            opponentWonIcon.SetActive(false);
        }
        else
        {
            UserDataHandler.instance.ReturnSavedValues().numberOfLosses++;
            UserDataHandler.instance.ReturnSavedValues().losingStreak++;
            UserDataHandler.instance.ReturnSavedValues().winningStreak = 0;
            playerWonIcon.SetActive(false);
            opponentWonIcon.SetActive(true);
        }
        UserDataHandler.instance.SaveUserData();
        MiscellaneousDataHandler.instance.SaveMiscData();
        SceneToLoad = "MainMenu";

        if(Random.Range(0,3) != 0)
        {
            MiscellaneousDataHandler.instance.ReturnSavedValues().hasComeFromMainGame = true;
            MiscellaneousDataHandler.instance.SaveMiscData();
        }

        AnimateCoins();
    }
    private async void SceneChange(string sceneName)
    {
        timer = timeToGoToNextScene;
        startTimer = true;
        await Task.Delay(timeToGoToNextScene * 1000);
        SceneManager.LoadScene(sceneName);
    }
    private async void StartAnimatingCoins()
    {
        await Task.Delay(1000);
        startCoinChange = true;
    }
    private void FreeThrowUI()
    {
        touchArea.SetActive(false);
        gameOverScene.SetActive(true);
        finalScore.text = ScoreCalculator.instance.scoreValue.ToString();

        int amountOfGemsToAdd = (ScoreCalculator.instance.scoreValue / 100);
        CurrencyManager.instance.AdjustGems(amountOfGemsToAdd);
        amountOfGemsEarned.text = amountOfGemsToAdd.ToString();

        if (Random.Range(0, 3) != 0)
        {
            MiscellaneousDataHandler.instance.ReturnSavedValues().hasComeFromMainGame = true;
            MiscellaneousDataHandler.instance.SaveMiscData();
        }
    }

    public void ReturnToMainMenu()
    {
        confirmExit.SetActive(!confirmExit.activeInHierarchy);
        if (!GameManager.instance.isMainGame && !confirmExit.activeInHierarchy)
        {
            Time.timeScale = 1.0f;
        }
        else if (!GameManager.instance.isMainGame && confirmExit.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
    }
    public void ConfirmExit()
    {
        if (GameManager.instance.isMainGame)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            confirmExit.SetActive(false);
            Time.timeScale = 1.0f;
            InvokeGameOverButton();
        }
    }
    private void InvokeGameOverButton()
    {
        GameManager.instance.OnGameOver?.Invoke();
    }

    public void CheckMatchResult()
    {
        if (ScoreCalculator.instance.scoreValue > AIScore.instance.opponentScore)
        {
            matchResult = MatchResult.PlayerWon;
            audioSource.clip = winSound;
            audioSource.Play();
            MissionTracker.instance.AdjustValues(Quest.WinMatches);
            MissionTracker.instance.AdjustValues(Quest.PlayMatches);
            GameManager.instance.OnGameOver?.Invoke();
        }
        else if (ScoreCalculator.instance.scoreValue < AIScore.instance.opponentScore)
        {
            matchResult = MatchResult.PlayerLost;
            audioSource.clip = loseSound;
            audioSource.Play();
            MissionTracker.instance.AdjustValues(Quest.LoseMatches);
            MissionTracker.instance.AdjustValues(Quest.PlayMatches);
            GameManager.instance.OnGameOver?.Invoke();
        }
        else if ((ScoreCalculator.instance.scoreValue == AIScore.instance.opponentScore))
        {
            mainGameFlow.WhenMatchTies();
        }
    }
}
