using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using UnityEditor;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    #region =========================== PROPERTIES ===========================

    [Header("Admob")] 
    // [SerializeField] private string _rewardedAdmobID = "";
    // [SerializeField] private string _interstitialAdmobID = "";
    // [SerializeField] private string _bannerAdmobID = "";
    // [SerializeField] private string _openAdsAdmobID = "";
    [SerializeField] private string _nativeAdsAdmobID = "";

    [Header("Applovin")] 
    [SerializeField] private string _bannerID = "";
    [SerializeField] private string _openAdsID = "";
    // [SerializeField] private string _nativeAdsID = "";
    [SerializeField] private string _rewardedID = "";
    [SerializeField] private string _interstitialID = "";

    [Header("Other")] 
    public bool VersionTrue;
    [SerializeField] private bool _testMode;
    private readonly string _testBannerID = "ca-app-pub-3940256099942544/2014213617";
    private readonly string _testOpenID = "ca-app-pub-3940256099942544/9257395921";
    private readonly string _testNativeID = "ca-app-pub-3940256099942544/2247696110";
    private bool _initialized;
    private bool _openAds;
    private RemoteConfigManager Remote => RemoteConfigManager.Instance;
    private float IntersCapping => Remote.IntersCapping;
    private float StartCapping => Remote.StartCapping;
    private float BundleVersion => Remote.BundleVersion;
    private bool _canShowInters;
    private bool _canShowStart;
    private bool _switchAds;
    private int _versionCode;

    public bool StartCappingAds
    {
        get => _canShowStart;
        set
        {
            if (!value)
            {
                StartCoroutine(StartCappingCoroutine());
            }

            _canShowStart = value;
        }
    }

    public bool CanShowInters
    {
        get => _canShowInters;
        private set
        {
            if (!value)
            {
                StartCoroutine(TimeCappingCoroutine());
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

    #endregion

    #region =========================== MAIN ===========================

    private void Init()
    {
        if (_initialized) return;

        Remote.OnRemoteConfigFetched += OnRemoteConfigFetched;
        InitAdsID();
        MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[]
        {
            "5948dbcb-daf2-4012-9b25-d8110a3f32c6", "a098332a-53f2-4d83-a556-1b60e54561c1",
        });
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

        RequestNativeAd();
    }
    
    private void OnRemoteConfigFetched()
    {
        Remote.OnRemoteConfigFetched -= OnRemoteConfigFetched;
        VersionTrue = GetVersionCode();
    }

    private bool GetVersionCode()
    {
#if UNITY_EDITOR
        _versionCode = PlayerSettings.Android.bundleVersionCode;
#endif

#if UNITY_ANDROID
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (var packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager"))
                {
                    string packageName = currentActivity.Call<string>("getPackageName");
                    using (var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0))
                    {
                        _versionCode = packageInfo.Get<int>("versionCode");
                    }
                }
            }
        }
#endif

        if (_versionCode > BundleVersion)
        {
            return false;
        }

        return true;
    }

    private void InitAdsID()
    {
        _nativeAdsAdmobID = _testMode ? _testNativeID : _nativeAdsAdmobID;
        _banner.AdUnitId = _bannerID;
        _appOpenAd.AdUnitId = _openAdsID;
        _interstitial.AdUnitId = _interstitialID;
        _rewarded.AdUnitId = _rewardedID;
    }

    public void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(_nativeAdsAdmobID)
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
        if (ResourceManager.RemoveAds || !VersionTrue) return;
        _banner.ShowBanner();
    }

    public void HideBanner()
    {
        _banner.HideBanner();
        _banner.DestroyBanner();
    }

    public void ShowOpen(Action<bool> completed = null)
    {
        if (ResourceManager.RemoveAds || !VersionTrue)
        {
            completed?.Invoke(false);
            return;
        }

        _appOpenAd.ShowAppOpenAd(completed);
    }

    public void ShowInters(Action<bool> completed = null)
    {
        if (ResourceManager.RemoveAds || !VersionTrue)
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

    public void ShowRewarded(Action<bool> completed = null)
    {
        _rewarded.ShowReward(completed);
    }

    private void StartCount()
    {
        if (!_switchAds)
        {
            _switchAds = true;
            CanShowInters = true;
        }
    }

    private void TimeCapping()
    {
        CanShowInters = true;
    }

    #endregion

    #region =========================== COROUTINES ===========================

    private IEnumerator StartCappingCoroutine()
    {
        yield return new WaitForSeconds(StartCapping);
        StartCount();
    }

    private IEnumerator TimeCappingCoroutine()
    {
        yield return new WaitForSeconds(IntersCapping);
        TimeCapping();
    }

    #endregion
}