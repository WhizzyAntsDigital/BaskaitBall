using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string playerId;
    public string playerName;
    public int rank;
    public double score;

    public PlayerInfo(string playerId, string playerName, int rank, double score)
    {
        this.playerId = playerId;
        this.playerName = playerName;
        this.rank = rank;
        this.score = score;
    }
}
