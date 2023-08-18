using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager instance;
    private async void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerTheEvent(string eventName)
    {
#if !UNITY_EDITOR
        AnalyticsService.Instance.CustomData(eventName);
        AnalyticsService.Instance.Flush();
#endif
        HelperClass.DebugMessage("Custom Event Named " + eventName + " has been debugged");    
    }
}
