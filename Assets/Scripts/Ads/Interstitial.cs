using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interstitial : MonoBehaviour
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

    private int _retryAttempt;
    private event Action<bool> Completed;

    public void Init()
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

    private void OnDestroy()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialAdFailedToDisplayEvent;
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(_adUnitId);
    }

    public void ShowInterstitial(Action<bool> completed = null)
    {
        if (IsInterstitialReady())
        {
            Completed = completed;
            MaxSdk.ShowInterstitial(_adUnitId);
            FirebaseManager.Instance.LogEventName("inter_attempt");
        }
    }

    private bool IsInterstitialReady()
    {
        return MaxSdk.IsInterstitialReady(_adUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        _retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        _retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, _retryAttempt));

        Invoke(nameof(LoadInterstitial), (float) retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        Completed?.Invoke(false);
        Completed = null;
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        Completed?.Invoke(true);
        Completed = null;
        LoadInterstitial();
    }
}
