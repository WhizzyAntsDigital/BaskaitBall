using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [field: Header("Tutorial Parameters")]
    [field: SerializeField] private string tutorialSceneName;
    [field: SerializeField] private string mainMenuSceneName;
    [field: SerializeField] private GameObject onCompletionButtonsHolder;
    [field: SerializeField] private Button continueTextButton;
    [field: SerializeField] private List<GameObject> ballsInScene;
    [field: SerializeField] private float timeLimit;

    [field: Header("Instructions")]
    [field: SerializeField] private TextMeshProUGUI instructionsText;
    [field: SerializeField] private List<string> instructionsTextToDisplay;
    [field: SerializeField] private int indexOfFirstInput;
    [field: SerializeField] private int indexOfGoalBased;
    [field: SerializeField] private int targetToCompleteTutorial = 10;
    private int currentInstructionsIndex = 0;

    [field: Header("Recentre Ball")]
    [field: SerializeField] private GameObject spawnPoint;
    [field: SerializeField] private float speedOfMovingToSpawn;
    private int ballID = 0;
    private bool hitTrigger = false;
    private GameObject ballObj;

    [field: Header("BasketBall Stuff")]
    [field: SerializeField] TextMeshProUGUI timerText;
    [field: SerializeField] TextMeshProUGUI playerScore;
    [field: SerializeField] TextMeshProUGUI targetScore;
    private float timer = 30f;
    private bool startTimer = false;
    private int currentScore = 0;

    bool firstInput = false;
    bool secondInput = false;

    private void Start()
    {
        timerText.text = "00";
        playerScore.text = "00";
        targetScore.text = "00";
        onCompletionButtonsHolder.SetActive(false);
        GoToNextInstruction();
        AdjustInput(true);
        timer = timeLimit;
    }

    private void Update()
    {
        if (hitTrigger)
        {
            float step = speedOfMovingToSpawn * Time.deltaTime;
            ballObj.transform.position = Vector3.MoveTowards(ballObj.transform.position, spawnPoint.transform.position, step);
            if (ballObj.transform.position.x == spawnPoint.transform.position.x)
            {
                hitTrigger = false;
                ballObj.GetComponent<Rigidbody>().isKinematic = true;
                //if (!GameManager.instance.isMainGame)
                //{
                //    ballObj.GetComponent<TutorialBallInput>().hasGotInput = true;
                //}
                //else if (!GameManager.instance.isMainGame)
                //{
                //    ballObj.GetComponent<TutorialBallInput>().hasGotInput = false;
                //}
                //else
                //{
                    ballObj.GetComponent<TutorialBallInput>().hasGotInput = false;
                //}
                ballObj = null;
            }
        }

        if(startTimer)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.RoundToInt(timer).ToString();
            if(timer <= 0)
            {
                timerText.text = "END";
                if(currentScore < targetToCompleteTutorial) 
                {
                    instructionsText.text = "Let's try that again...";
                    timer = timeLimit;
                    timerText.text = "0";
                    currentScore = 0;
                    playerScore.text = "0";
                }
            }
        }
    }
    public void GoToNextInstruction()
    {
        if(currentInstructionsIndex == indexOfFirstInput)
        {
            continueTextButton.interactable = false;
            FirstInput();
        }
        if(currentInstructionsIndex == indexOfGoalBased)
        {
            continueTextButton.interactable = false;
            SecondInput();
        }
        instructionsText.text = instructionsTextToDisplay[currentInstructionsIndex];
        currentInstructionsIndex++;
    }

    private void FirstInput()
    {
        AdjustInput(false);
    }

    private void SecondInput()
    {
        AdjustInput(false);
        targetScore.text = targetToCompleteTutorial.ToString();
        playerScore.text = "0";
        timerText.text = timeLimit.ToString();
        startTimer = true;
    }

    public void OnTargetAchieved()
    {
        GoToNextInstruction();
        Invoke("EnableOnCompletionButtonHolder", 2f);
    }

    private void EnableOnCompletionButtonHolder()
    {
        onCompletionButtonsHolder.SetActive(true);
    }

    public void RestartTutorial()
    {
        MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedTutorial = true;
        MiscellaneousDataHandler.instance.SaveMiscData();
        SceneManager.LoadScene(tutorialSceneName);
    }

    public void GoToMainMenu()
    {
        MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedTutorial = true;
        MiscellaneousDataHandler.instance.SaveMiscData();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void AdjustInput(bool inputStatus)
    {
        if (inputStatus)
        {
            for (int i = 0; i < ballsInScene.Count; i++)
            {
                ballsInScene[i].GetComponent<TutorialBallInput>().hasGotInput = inputStatus;
            }
        }
        else
        {
            ballsInScene[ballID].GetComponent<TutorialBallInput>().hasGotInput = false;
            ballsInScene[ballID].GetComponent<TutorialBallInput>().kinematic = false;
            ballsInScene[ballID].GetComponent<Rigidbody>().isKinematic = false;
        }
        
    }
    public void MoveBallToCentre()
    {
        ballID++;
        ballID = ballID % ballsInScene.Count;
        ballObj = ballsInScene[ballID];
        hitTrigger = true;
    }

    public void OnBallEnterBasket()
    {
        if(!firstInput)
        {
            AdjustInput(true);
            continueTextButton.interactable = true;
            GoToNextInstruction();
            firstInput = true;
        }
        else if(!secondInput) 
        {
            currentScore++;
            playerScore.text = currentScore.ToString();
            if(currentScore == targetToCompleteTutorial)
            {
                AdjustInput(true);
                OnTargetAchieved();
                secondInput = true;
                startTimer = false;
            }
        }
    }
}
