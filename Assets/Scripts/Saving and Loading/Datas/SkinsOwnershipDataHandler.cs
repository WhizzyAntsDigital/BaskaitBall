using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsOwnershipDataHandler : MonoBehaviour
{
    public static SkinsOwnershipDataHandler instance;
    [SerializeField] private SkinsOwnershipData skinsOwnershipData;
    private void Awake()
    {
        instance = this;

        skinsOwnershipData = SaveLoadManager.LoadData<SkinsOwnershipData>();
        if (skinsOwnershipData == null)
        {
            skinsOwnershipData = new SkinsOwnershipData();
        }
    }

    #region Return All Data
    public SkinsOwnershipData ReturnSavedValues()
    {
        return skinsOwnershipData;
    }
    #endregion

    #region Saving
    public void SaveUserData()
    {
        SaveLoadManager.SaveData(skinsOwnershipData);
    }
    private void OnDisable()
    {
        SaveUserData();
    }
    #endregion
}

[System.Serializable]
public class SkinsOwnershipData
{
    public bool[] isOwned;
    public bool[] isEquipped;
}