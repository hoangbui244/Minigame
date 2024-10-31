using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsManager : Singleton<AdsManager>
{
    #region =========================== PROPERTIES ===========================

    [Header("Admob")]
    [SerializeField] private string _bannerID = "";
    [SerializeField] private string _openAdsID = "";
    [SerializeField] private string _nativeAdsID = "";

    [Header("Applovin")]
    [SerializeField] private string _rewardedID = "";
    [SerializeField] private string _interstitialID = "";

    [Header("Other")]
    [SerializeField] private bool _testMode;
    private readonly string _testBannerID = "ca-app-pub-3940256099942544/2014213617";
    private readonly string _testOpenID = "ca-app-pub-3940256099942544/9257395921";
    private readonly string _testNativeID = "ca-app-pub-3940256099942544/2247696110";
    private bool _initialized;
    private bool _openAds;
    private RemoteConfigManager _remote => RemoteConfigManager.Instance;
    private float _intersCapping => _remote.IntersCapping;
    private bool _canShowInters = false;
    public static int IntersCount = 0;
    
    public bool CanShowInters
    {
        get => _canShowInters;
        set
        {
            if (!value)
            {
                Invoke(nameof(TimeCapping), _intersCapping);
            }
            _canShowInters = value;
        }
    }
    
    [SerializeField] private Banner _banner;
    [SerializeField] private OpenAds _appOpenAd;
    [SerializeField] private Interstitial _interstitial;
    [SerializeField] private Rewarded _rewarded;
    public NativeAd LoadedNativeAd { get; private set; }
    public event Action OnNativeAdLoaded;

    #endregion

    #region =========================== UNITY CORES ===========================

    private void Awake()
    {
        Init();
    }

    #endregion

    #region =========================== MAIN ===========================

    private void Init()
    {
        if (_initialized) return;
        
        InitAdsID();
        MaxSdk.SetSdkKey("dQ15CD6nC7CfuD2IKhScGfRyQOoJpENkqUqftd_Xg0z83xvbcqZKQG3JTTbzUAaR8bGxPGTBufsv3sqxsXzrcV");
        MaxSdk.InitializeSdk();
        // MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        // {
        //     MaxSdk.ShowMediationDebugger();
        // };
        _interstitial.Init();
        _rewarded.Init();
        _appOpenAd.Init();
        _initialized = true;
    }

    private void InitAdsID()
    {
        _banner.AdUnitId = _testMode ? _testBannerID : _bannerID;
        _appOpenAd.AdUnitId = _testMode ? _testOpenID : _openAdsID;
        _nativeAdsID = _testMode ? _testNativeID : _nativeAdsID;
        _interstitial.AdUnitId = _interstitialID;
        _rewarded.AdUnitId = _rewardedID;
    }

    public void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(_nativeAdsID)
            .ForNativeAd()
            .Build();
        adLoader.OnNativeAdLoaded += HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
        adLoader.LoadAd(new AdRequest());
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.LogError("Native ad failed to load: " + args.LoadAdError.GetMessage());
        // if (SceneManager.GetActiveScene().name == "GamePlay")
        // {
        //     GameUIManager.Instance.EnableNative(false);
        // }
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        LoadedNativeAd = args.nativeAd;
        OnNativeAdLoaded?.Invoke();
    }
    
    public void ShowBanner()
    {
        if (ResourceManager.RemoveAds) return;
        _banner.LoadAd();
    }
    
    public void ShowOpen(Action<bool> completed = null)
    {
        if (ResourceManager.RemoveAds)
        {
            completed?.Invoke(false);
            return;
        }
        _appOpenAd.ShowAppOpenAd(completed);
    }

    public void ShowInters(Action<bool> completed = null)
    {
        if (ResourceManager.RemoveAds)
        {
            completed?.Invoke(false);
            return;
        }
        _interstitial.ShowInterstitial(success =>
        {
            completed?.Invoke(success);
            IntersCount++;
            CanShowInters = false;
        });
    }
    
    public void ShowRewarded(Action<bool> completed = null)
    {
        _rewarded.ShowReward(completed);
    }
    
    private void TimeCapping()
    {
        CanShowInters = true;
    }

    #endregion
}
