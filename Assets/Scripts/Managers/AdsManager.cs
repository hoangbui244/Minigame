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
    [SerializeField] private string _rewardedAdmobID = "";
    [SerializeField] private string _interstitialAdmobID = "";
    [SerializeField] private string _bannerAdmobID = "";
    [SerializeField] private string _openAdsAdmobID = "";
    [SerializeField] private string _nativeAdsAdmobID = "";

    [Header("Applovin")]
    [SerializeField] private string _bannerID = "";
    [SerializeField] private string _openAdsID = "";
    [SerializeField] private string _nativeAdsID = "";
    [SerializeField] private string _rewardedID = "";
    [SerializeField] private string _interstitialID = "";

    [Header("Other")]
    [SerializeField] private bool _testMode;
    private readonly string _testBannerID = "ca-app-pub-3940256099942544/2014213617";
    private readonly string _testOpenID = "ca-app-pub-3940256099942544/9257395921";
    private readonly string _testNativeID = "ca-app-pub-3940256099942544/2247696110";
    private bool _initialized;
    private bool _openAds;
    private RemoteConfigManager Remote => RemoteConfigManager.Instance;
    private float IntersCapping => Remote.IntersCapping;
    private float BreakCapping => Remote.BreakCapping;
    private bool _canShowBreak = false;
    private bool _canShowInters = false;

    public bool CanShowBreak
    {
        get => _canShowBreak;
        set
        {
            if (!value)
            {
                Invoke(nameof(BreakTimeCapping), BreakCapping);
            }
            _canShowBreak = value;
        }
    }
    
    public bool CanShowInters
    {
        get => _canShowInters;
        set
        {
            if (!value)
            {
                Invoke(nameof(TimeCapping), IntersCapping);
            }
            _canShowInters = value;
        }
    }
    
    [SerializeField] private BannerApplovin _banner;
    [SerializeField] private OpenAdsApplovin _appOpenAd;
    [SerializeField] private InterstitialApplovin _interstitial;
    [SerializeField] private RewardedApplovin _rewarded;
    public NativeAd LoadedNativeAd { get; private set; }
    public event Action OnNativeAdLoaded;

    #endregion

    #region =========================== UNITY CORES ===========================

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Debug.LogError("CanShowBreak: " + CanShowBreak);
    }

    #endregion

    #region =========================== MAIN ===========================

    private void Init()
    {
        if (_initialized) return;
        
        InitAdsID();
        MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { "5948dbcb-daf2-4012-9b25-d8110a3f32c6", "a098332a-53f2-4d83-a556-1b60e54561c1", "d13d9e78-313b-4c35-a08c-e4d1fc10306e"});
        MaxSdk.InitializeSdk();
        // MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        // {
        //     MaxSdk.ShowMediationDebugger();
        // };
        _interstitial.Init();
        _banner.Init();
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
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        LoadedNativeAd = args.nativeAd;
        OnNativeAdLoaded?.Invoke();
    }
    
    public void ShowBanner()
    {
        if (ResourceManager.RemoveAds) return;
        _banner.ShowBanner();
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
            CanShowInters = false;
        });
    }
    
    public void ShowAdBreak(Action<bool> completed = null)
    {
        if (ResourceManager.RemoveAds)
        {
            completed?.Invoke(false);
            return;
        }
        _interstitial.ShowInterstitial(success =>
        {
            completed?.Invoke(success);
            CanShowBreak = false;
        });
    }
    
    public void ShowRewarded(Action<bool> completed = null)
    {
        _rewarded.ShowReward(completed);
    }
    
    private void BreakTimeCapping()
    {
        CanShowBreak = true;
    }
    
    private void TimeCapping()
    {
        CanShowInters = true;
    }

    #endregion
}
