using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanksSystemController : MonoBehaviour
{
    public static RanksSystemController instance;
    [field: Header("Rank System Info")]
    [field: SerializeField] List<RanksSpecifications> ranksInGame;

    private void Awake()
    {
        instance = this;
    }

    public void AdjustRank(int amountToAdjust)
    {
        UserDataHandler.instance.ReturnSavedValues().playerRankPoints += amountToAdjust;
    }

    public void OnRankUp()
    {

    }

    public int GetPlayerRank()
    {
        int currentRankOfPlayer = 0;
        for(int i = 0; i < ranksInGame.Count; i++)
        {
            if(i == (ranksInGame.Count - 1))
            {
                currentRankOfPlayer = i;
                break;
            }
            if(UserDataHandler.instance.ReturnSavedValues().playerRankPoints >= ranksInGame[i].minRankPointsNeeded && UserDataHandler.instance.ReturnSavedValues().playerRankPoints < ranksInGame[i++].minRankPointsNeeded)
            {
                currentRankOfPlayer = i;
                break;
            }
        }
        return currentRankOfPlayer;
    }
}
