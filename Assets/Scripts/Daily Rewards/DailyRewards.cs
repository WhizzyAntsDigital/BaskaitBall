using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DailyRewards : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI datetimeText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && GetInternetTime.Instance.IsTimeLodaed)
        {
            DateTime currentDateTime = GetInternetTime.Instance.GetCurrentDateTime();

            datetimeText.text = currentDateTime.ToString();
        }
    }
}