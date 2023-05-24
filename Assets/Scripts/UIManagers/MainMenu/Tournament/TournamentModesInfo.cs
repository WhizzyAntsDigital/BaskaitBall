using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public class TournamentModesInfo
{
    public int tournamentID;
    public int tournamentCost;
    public int tournamentUnlockCost;
    public GameObject lockedOverlay;
    public bool tournamentChosen = false;
    public UnlockAnimation unlockAnimation;
}
