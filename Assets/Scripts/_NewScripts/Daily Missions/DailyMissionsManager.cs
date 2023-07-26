using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Quest { PlayMatches, GetShots, WinMatches, LoseMatches, Get3Pointers, WatchRewardedAD };
public enum QuestReward { FirstQuest, SecondQuest, ThirdQuest };

public class DailyMissionsManager : MonoBehaviour
{
    [field: Header("Daily Missions")]
    [field: SerializeField] private List<MissionsSpecs> dailyMissionsList;
    [field: SerializeField] private List<MissionsSpecs> todaysMissions;
    [field: SerializeField] private List<TextMeshProUGUI> missionDescriptions;
    [field: SerializeField] private List<TextMeshProUGUI> missionProgress;
    [field: SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        if (MissionsDataHandler.instance.CheckIfItsNextDay())
        {
            //Generates New Missions when the game is opened for the first time or when a new day starts
            ResetValues();
            GetNewMissions();
            FillInText();
        }
        else
        {
            //If it's current day, it assigns the already generated current Values
            for(int i = 0; i < MissionsDataHandler.instance.ReturnSavedValues().todaysMissions.Length ; i++)
            {
                int indexValue = MissionsDataHandler.instance.ReturnSavedValues().todaysMissions[i];
                todaysMissions.Add(dailyMissionsList[indexValue]);
                todaysMissions[i].todaysValue = ReturnTargetsForTheDay(i);
                todaysMissions[i].playerProgress = ReturnProgressForTheDay(i);
            }
            FillInText();
            UpdateProgress();
            CheckAndReward();
        }
    }

    private void Update()
    {
        CalculateTimeUntilNextDay();
    }

    private void GetNewMissions() //Generates New Daily Missions
    {
        todaysMissions = new List<MissionsSpecs>();

        int firstQuest = GetMissionBasedOnTier(QuestReward.FirstQuest);
        int secondQuest = GetMissionBasedOnTier(QuestReward.SecondQuest);
        int thirdQuest = GetMissionBasedOnTier(QuestReward.ThirdQuest);

        MissionsDataHandler.instance.ReturnSavedValues().todaysMissions[0] = firstQuest;
        MissionsDataHandler.instance.ReturnSavedValues().todaysMissions[1] = secondQuest;
        MissionsDataHandler.instance.ReturnSavedValues().todaysMissions[2] = thirdQuest;
        MissionsDataHandler.instance.SaveMissionsData();

        todaysMissions.Add(dailyMissionsList[firstQuest]);
        todaysMissions[0].todaysValue = Random.Range(todaysMissions[0].minValue, todaysMissions[0].maxValue + 1);

        todaysMissions.Add(dailyMissionsList[secondQuest]);
        todaysMissions[1].todaysValue = Random.Range(todaysMissions[1].minValue, todaysMissions[1].maxValue + 1);

        todaysMissions.Add(dailyMissionsList[thirdQuest]);
        todaysMissions[2].todaysValue = Random.Range(todaysMissions[2].minValue, todaysMissions[2].maxValue + 1);

        SaveTargetsForTheDay();
    }

    private int GetMissionBasedOnTier(QuestReward reward) //Returns Mission Based on Tier Chosen
    {
        int i = Random.Range(0, dailyMissionsList.Count);
        while (dailyMissionsList[i].questRewardType != reward)
        {
            i = Random.Range(0, dailyMissionsList.Count);
        }
        return i;
    }

