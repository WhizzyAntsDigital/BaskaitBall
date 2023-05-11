using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: Header("For BallInput Script")]
    [field: SerializeField] public GameObject ringObj { get; private set; } //Used as reference to calculate force for ball by BallInput Script

    private void Awake()
    {
        instance = this;
    }
}
