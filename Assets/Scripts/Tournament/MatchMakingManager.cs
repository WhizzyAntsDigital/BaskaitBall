using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MatchMakingManager : MonoBehaviour
{
    [field: Header("Tournament Bracket Generator")]
    [field: SerializeField] private TextMeshProUGUI playerUsername;
    [field: SerializeField] private TextMeshProUGUI opponentUsername;
    [field: SerializeField] private TextMeshProUGUI roundTitle;
    [field: SerializeField] private TextMeshProUGUI timerText;
    [field: SerializeField] private int maxTimeToSearchForPlayer = 10;
    [field: SerializeField] private int timeToGoToGame = 5;
    [field: SerializeField] private Image playerIcon;
    [field: SerializeField] private Image opponentIcon;
    [field: SerializeField] private string firstRoundName = "Entry Round";
    [field: SerializeField] private string secondRoundName = "Finals";
    [field: SerializeField] private string mainGameSceneName = "TournamentMode";

    private int playerSearchingTime;
    private float timer;
    private bool startTimer = false;

    private void Start()
    {
        playerSearchingTime = Random.Range(3, maxTimeToSearchForPlayer);
        SetValues();
        timerText.text = "Searching...";
        timer = timeToGoToGame;
    }

    private void Update()
    {
        if(startTimer)
        {
            timer -= Time.deltaTime;
            timerText.text = "Start in " + Mathf.RoundToInt(timer) + "...";
            if (timer <= 0)
            {
                timerText.text = "Loading...";
            }
        }
    }

    private void SetValues()
    {
        if(!UserDataHandler.instance.ReturnSavedValues().firstRound)
        {
            UserDataHandler.instance.ReturnSavedValues().firstRound = true;
            UserDataHandler.instance.SaveUserData();
            playerUsername.text = UserDataHandler.instance.ReturnSavedValues().userName;
            roundTitle.text = firstRoundName;
            opponentUsername.text = "???";
            SetOpponentUsernameFirstRound();
        }
        else if(UserDataHandler.instance.ReturnSavedValues().firstRound && !UserDataHandler.instance.ReturnSavedValues().secondRound)
        {
            UserDataHandler.instance.ReturnSavedValues().secondRound = true;
            UserDataHandler.instance.SaveUserData();
            playerUsername.text = UserDataHandler.instance.ReturnSavedValues().userName;
            roundTitle.text = secondRoundName;
            opponentUsername.text = AINamesGenerator.Utils.GetRandomName();
            GoToGameScene();
        }
        else
        {
            UserDataHandler.instance.ReturnSavedValues().firstRound = false;
            UserDataHandler.instance.ReturnSavedValues().secondRound = false;
            UserDataHandler.instance.SaveUserData();
            return;
        }
    }

    private async void SetOpponentUsernameFirstRound()
    {
        await Task.Delay(playerSearchingTime * 1000);
        opponentUsername.text = AINamesGenerator.Utils.GetRandomName();
        GoToGameScene();
    }
    private async void GoToGameScene()
    {
        startTimer = true;
        await Task.Delay(timeToGoToGame * 1000);
        startTimer = false;
        SceneManager.LoadScene(mainGameSceneName);
    }
}
