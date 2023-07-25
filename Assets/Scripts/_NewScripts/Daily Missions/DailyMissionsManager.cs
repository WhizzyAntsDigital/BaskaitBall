using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Quest { PlayMatches, GetShots, WinMatches, LoseMatches, Get3Pointers, WatchRewardedAD };
public enum QuestReward { FirstQuest, SecondQuest, ThirdQuest };

public class DailyMissionsManager : MonoBehaviour
{
    [field: Header("Daily Missions")]
    [field: SerializeField] private List<MissionsSpecs> dailyMissionsList;
    [field: SerializeField] private List<MissionsSpecs> todaysMissions;
    [field: SerializeField] private List<TextMeshProUGUI> missionDescriptions;
    [field: SerializeField] private List<TextMeshProUGUI> missionProgress;

    private void Start()
    {
        //if(MissionsDataHandler.instance.CheckIfItsNextDay())
        //{
            ResetValues();
            GetNewMissions();
            FillInText();
        //}
        //else
        //{
        //    FillInText();
        //}
    }

    private void GetNewMissions()
    {
        todaysMissions = new List<MissionsSpecs>();

        todaysMissions.Add(dailyMissionsList[GetMissionBasedOnTier(QuestReward.FirstQuest)]);
        todaysMissions[0].todaysValue = Random.Range(todaysMissions[0].minValue, todaysMissions[0].maxValue + 1);
        todaysMissions[0].isActiveForToday = true;

        todaysMissions.Add(dailyMissionsList[GetMissionBasedOnTier(QuestReward.SecondQuest)]);
        todaysMissions[1].todaysValue = Random.Range(todaysMissions[1].minValue, todaysMissions[1].maxValue + 1);
        todaysMissions[1].isActiveForToday = true;

        todaysMissions.Add(dailyMissionsList[GetMissionBasedOnTier(QuestReward.ThirdQuest)]);
        todaysMissions[2].todaysValue = Random.Range(todaysMissions[2].minValue, todaysMissions[2].maxValue + 1);
        todaysMissions[2].isActiveForToday = true;

        SaveTargetsForTheDay();
    }

    private int GetMissionBasedOnTier(QuestReward reward)
    {
        int i = Random.Range(0, dailyMissionsList.Count);
        while (dailyMissionsList[i].questRewardType != reward)
        {
            i = Random.Range(0, dailyMissionsList.Count);
        }
        return i;
    }

    private void SaveTargetsForTheDay()
    {
        for(int i = 0; i < todaysMissions.Count; i++) 
        {
            switch(todaysMissions[i].questType)
            {
                case Quest.PlayMatches:
                    MissionsDataHandler.instance.ReturnSavedValues().currentTargetForMatches = todaysMissions[i].todaysValue;
                    break;
                case Quest.GetShots:
                    MissionsDataHandler.instance.ReturnSavedValues().currentTargetForShots = todaysMissions[i].todaysValue;
                    break;
                case Quest.WinMatches:
                    MissionsDataHandler.instance.ReturnSavedValues().currentTargetForWins = todaysMissions[i].todaysValue;
                    break;
                case Quest.LoseMatches:
                    MissionsDataHandler.instance.ReturnSavedValues().currentTargetForLosses = todaysMissions[i].todaysValue;
                    break;
                case Quest.Get3Pointers:
                    MissionsDataHandler.instance.ReturnSavedValues().currentTargetForPointers = todaysMissions[i].todaysValue;
                    break;
                case Quest.WatchRewardedAD:
                    MissionsDataHandler.instance.ReturnSavedValues().currentTargetForADs = todaysMissions[i].todaysValue;
                    break;
            }    
               
        }
    }

    private void FillInText()
    {
        for(int i = 0; i < missionDescriptions.Count; i++)
        {
            missionDescriptions[i].text = todaysMissions[i].questDescription;
            missionProgress[i].text = todaysMissions[i].playerProgress + "/" + todaysMissions[i].todaysValue;
        }
    }

    private void ResetValues()
    {
        MissionsDataHandler.instance.ResetAllData();
        for(int i = 0; i < dailyMissionsList.Count; i++)
        {
            dailyMissionsList[i].playerProgress = 0;
            dailyMissionsList[i].todaysValue = 0;
        }
    }
}