    private void SaveTargetsForTheDay() //Saves the current day's missions' targets
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
        MissionsDataHandler.instance.SaveMissionsData();
    }

    private int ReturnTargetsForTheDay(int i)
    {
            switch (todaysMissions[i].questType)
            {
                case Quest.PlayMatches:
                    return MissionsDataHandler.instance.ReturnSavedValues().currentTargetForMatches;
                case Quest.GetShots:
                    return MissionsDataHandler.instance.ReturnSavedValues().currentTargetForShots;
                case Quest.WinMatches:
                    return MissionsDataHandler.instance.ReturnSavedValues().currentTargetForWins;
                case Quest.LoseMatches:
                    return MissionsDataHandler.instance.ReturnSavedValues().currentTargetForLosses;
                case Quest.Get3Pointers:
                    return MissionsDataHandler.instance.ReturnSavedValues().currentTargetForPointers;
                case Quest.WatchRewardedAD:
                    return MissionsDataHandler.instance.ReturnSavedValues().currentTargetForADs;
            default:
                return 69420;
            }
    }

    private int ReturnProgressForTheDay(int i)
    {
        switch (todaysMissions[i].questType)
        {
            case Quest.PlayMatches:
                return MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfMatches;
            case Quest.GetShots:
                return MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfShots;
            case Quest.WinMatches:
                return MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfWins;
            case Quest.LoseMatches:
                return MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfLosses;
            case Quest.Get3Pointers:
                return MissionsDataHandler.instance.ReturnSavedValues().currentNumberOf3Pointers;
            case Quest.WatchRewardedAD:
                return MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfADs;
            default:
                return 69420;
        }
    }

    private void FillInText() //Populates the Text Mesh Pros for display
    {
        for(int i = 0; i < missionDescriptions.Count; i++)
        {
            missionDescriptions[i].text = todaysMissions[i].questDescription;
            missionProgress[i].text = todaysMissions[i].playerProgress + "/" + todaysMissions[i].todaysValue;
        }
        if(MissionsDataHandler.instance.ReturnSavedValues().completedMission1)
        {
            missionProgress[0].text = todaysMissions[0].todaysValue + "/" + todaysMissions[0].todaysValue;
        }
        if (MissionsDataHandler.instance.ReturnSavedValues().completedMission2)
        {
            missionProgress[1].text = todaysMissions[1].todaysValue + "/" + todaysMissions[1].todaysValue;
        }
        if (MissionsDataHandler.instance.ReturnSavedValues().completedMission3)
        {
            missionProgress[2].text = todaysMissions[2].todaysValue + "/" + todaysMissions[2].todaysValue;
        }
    }

    private void ResetValues() //Resets all Values
    {
        MissionsDataHandler.instance.ResetAllData();
        for(int i = 0; i < dailyMissionsList.Count; i++)
        {
            dailyMissionsList[i].playerProgress = 0;
            dailyMissionsList[i].todaysValue = 0;
        }
    }

    private void UpdateProgress() //Updates Player's Progress
    {
        for(int i = 0; i < dailyMissionsList.Count; i ++)
        {
            switch (dailyMissionsList[i].questType)
            {
                case Quest.PlayMatches:
                    dailyMissionsList[i].playerProgress = MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfMatches;
                    break;
                case Quest.GetShots:
                    dailyMissionsList[i].playerProgress = MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfShots;
                    break;
                case Quest.WinMatches:
                    dailyMissionsList[i].playerProgress = MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfWins;
                    break;
                case Quest.LoseMatches:
                    dailyMissionsList[i].playerProgress = MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfLosses;
                    break;
                case Quest.Get3Pointers:
                    dailyMissionsList[i].playerProgress = MissionsDataHandler.instance.ReturnSavedValues().currentNumberOf3Pointers;
                    break;
                case Quest.WatchRewardedAD:
                    dailyMissionsList[i].playerProgress = MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfADs;
                    break;
            }
        }
    }

    private void CheckAndReward() //Compares Player's Progress with the target and rewards them accordingly.
    {
        //Mission 1
        if(!MissionsDataHandler.instance.ReturnSavedValues().completedMission1 && (todaysMissions[0].playerProgress >= todaysMissions[0].todaysValue))
        {
            print("Rewarded For 1");
            //RewardPlayer
            MissionsDataHandler.instance.ReturnSavedValues().completedMission1 = true;
            missionProgress[0].text = todaysMissions[0].todaysValue + "/" + todaysMissions[0].todaysValue;
        }

        //Mission 2
        if (!MissionsDataHandler.instance.ReturnSavedValues().completedMission2 && (todaysMissions[1].playerProgress >= todaysMissions[1].todaysValue))
        {
            print("Rewarded For 2");
            //RewardPlayer
            MissionsDataHandler.instance.ReturnSavedValues().completedMission2 = true;
            missionProgress[1].text = todaysMissions[1].todaysValue + "/" + todaysMissions[1].todaysValue;
        }

        //Mission 3
        if (!MissionsDataHandler.instance.ReturnSavedValues().completedMission3 && (todaysMissions[2].playerProgress >= todaysMissions[2].todaysValue))
        {
            print("Rewarded For 3");
            //RewardPlayer
            MissionsDataHandler.instance.ReturnSavedValues().completedMission3 = true;
            missionProgress[2].text = todaysMissions[2].todaysValue + "/" + todaysMissions[2].todaysValue;
        }
        MissionsDataHandler.instance.SaveMissionsData();
    }

    private void CalculateTimeUntilNextDay()
    {
        DateTime currentTime = GetInternetTime.Instance.GetCurrentDateTime();
        DateTime nextDay = currentTime.AddDays(1).Date;
        TimeSpan timeSpan = nextDay - currentTime;
        timerText.text = timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
        if(timeSpan.Hours >= 23 && timeSpan.Minutes >= 59 && MissionsDataHandler.instance.CheckIfItsNextDay())
        {
            ResetValues();
            GetNewMissions();
            FillInText();
        }
    }
}
