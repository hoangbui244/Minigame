using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAdsApplovin : MonoBehaviour
{
#if UNITY_IOS
    private string _adUnitId = "";
#else // UNITY_ANDROID
    private string _adUnitId = "";
#endif

    public string AdUnitId
    {
        private get => _adUnitId;
        set => _adUnitId = value;
    }
    
    private event Action<bool> ShowOpen;

    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
        };
    }

    private void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(_adUnitId);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            ShowAppOpenAd();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ShowAppOpenAd(Action<bool> completed = null)
    {
        if (MaxSdk.IsAppOpenAdReady(_adUnitId))
        {
            ShowOpen = completed;
            MaxSdk.ShowAppOpenAd(_adUnitId);
            ShowOpen?.Invoke(true);
            ShowOpen = null;
            MaxSdk.LoadAppOpenAd(_adUnitId);
#if UNITY_EDITOR
            Debug.Log("Showing App Open Ad");
#endif
        }
        else
        {
            ShowOpen?.Invoke(false);
            ShowOpen = null;
            MaxSdk.LoadAppOpenAd(_adUnitId);
#if UNITY_EDITOR
            Debug.LogError("App open ad is not ready yet.");
#endif
        }
    }
}