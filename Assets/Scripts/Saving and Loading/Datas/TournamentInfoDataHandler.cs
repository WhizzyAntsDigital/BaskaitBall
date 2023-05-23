using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentInfoDataHandler : MonoBehaviour
{
    public static TournamentInfoDataHandler instance;
    [SerializeField] private TournamentInfoData tournamentInfoData;
    [SerializeField] public List<TournamentModesInfo> allGameModesSpecs;

    private void Awake()
    {
        instance = this;

        tournamentInfoData = SaveLoadManager.LoadData<TournamentInfoData>();
        if (tournamentInfoData == null)
        {
            tournamentInfoData = new TournamentInfoData();
        }
    }
    private void Start()
    {
        AssignTourneyValues();
    }
    #region Assign Tourney Values On Start
    private void AssignTourneyValues()
    {
        for (int i = 0; i < allGameModesSpecs.Count; i++)
        {
            tournamentInfoData.prices[i] = allGameModesSpecs[i].tournamentCost;
            tournamentInfoData.selected[i] = false;
        }
        SaveTourneyData();
        
    }
    #endregion

    #region Return All Data
    public TournamentInfoData ReturnSavedValues()
    {
        return tournamentInfoData;
    }
    #endregion

    #region Saving
    public void SaveTourneyData()
    {
        SaveLoadManager.SaveData(tournamentInfoData);
    }
    private void OnDisable()
    {
        SaveTourneyData();
    }
    #endregion
}

[System.Serializable]
public class TournamentInfoData
{
    public int[] prices = {0,0,0,0};
    public bool[] selected = {false, false, false, false};
    public bool[] unlocked = {true, false, false, false};
}