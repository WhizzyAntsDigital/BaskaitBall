using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [field: Header("Loading Screen Stuff")]
    [field: SerializeField] private GameObject loadingScenePanel;
    [field: SerializeField] private string tutorialSceneName;

    [field: Header("On Screen Stuff")]
    [field: SerializeField] TextMeshProUGUI tipsText;
    [field: SerializeField] List<string> loadingScreenText;
    [field: SerializeField] int timeBetweenText;
    [field: SerializeField] List<Sprite> loadingScreenSprites;
    [field: SerializeField] Image loadingScreenImage;

    float timer = 0;
    bool startTimer = false;

    private void Start()
    {
        tipsText.text = loadingScreenText[Random.Range(0, loadingScreenText.Count)];
        loadingScreenImage.sprite = loadingScreenSprites[Random.Range(0, loadingScreenSprites.Count)];
        startTimer = true;
    }

    public void CheckWhatToDo()
    {
        if(MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedTutorial)
        {
            loadingScenePanel.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(tutorialSceneName);
        }
        startTimer = false;
    }

    private void Update()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;
            if(timer >= timeBetweenText) 
            { 
                tipsText.text = loadingScreenText[Random.Range(0, loadingScreenText.Count)];
                timer = 0;
            }
        }
    }
}
