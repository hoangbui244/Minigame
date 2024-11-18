using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using System;
using Firebase;
using Firebase.RemoteConfig;
using UnityEngine.Serialization;

public class RemoteConfigManager : Singleton<RemoteConfigManager>
{
    public float IntersCapping = 40f;
    public float BreakCapping = 40f;
    private bool _initFirebase;
    DependencyStatus _dependencyStatus = DependencyStatus.UnavailableOther;
    
    public bool InitFirebase => _initFirebase;
    
    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        FirebaseApp
            .CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                _dependencyStatus = task.Result;
                if (_dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                    _initFirebase = true;
                }
                else
                {
                    Debug.Log(
                        "Could not resolve all Firebase dependencies: " + _dependencyStatus
                    );
                }
            });
    }
    
    private void InitializeFirebase()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>();

        FirebaseRemoteConfig.DefaultInstance
            .SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                FetchDataAsync();
            });
    }

    private Task FetchDataAsync() {
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
    
    private void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed success");
            }

            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance
                        .ActivateAsync()
                        .ContinueWithOnMainThread(task =>
                        {
                            Debug.Log("Remote data loaded, last fetch time: " + info.FetchTime);
                            // Start load data from remote config
                            LoadRemoteConfigData();
                        });
                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.LogError("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.LogError("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Lasted fetch call still pending");
                    break;
            }
        }

        private void LoadRemoteConfigData()
        {
            IntersCapping = (float)FirebaseRemoteConfig.DefaultInstance
                .GetValue("inter_capping")
                .DoubleValue;
            BreakCapping = (float)FirebaseRemoteConfig.DefaultInstance
                .GetValue("ad_break_capping")
                .DoubleValue;
        }
}
