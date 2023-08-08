using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdRewards : MonoBehaviour
{
    public void RewardedADsCoins()
    {
        ADManager.Instance.ShowRewardedAd(TypeOfRewardedAD.AddCoins);
    }
    public void RewardedADsGems()
    {
        ADManager.Instance.ShowRewardedAd(TypeOfRewardedAD.AddGems);
    }
}
