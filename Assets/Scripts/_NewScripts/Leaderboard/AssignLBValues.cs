using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssignLBValues : MonoBehaviour
{
    [field: Header("Leaderboard Content Values For Prefab")]
    [field: SerializeField] private TextMeshProUGUI usernameText;
    [field: SerializeField] private TextMeshProUGUI scoreText;

    public void AssignValues(string username, int  score)
    {
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}
