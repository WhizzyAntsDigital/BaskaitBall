using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MissionsSpecs
{
    public int questID;
    public Quest questType;
    public QuestReward questRewardType;
    public string questDescription;
    public bool isActiveForToday = false;
    public int minValue;
    public int maxValue;
    public int todaysValue;
    public int playerProgress;
}
