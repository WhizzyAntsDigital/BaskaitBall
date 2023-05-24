using UnityEngine;
using System;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: Header("General")]
    [field: SerializeField] private int targetFPS = 60;
    [field: SerializeField] private InGameUI inGameUI;
    [field: SerializeField] private List<GameObject> arcadePrefabs;
    [field: SerializeField] public NeededGameObjects neededGameObjects { get; private set; }

    [field: Header("For BallInput Script")]
    [field: SerializeField] public GameObject ringObj { get; private set; } //Used as reference to calculate force for ball by BallInput Script

    [field: Header("General Game Flow")]
    [field: SerializeField] public bool isGameOver = false;
    [field: SerializeField] public bool isMainGame { get; private set; } = false;
    public Action OnGameOver, onOvertime;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Application.targetFrameRate = targetFPS;
        int selectedTournamentID = 0;
        for (int i = 0; i <= 3; i++)
        {
            if (TournamentInfoDataHandler.instance.ReturnSavedValues().selected[i] == true)
            {
                selectedTournamentID = i;
                break;
            }
        }
        GameObject temp = Instantiate(arcadePrefabs[selectedTournamentID], Vector3.zero, Quaternion.identity);
        neededGameObjects = temp.GetComponent<NeededGameObjects>();
        ringObj = neededGameObjects.ringObj;
    }

}