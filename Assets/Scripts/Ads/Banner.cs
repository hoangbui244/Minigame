using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class Banner : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "";
#elif UNITY_IPHONE
  private string _adUnitId = "";
#elif UNITY_EDITOR
    private string _adUnitId = "";
#endif

    private BannerView _bannerView;

    public string AdUnitId
    {
        private get => _adUnitId;
        set => _adUnitId = value;
    }

    private void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyBannerView();
        }
        
        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    public void LoadAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Show();
            // if (SceneManager.GetActiveScene().name == "GamePlay")
            // {
            //     GameUIManager.Instance.UpdateBannerUI(true);
            // }
        }

        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest(); 
        
        if (SceneManager.GetActiveScene().name == "Special")
        {
            adRequest.Extras.Add("collapsible", "bottom");
        }

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
        ListenToAdEvents();
    }

    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.LogError("Banner view loaded an ad with response : "
                           + _bannerView.GetResponseInfo());
            Debug.LogError(_bannerView.IsCollapsible()
                ? "Banner is collapsible."
                : "Banner is not collapsible.");
            // if (SceneManager.GetActiveScene().name == "HomeScreen")
            // {
            //     HomeUIManager.Instance.UpdateBannerUI(true);
            // }
            // else if (SceneManager.GetActiveScene().name == "GamePlay")
            // {
            //     GameUIManager.Instance.UpdateBannerUI(true);
            // }
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                           + error);
            _bannerView = null;
            // if (SceneManager.GetActiveScene().name == "HomeScreen")
            // {
            //     HomeUIManager.Instance.UpdateBannerUI(false);
            // }
            // else if (SceneManager.GetActiveScene().name == "GamePlay")
            // {
            //     GameUIManager.Instance.UpdateBannerUI(false);
            // }
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () => { Debug.Log("Banner view recorded an impression."); };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () => { Debug.Log("Banner view was clicked."); };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () => { Debug.Log("Banner view full screen content opened."); };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () => { Debug.Log("Banner view full screen content closed."); };
    }

    private void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    private void OnDestroy()
    {
        DestroyBannerView();
    }

    private void OnEnable()
    {
        GameEventManager.PurchaseAds += DestroyBannerView;
    }

    private void OnDisable()
    {
        GameEventManager.PurchaseAds -= DestroyBannerView;
    }
}