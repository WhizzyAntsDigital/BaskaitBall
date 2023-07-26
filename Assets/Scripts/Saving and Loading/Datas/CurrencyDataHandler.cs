using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDataHandler : MonoBehaviour
{
    public static CurrencyDataHandler instance;
    [SerializeField] private CurrencyData currencyData;
    private void Awake()
    {
        instance = this;

        currencyData = SaveLoadManager.LoadData<CurrencyData>();
        if (currencyData == null)
        {
            currencyData = new CurrencyData();
        }
    }


    #region Return All Data
    public CurrencyData ReturnSavedValues()
    {
        return currencyData;
    }
    #endregion

    #region Saving
    public void SaveCurrencyData()
    {
        SaveLoadManager.SaveData(currencyData);
    }
    private void OnDisable()
    {
        SaveCurrencyData();
    }
    #endregion
}

[System.Serializable]
public class CurrencyData
{
    public int amountOfCoins = 1000;
    public int amountOfGems = 10;
}