using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    public static MissionTracker instance;

    private void Awake()
    {
        instance = this;
    }

    public void AdjustValues(Quest typeOfQuest)
    {
        if (!MissionsDataHandler.instance.ReturnSavedValues().completedMission1 || !MissionsDataHandler.instance.ReturnSavedValues().completedMission2 || !MissionsDataHandler.instance.ReturnSavedValues().completedMission3)
        {
            switch (typeOfQuest)
            {
                case Quest.PlayMatches:
                    MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfMatches++;
                    break;
                case Quest.GetShots:
                    MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfShots++;
                    break;
                case Quest.WinMatches:
                    MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfWins++;
                    break;
                case Quest.LoseMatches:
                    MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfLosses++;
                    break;
                case Quest.Get3Pointers:
                    MissionsDataHandler.instance.ReturnSavedValues().currentNumberOf3Pointers++;
                    break;
                case Quest.WatchRewardedAD:
                    MissionsDataHandler.instance.ReturnSavedValues().currentNumberOfADs++;
                    break;
            }
            MissionsDataHandler.instance.SaveMissionsData();
        }
    }
}
