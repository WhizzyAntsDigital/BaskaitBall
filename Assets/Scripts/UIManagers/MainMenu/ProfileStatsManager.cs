using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileStatsManager : MonoBehaviour
{
    [field: Header("Profile & Statistics")]
    [field: SerializeField] private TextMeshProUGUI practiceHighScoreText;
    [field: SerializeField] private TextMeshProUGUI tournamentWinsText;
    [field: SerializeField] private TextMeshProUGUI tournamentLossesText;
    [field: SerializeField] private TextMeshProUGUI numberOfBasketsText;
    [field: SerializeField] private TextMeshProUGUI numberOfThreePointersText;

    public void SetPlayerStats()
    {
        practiceHighScoreText.text = UserDataHandler.instance.ReturnSavedValues().practiceHighScore.ToString();
        tournamentWinsText.text = UserDataHandler.instance.ReturnSavedValues().numberOfWins.ToString();
        tournamentLossesText.text = UserDataHandler.instance.ReturnSavedValues().numberOfLosses.ToString();
        numberOfBasketsText.text = UserDataHandler.instance.ReturnSavedValues().numberOfBaskets.ToString();
        numberOfThreePointersText.text = UserDataHandler.instance.ReturnSavedValues().numberOf3Pointers.ToString();
    }    
}
