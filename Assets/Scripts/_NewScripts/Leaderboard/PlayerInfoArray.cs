using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfoArray
{
    public List<PlayerInfo> players;
    public PlayerInfoArray()
    {
        players = new List<PlayerInfo>();
    }
    

}
