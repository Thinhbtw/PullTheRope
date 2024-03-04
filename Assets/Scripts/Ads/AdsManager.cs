using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AdsManager : Singleton<AdsManager>
{
    string adUnitKey = "enmXpFsmcX7SfpGFuvnLm5aTuScxPJoH35SlgABnd6u0BfCk-RZqpcJiLHEP86Sev9NTM6u9P5aHtI09K7t7Cw";
    string rewardAdUnitId = "c79226aeb7ea322b";

    string STPAdsFromHome = "d336f3e77cc68911";
    string STPAdsFromGameplay = "0dad85b6831179ea";
    string InterstitialAdUnitId = "ccc40b6a8d82466d";

    string bannerAdUnitId = "690573e678679504";
    string AppOpenAdUnitId = "4f52e166b5b8c08a";


    bool ShowOpenAds = false;

    int retryAttempt;
    public bool gameIsOn, hasInternet, adsHint, adsSkip;
    [SerializeField] ButtonManager buttonManager;
    [SerializeField] UIGamePlay uiGamePlay;
    [SerializeField] FirebaseManager firebaseManager;

    bool hasLoadRewardAds;

    private void Awake()
    {
        gameIsOn = true;
        hasLoadRewardAds = false;
        if (!PlayerPrefs.HasKey("BuyRemoveAds"))
        {
            PlayerPrefs.SetInt("BuyRemoveAds", 0);
        }

        
#if UNITY_EDITOR
#else
            Debug.unityLogger.filterLogType = LogType.Warning;
            Debug.unityLogger.filterLogType = LogType.Error;
#endif
    }

    private void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [Obsolete]
    private void Start()
    {
        /*MaxSdk.SetIsAgeRestrictedUser(true);

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
            if (PlayerPrefs.GetInt("BuyRemoveAds") != 1)
            {
                InitializeBannerAds();
                InitializeInterstitialAds();
            }
            InitializeRewardedAds();
        };
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            if (PlayerPrefs.GetInt("BuyRemoveAds") != 1)
            {
                MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
            }
        };
        MaxSdk.SetSdkKey(adUnitKey);
            //MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        //}*/
        StartCoroutine(CheckInternetConnection());
    }

    [Obsolete]
    IEnumerator CheckInternetConnection() //set lai. gameIsOn thanh` true;
    {
        while (gameIsOn)
        {
            yield return new WaitForSeconds(1f);
            const string echoServer = "http://www.example.com";

            bool result;
            using (var request = UnityWebRequest.Head(echoServer))
            {
                request.timeout = 3;
                yield return request.SendWebRequest();
                result = !request.isNetworkError && !request.isHttpError && request.responseCode == 200;
            }
            if (result)
            {
                hasInternet = true;
                firebaseManager.isOffline = false;
            }
            else
            {
                hasInternet = false;
                firebaseManager.isOffline = true;
            }
        }
    }

   /* private void Update()
    {
        if (!ShowOpenAds)
        {
            ShowAdIfReady();
        }
        if (FirebaseManager.Instance.isShowBannerAds)
            ShowBanner();
    }
    #region Banner
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(bannerAdUnitId, "adaptive_banner", "false");
        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
    }
    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion
    #region Interstitials
    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }
    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        MaxSdk.LoadInterstitial(STPAdsFromHome);
        MaxSdk.LoadInterstitial(STPAdsFromGameplay);
    }
    public void ShowInterstitial()
    {     
        if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
        {
            if (((TimeSpan)(DateTime.Now - firebaseManager.CurrentTime)).TotalSeconds > firebaseManager.TimeShowInterAds)
            {
                firebaseManager.CurrentTime = DateTime.Now;
                if (firebaseManager.isShowInterAds)
                    MaxSdk.ShowInterstitial(InterstitialAdUnitId);
            }       
        }
    }

    public void ShowInterstitialFromHome()
    {
        
        if (MaxSdk.IsInterstitialReady(STPAdsFromHome))
        {
            if (((TimeSpan)(DateTime.Now - firebaseManager.CurrentTime)).TotalSeconds > firebaseManager.TimeShowInterAds)
            {
                firebaseManager.CurrentTime = DateTime.Now;
                if (firebaseManager.isShowInterAds)
                {
                    MaxSdk.ShowInterstitial(STPAdsFromHome);
                }
            }         
        }
    }

    public void ShowInterstitialFromGameplay()
    {
        if (MaxSdk.IsInterstitialReady(STPAdsFromGameplay))
        {
            if (((TimeSpan)(DateTime.Now - firebaseManager.CurrentTime)).TotalSeconds > firebaseManager.TimeShowInterAds)
            {
                firebaseManager.CurrentTime = DateTime.Now;
                if (firebaseManager.isShowInterAds)
                    MaxSdk.ShowInterstitial(STPAdsFromGameplay);
            }           
        }
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }
    #endregion
    #region Reward
    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }
    private void LoadRewardedAd()
    {
         MaxSdk.LoadRewardedAd(rewardAdUnitId);
    }
    public void ShowRewardedAd()
    {
        if (firebaseManager.isShowBannerAds)
        {
            if (MaxSdk.IsRewardedAdReady(rewardAdUnitId))
            {
                MaxSdk.ShowRewardedAd(rewardAdUnitId);
            }
        }
    }
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {

        // The rewarded ad displayed and the user should receive the reward.
        if(buttonManager.clickHint)
        {
            adsHint = true;
            buttonManager.clickHint = false;
        }
        if (buttonManager.clickSkip)
        {
            adsSkip = true;
            buttonManager.clickSkip = false;
        }
    }
    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }
    #endregion

    #region Open Ads
    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            ShowAdIfReady();
        }
    }
    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    }
    public void ShowAdIfReady()
    {
        if (FirebaseManager.Instance.isShowOpenAds)
        {
            if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
            {
                MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
                ShowOpenAds = true;
            }
            else
            {
                MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
            }
        }
    }
    #endregion*/
}
