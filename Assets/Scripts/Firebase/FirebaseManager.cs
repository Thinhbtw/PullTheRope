using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{
    public DateTime CurrentTime;
    public List<int> listLevelShowInterAds;
    public double TimeShowInterAds;

    public bool isShowBannerAds = false;
    public bool isShowInterAds = false;
    public bool isShowRewardAds = false;

    public bool isShowInterAdsFromHome = false;
    public bool isShowInterAdsFromGameplay = false;
    public bool isShowOpenAds = false;
    public bool isOffline= false;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync().ContinueWith(x =>
            {
                string[] listStringFirebase = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("show_insterstitial_after_finished_levels").StringValue.Split(',');
                foreach (string str in listStringFirebase)
                {
                    listLevelShowInterAds.Add(int.Parse(str));
                }
                TimeShowInterAds = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("show_insterstitial_interval").DoubleValue;

                isShowBannerAds = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enable_banner_ads").BooleanValue;
                isShowInterAds = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enable_insterstitial_ads").BooleanValue;
                isShowRewardAds = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enable_reward_ads").BooleanValue;
                isShowOpenAds = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enable_open_app_ads").BooleanValue;
                isShowInterAdsFromHome = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enable_inter_ads_from_home").BooleanValue;
                isShowInterAdsFromGameplay = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enable_inter_ads_from_gameplay").BooleanValue;
            });
        });
            CurrentTime = DateTime.Now;
    }

    public void PassLevel(int num)
    {
        if(isOffline)
        {
            FirebaseAnalytics.LogEvent("User_Is_Offline_NormalMode_LV_" + num.ToString());
            return;
        }
        FirebaseAnalytics.LogEvent("NormalMode_LV_" + num.ToString());
    }

    public void SkipLevel(int num)
    {
        if (isOffline)
        {
            FirebaseAnalytics.LogEvent("User_Is_Offline_Skip_LV_" + num.ToString());
            return;
        }
        FirebaseAnalytics.LogEvent("Skip_LV_" + num.ToString());
    }

    public void WatchHintAtLevel(int num)
    {
        if (isOffline)
        {
            FirebaseAnalytics.LogEvent("User_Is_Offline_Hint_LV_" + num.ToString());
            return;
        }
        FirebaseAnalytics.LogEvent("Hint_LV_" + num.ToString());
    }

    public void PlayerIsOffline()
    {
        FirebaseAnalytics.LogEvent("User_Is_Offline");
    }
    public void PlayerIsOnline()
    {
        FirebaseAnalytics.LogEvent("User_Is_Online");
    }
}