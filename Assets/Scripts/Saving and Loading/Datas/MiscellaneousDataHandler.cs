using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousDataHandler : MonoBehaviour
{
    public static MiscellaneousDataHandler instance;
    [SerializeField] private MiscellaneousData miscellaneousData;
    private void Awake()
    {
        instance = this;

        miscellaneousData = SaveLoadManager.LoadData<MiscellaneousData>();
        if (miscellaneousData == null)
        {
            miscellaneousData = new MiscellaneousData();
        }
    }


    #region Return All Data
    public MiscellaneousData ReturnSavedValues()
    {
        return miscellaneousData;
    }
    #endregion

    #region Saving
    public void SaveMiscData()
    {
        SaveLoadManager.SaveData(miscellaneousData);
    }
    private void OnDisable()
    {
        SaveMiscData();
    }
    #endregion
}

[System.Serializable]
public class MiscellaneousData
{
    public bool hasPlayedTutorial = false;
    public string Date_And_Time_DailyReward = null;
    public string Date_And_Time_Bonus = null;
    public bool hasRequestedReview = false;
    public bool hasComeFromMainGame = false;
    public bool hasClaimedDailyRewardToday = false;
    public bool hasPlayedDailyBonusLevel = false;
}