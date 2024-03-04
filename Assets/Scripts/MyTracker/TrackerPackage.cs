using Mycom.Tracker.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerPackage : MonoBehaviour
{
    public void Awake()
    {
#if !UNITY_IOS && !UNITY_ANDROID
        return;
#endif

        // Setting up the configuration if needed
        var myTrackerConfig = MyTracker.MyTrackerConfig;
        // ...
        // Setting up params
        // ...

        // Initialize the tracker
#if UNITY_IOS
        MyTracker.Init("SDK_KEY_IOS");
#elif UNITY_ANDROID
        MyTracker.Init("30967151382800742149");
#endif
    }
}
