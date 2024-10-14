using UnityEngine;
using Firebase.Analytics;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private void Start()
    {
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }
    
    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        double revenue = impressionData.Revenue;
        var impressionParameters = new[] {
            new Parameter("ad_platform", "AppLovin"),
            new Parameter("ad_source", impressionData.NetworkName),
            new Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
            new Parameter("ad_format", impressionData.AdFormat),
            new Parameter("value", revenue),
            new Parameter("currency", "USD"),
        };
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, impressionParameters);
    }
    
    public void LogEventName(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }
        
    public void LogEventNameWithParam(string eventName, string paramName, string paramValue)
    {
        FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
    }
}
