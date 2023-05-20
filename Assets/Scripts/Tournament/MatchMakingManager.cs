using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingManager : MonoBehaviour
{
    [field: Header("Tournament Bracket Generator")]
    [field: SerializeField] private TextMeshProUGUI playerUsername;
    [field: SerializeField] private TextMeshProUGUI opponentUsername;
    [field: SerializeField] private TextMeshProUGUI roundTitle;
    [field: SerializeField] private Image playerIcon;
    [field: SerializeField] private Image opponentIcon;

    private void Start()
    {
        string[] roundsNames = { "Semi Finals", "Finals" };
        playerUsername.text = AINamesGenerator.Utils.GetRandomName();
        opponentUsername.text = AINamesGenerator.Utils.GetRandomName();
        roundTitle.text = roundsNames[Random.Range(0, 2)];
    }
}
