using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class GoogleAdMobManager : MonoBehaviour
{
    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    public bool adsHint, adsSkip, loadOpenAdFailed;
    [SerializeField] ButtonManager buttonManager;
    [SerializeField] FirebaseManager firebaseManager;
    //[SerializeField] Text errorTxt;
    int countInter = 0;
    bool ShowOpenAds = false;
    bool ShowBanner = false;
    bool RewardedUser = false;

    string _openAppId = "ca-app-pub-4810228587303243/6275051158";

    [Obsolete]
    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
        //}
        LoadAppOpenAd();
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    [Obsolete]
    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            RequestBannerAd();
            RequestAndLoadFullscreenNextLevelInterstitialAd();
            RequestAndLoadRewardedAd();
        });
    }

    private void Update()
    {
        //if (firebaseManager.isUseAdmobAds)
        //{
        if (firebaseManager.isShowBannerAds && PlayerPrefs.GetInt("BuyRemoveAds") != 1 && !ShowBanner)
        {
            RequestBannerAd();
            ShowBanner = true;
        }
        //}
    }

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    #endregion
    #region BANNER ADS

    public void RequestBannerAd()
    {

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4810228587303243/9226579750";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Load a banner ad
        if (firebaseManager.isShowBannerAds && PlayerPrefs.GetInt("BuyRemoveAds") != 1)
            bannerView.LoadAd(CreateAdRequest());
    }
    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion
    #region INTERSTITIAL ADS

    public void RequestAndLoadFullscreenNextLevelInterstitialAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4810228587303243/9533701519";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        // Load an interstitial ad
        InterstitialAd.Load(adUnitId, CreateAdRequest(),
            (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    return;
                }
                else if (ad == null)
                {
                    return;
                }
                interstitialAd = ad;
                ad.OnAdFullScreenContentClosed += () =>
                {
                    //buttonManager.eventsManager.enabled = true;
                    RequestAndLoadFullscreenNextLevelInterstitialAd();
                };
                ad.OnAdFullScreenContentOpened += () =>
                {
                    //buttonManager.eventsManager.enabled = false;
                };
                ad.OnAdClicked += () =>
                {

                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    RequestAndLoadFullscreenNextLevelInterstitialAd();
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {

                };
            });
    }
    public void ShowFullscreenNextLevelInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            if (firebaseManager.isShowInterAds)
            {
                if (countInter < 1)
                {
                    interstitialAd.Show();
                    firebaseManager.CurrentTime = DateTime.Now;
                    countInter++;
                }
                else
                if ((DateTime.Now - firebaseManager.CurrentTime).TotalSeconds > firebaseManager.TimeShowInterAds)
                {
                    interstitialAd.Show();
                    firebaseManager.CurrentTime = DateTime.Now;
                }

            }
        }
    }
    public void DestroyFullscreenNextLevelInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion
    #region AppOpen
    public bool IsAppOpenAdAvailable
    {
        get
        {
            return (appOpenAd != null
                    && appOpenAd.CanShowAd()
                   );
        }
    }
    public void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        UnityEngine.Debug.Log("App State is " + state);

        // OnAppStateChanged is not guaranteed to execute on the Unity UI thread.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (state == AppState.Foreground)
            {
                ShowAppOpenAd();
            }
        });
    }
    public void ShowAppOpenAd()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            //errorTxt.text = "Showing app open ad.";
            appOpenAd.Show();
            ShowOpenAds = true;
        }
        else
        {
            //errorTxt.text = "App open ad is not ready yet.";
        }

    }
    public bool IsAdAvailable
    {
        get
        {
            return _openAppId != null;
        }
    }

    public void LoadAppOpenAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }
        //errorTxt.text = "Loading the app open ad.";

        // Create our request used to load the ad.
        //var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        AppOpenAd.Load(_openAppId, ScreenOrientation.Portrait, CreateAdRequest(),
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    loadOpenAdFailed = true;
                    /*errorTxt.text = "app open ad failed to load an ad " +
                                   "with error : " + error;*/
                    return;
                }

                /*errorTxt.text = "App open ad loaded with response : "
                              + ad.GetResponseInfo();*/

                appOpenAd = ad;
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    /*errorTxt.text = String.Format("App open ad paid {0} {1}.",
                            adValue.Value,
                            adValue.CurrencyCode);*/
                };
                // Raised when an impression is recorded for an ad.
                ad.OnAdImpressionRecorded += () =>
                {
                    //errorTxt.text = "App open ad recorded an impression.";
                };
                // Raised when a click is recorded for an ad.
                ad.OnAdClicked += () =>
                {
                   // errorTxt.text = "App open ad was clicked.";
                };
                // Raised when an ad opened full screen content.
                ad.OnAdFullScreenContentOpened += () =>
                {
                    //buttonManager.eventsManager.enabled = false;
                    //errorTxt.text = "App open ad full screen content opened.";
                };
                // Raised when the ad closed full screen content.
                ad.OnAdFullScreenContentClosed += () =>
                {
                    //buttonManager.eventsManager.enabled = true;
                    //errorTxt.text = "App open ad full screen content closed.";
                };
                // Raised when the ad failed to open full screen content.
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    /*errorTxt.text = "App open ad failed to open full screen content " +
                                       "with error : " + error;*/
                };

            });
    }

    public void DestroyAppOpenAd()
    {
        if (this.appOpenAd != null)
        {
            this.appOpenAd.Destroy();
            this.appOpenAd = null;
        }
    }
    #endregion
    #region REWARDED ADS

    [Obsolete]
    public void RequestAndLoadRewardedAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-4810228587303243/7917367518";
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        RewardedAd.Load(adUnitId, CreateAdRequest(),
            (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    return;
                }
                else if (ad == null)
                {
                    return;
                }
                rewardedAd = ad;
                ad.OnAdFullScreenContentOpened += () =>
                {
                    //buttonManager.eventsManager.enabled = false;
                };
                ad.OnAdImpressionRecorded += () =>
                {

                };
                ad.OnAdClicked += () =>
                {

                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    RequestAndLoadRewardedAd();
                };
                ad.OnAdClosed += (object sender, EventArgs a) =>
                {
                    //buttonManager.eventsManager.enabled = true;
                    RequestAndLoadRewardedAd();
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {

                };
                ad.OnUserEarnedReward += Ad_OnUserEarnedReward;
            });
    }

    private void Ad_OnUserEarnedReward(object sender, GoogleMobileAds.Api.Reward e)
    {
        if (buttonManager.clickHint)
        {
            adsHint = true;
            buttonManager.clickHint = false;
        }
        if (buttonManager.clickSkip)
        {
            Invoke("DelaySpawnLevel", 0.5f);
            buttonManager.clickSkip = false;
        }
    }

    private void DelaySpawnLevel()
    {
        adsSkip = true;
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show((GoogleMobileAds.Api.Reward reward) => { });
        }
    }
    #endregion

    public bool hasLoadOpenAds()
    {
        if (appOpenAd.CanShowAd())
            return true;
        return false;
    }

    public bool hasLoadIntersAds()
    {
        if (interstitialAd.CanShowAd())
            return true;
        return false;
    }

}
