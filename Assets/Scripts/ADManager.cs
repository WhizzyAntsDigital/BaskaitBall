using Unity.Advertisement.IosSupport;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum TypeOfRewardedAD
{
    AddCoins,
    AddGems,
    BonusLevel
}
public class ADManager : MonoBehaviour
{
    public static ADManager Instance;
    [field: SerializeField] GameObject removeADsButton;
    [field: SerializeField] int amountOfCoinsForAdding = 200;
    [field: SerializeField] int amountOfGemsForAdding = 5;
    //Do not change these values
    private const string _androidAppID = "1a26eb165";
    private TypeOfRewardedAD typeOfAd;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        #region Popup Request
        IronSourceEvents.onConsentViewDidAcceptEvent += onConsentViewDidAcceptEvent;
        IronSourceEvents.onConsentViewDidFailToLoadWithErrorEvent += onConsentViewDidFailToLoadWithErrorEvent;
        IronSourceEvents.onConsentViewDidLoadSuccessEvent += onConsentViewDidLoadSuccessEvent;
        IronSourceEvents.onConsentViewDidFailToShowWithErrorEvent += onConsentViewDidFailToShowWithErrorEvent;
        IronSourceEvents.onConsentViewDidShowSuccessEvent += onConsentViewDidShowSuccessEvent;
        #endregion
        #region Rewarded Ads
        //Rewarded Ads
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
        #endregion
        #region Interstitial Ads
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
        #endregion

    }
    private void OnDisable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;

        #region Popup Request
        IronSourceEvents.onConsentViewDidAcceptEvent -= onConsentViewDidAcceptEvent;
        IronSourceEvents.onConsentViewDidFailToLoadWithErrorEvent -= onConsentViewDidFailToLoadWithErrorEvent;
        IronSourceEvents.onConsentViewDidLoadSuccessEvent -= onConsentViewDidLoadSuccessEvent;
        IronSourceEvents.onConsentViewDidFailToShowWithErrorEvent -= onConsentViewDidFailToShowWithErrorEvent;
        IronSourceEvents.onConsentViewDidShowSuccessEvent -= onConsentViewDidShowSuccessEvent;
        #endregion

        #region Rewarded Ads
        //Rewarded Ads
        IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;
        #endregion
        #region Interstitial Ads
        IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent -= InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent -= InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;
        #endregion 
    }
    private void Start()
    {
        IronSource.Agent.init(_androidAppID);
        IronSource.Agent.validateIntegration();
        IronSource.Agent.loadRewardedVideo();
        LoadInterstitialAd();
        if(PurchaseTrackerDataHandler.instance.ReturnSavedValues().hasPurchasedAdBlock)
        {
            removeADsButton.SetActive(false);
        }
        else
        {
            removeADsButton.SetActive(true);
        }
    }
    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void OnRewardedADComplete()
    {
        MissionTracker.instance.AdjustValues(Quest.WatchRewardedAD);
        if (typeOfAd == TypeOfRewardedAD.AddCoins)
        {
            CurrencyManager.instance.AdjustCoins(amountOfCoinsForAdding);
        }
        else if (typeOfAd == TypeOfRewardedAD.BonusLevel)
        {
            DailyBonusLevelManager.instance.LoadBonusLevel();
        }
        else if (typeOfAd == TypeOfRewardedAD.AddGems)
        {
            CurrencyManager.instance.AdjustGems(amountOfGemsForAdding);
        }
    }
    public void onInterstitialAdComplete()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnPurchaseOfAdBlock()
    {
        PurchaseTrackerDataHandler.instance.ReturnSavedValues().hasPurchasedAdBlock = true;
        removeADsButton.SetActive(false);
    }
    public void ShowRewardedAd(TypeOfRewardedAD type)
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
            typeOfAd = type;
        }
        else
        {
            //No Ad
        }
    }
    public void LoadInterstitialAd()
    {
        HelperClass.DebugMessage("Load interstial Started");
        IronSource.Agent.loadInterstitial();
    }
    public void ShowInterstitialAd()
    {
        if (!PurchaseTrackerDataHandler.instance.ReturnSavedValues().hasPurchasedAdBlock)
        {
            if (IronSource.Agent.isInterstitialReady())
            {
                IronSource.Agent.showInterstitial();
            }
        }
        else
        {
            onInterstitialAdComplete();
        }
    }
    private void SdkInitializationCompletedEvent()
    {
        IronSource.Agent.loadConsentViewWithType("pre");
    }
    #region Rewarded Ads
   
 
    //Listeners
    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there�s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
            OnRewardedADComplete();

    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it�s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }
    #endregion
    #region Interstitial Ads
    /************* Interstitial AdInfo Delegates *************/
    // Invoked when the interstitial ad was loaded succesfully.
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the initialization process has failed.
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
    }
    // Invoked when the Interstitial Ad Unit has opened. This is the impression indication.
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {

    }
    // Invoked when the ad failed to show.
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the interstitial ad closed and the user went back to the application screen.
    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        onInterstitialAdComplete();
    }
    // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
    // This callback is not supported by all networks, and we recommend using it only if
    // it's supported by all networks you included in your build.
    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
    }
    #endregion

    public void MaybeShowInterstitial()
    {
        if(!PurchaseTrackerDataHandler.instance.ReturnSavedValues().hasPurchasedAdBlock)
        {
            int randomnum = Random.Range(0, 2);
            if(randomnum == 0)
            {
                if(IronSource.Agent.isInterstitialReady())
                {
                    ShowInterstitialAd();
                }
            }
        }
    }
    #region Consent View
    // Consent View was loaded successfully
    private void onConsentViewDidShowSuccessEvent(string consentViewType)
    {
    }
    // Consent view was failed to load
    private void onConsentViewDidFailToShowWithErrorEvent(string consentViewType, IronSourceError error)
    {
    }

    // Consent view was displayed successfully
    private void onConsentViewDidLoadSuccessEvent(string consentViewType)
    {
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            IronSource.Agent.showConsentViewWithType("pre");
        }
    }

    // Consent view was not displayed, due to error
    private void onConsentViewDidFailToLoadWithErrorEvent(string consentViewType, IronSourceError error)
    {
    }
    // The user pressed the Settings or Next buttons
    private void onConsentViewDidAcceptEvent(string consentViewType)
    {
        ATTrackingStatusBinding.RequestAuthorizationTracking();
    }
    #endregion
}