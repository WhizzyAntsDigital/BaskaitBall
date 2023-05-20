using UnityEngine;
using System;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: Header("General")]
    [field: SerializeField] private int targetFPS = 60;
    [field: SerializeField] private InGameUI inGameUI;

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
    }

}