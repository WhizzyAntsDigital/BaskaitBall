using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;

public class MatchMakingManager : MonoBehaviour
{
    [field: Header("Tournament Bracket Generator")]
    [field: SerializeField] private TextMeshProUGUI playerText;
    [field: SerializeField] private TextMeshProUGUI opponentText;
    [field: SerializeField] private TextMeshProUGUI currentTourneyReward;
    [field: SerializeField] private TextMeshProUGUI timerText;
    [field: SerializeField] private int maxTimeToSearchForPlayer = 10;
    [field: SerializeField] private int timeToGoToGame = 5;
    [field: SerializeField] private string mainGameSceneName = "TournamentMode";
    [field: SerializeField] AudioSource audioSource;
    [field: SerializeField] AudioClip coinAudioClip;
    [field: SerializeField] AudioClip playerFound;

    private int playerSearchingTime;
    private float timer;
    private bool startTimer = false;

    private int playersInvestingCoins;
    private float tempPlayersInvestingCoins;
    private int tournamentReward;
    private float tempTournamentReward;


    private float coinReductionRate;
    private bool startCoinChange = false;
    private bool startedAudio = false;

    [field: Header("Internet Connection Checking")]
    [field: SerializeField] private GameObject noInternetPopup;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        if(SettingsDataHandler.instance.ReturnSavedValues().soundMuted)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1;
        }


        playerSearchingTime = Random.Range(3, maxTimeToSearchForPlayer);
        SetValues();
        timerText.text = "Searching...";
        currentTourneyReward.text = "0";
        timer = timeToGoToGame;

        if (!InternetConnectivityChecker.Instance.CheckForInternetConnectionUponCommand())
        {
            OnInternetConnectionChange(false);
        }
        else
        {
            noInternetPopup.SetActive(false);
        }
    }
    private void OnEnable()
    {
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { OnInternetConnectionChange(false); };
    }


    private void OnDisable()
    {
        InternetConnectivityChecker.Instance.IsDisconnectedFromInternet += () => { OnInternetConnectionChange(false); };
    }
    private void OnInternetConnectionChange(bool connected)
    {
        if (!connected)
        {
            Time.timeScale = 0;
            noInternetPopup.SetActive(true);
        }
    }

    private void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;
            timerText.text = "Start in " + Mathf.RoundToInt(timer) + "...";
            if (timer <= 0)
            {
                timerText.text = "Loading...";
            }
        }

        if (startCoinChange)
        {
            tempPlayersInvestingCoins -= (Time.deltaTime * coinReductionRate * 2);
            tempTournamentReward += (Time.deltaTime * (coinReductionRate * 4));

            if (tempPlayersInvestingCoins >= 0 && tempTournamentReward <= tournamentReward)
            {
                playerText.text = Mathf.RoundToInt(tempPlayersInvestingCoins).ToString();
                opponentText.text = Mathf.RoundToInt(tempPlayersInvestingCoins).ToString();
                currentTourneyReward.text = Mathf.RoundToInt(tempTournamentReward).ToString();
                if (!startedAudio)
                {
                    audioSource.clip = coinAudioClip;
                    audioSource.loop = true;
                    audioSource.Play();
                    startedAudio = true;
                }
            }
            else if (tempPlayersInvestingCoins < 0 && tempTournamentReward > tournamentReward)
            {
                audioSource.Stop();
                audioSource.loop = false;
                startedAudio = false;
                playerText.text = "0";
                opponentText.text = "0";
                currentTourneyReward.text = tournamentReward.ToString();
                GoToGameScene();
                startCoinChange = false;
            }
        }
    }

    private void SetValues()
    {
        playerText.text = UserDataHandler.instance.ReturnSavedValues().userName;
        opponentText.text = "???";
        SetOpponentUsernameFirstRound();
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
        playersInvestingCoins = TournamentInfoDataHandler.instance.ReturnSavedValues().prices[selectedTournamentID];
        coinReductionRate = Mathf.Pow(10, ((TournamentInfoDataHandler.instance.ReturnSavedValues().prices[selectedTournamentID] * 4).ToString().Length - 2));
        playerText.text = playersInvestingCoins.ToString();
        opponentText.text = playersInvestingCoins.ToString();
        tournamentReward = playersInvestingCoins * 2;
        tempPlayersInvestingCoins = playersInvestingCoins;
        tempTournamentReward = 0;
        currentTourneyReward.text = "0";
        StartAnimatingCoins();
    }
    private async void SetOpponentUsernameFirstRound()
    {
        await Task.Delay(playerSearchingTime * 1000);
        audioSource.clip = playerFound;
        audioSource.Play();
        opponentText.text = AINamesGenerator.Utils.GetRandomName();
        timerText.text = "Starting...";
        GoToCoinAnimation();
    }
    private async void GoToGameScene()
    {
        startTimer = true;
        await Task.Delay(timeToGoToGame * 1000);
        startTimer = false;
        SceneManager.LoadScene(mainGameSceneName);
    }
    private async void StartAnimatingCoins()
    {
        await Task.Delay(2000);
        startCoinChange = true;
    }

    private async void GoToCoinAnimation()
    {
        await Task.Delay(1000);
        AnimateCoins();
    }
}
