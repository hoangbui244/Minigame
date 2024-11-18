using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class OpenAds : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "";
#elif UNITY_IPHONE
    string _adUnitId = "";
#elif UNITY_EDITOR
    private string _adUnitId = "";
#endif

    private DateTime _expireTime;
    private AppOpenAd _appOpenAd;
    private event Action<bool> ShowOpen;
    
    public string AdUnitId
    {
        private get => _adUnitId;
        set => _adUnitId = value;
    }
    
    public bool IsAdAvailable => _appOpenAd != null && _appOpenAd.CanShowAd() && DateTime.Now < _expireTime;

    private void OnAppStateChanged(AppState state)
    {
        if (state == AppState.Foreground && !ResourceManager.RemoveAds)
        {
            if (IsAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }

    private void LoadAppOpenAd()
    {
        // Clean up the old ad before loading a new one.
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_adUnitId, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());
                
                _expireTime = DateTime.Now + TimeSpan.FromHours(4);
                _appOpenAd = ad;
                RegisterEventHandlers(ad);
            });
    }
    
    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            ShowOpen?.Invoke(true);
            ShowOpen = null;
            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            LoadAppOpenAd();
        };
    }
    
    public void ShowAppOpenAd(Action<bool> completed = null)
    {
        ShowOpen = completed;
        if (_appOpenAd != null && _appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            _appOpenAd.Show();
        }
        else
        {
            ShowOpen?.Invoke(false);
            ShowOpen = null;
            Debug.LogError("App open ad is not ready yet.");
        }
    }

    private void DestroyAd()
    {
        if (_appOpenAd != null)
        {
            Debug.Log("Destroying banner ad.");
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }
    }
    
    public void Init()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadAppOpenAd();
        });
    }

    private void OnDestroy()
    {
        DestroyAd();
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }
}
