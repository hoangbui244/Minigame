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
        ShowOpen?.Invoke(true);
        ShowOpen = null;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus && !ResourceManager.RemoveAds && AdsManager.Instance.VersionTrue)
        {
            ShowAppOpenAd();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ShowAppOpenAd(Action<bool> completed = null)
    {
        if (!ResourceManager.RemoveAds && AdsManager.Instance.VersionTrue)
        {
            if (MaxSdk.IsAppOpenAdReady(_adUnitId))
            {
                ShowOpen = completed;
                MaxSdk.ShowAppOpenAd(_adUnitId);
            }
            else
            {
                completed?.Invoke(false);
                MaxSdk.LoadAppOpenAd(_adUnitId);
            }
        }
    }
}