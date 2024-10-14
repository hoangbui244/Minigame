using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class ResourceManager : Singleton<ResourceManager>
{
    #region =========================== PROPERTIES ===========================

    public static bool FirstOpen
    {
        get => PlayerPrefs.GetInt("FirstOpen", 1) == 1;
        set => PlayerPrefs.SetInt("FirstOpen", value ? 1 : 0);
    }
    
    public static bool RemoveAds
    {
        get => PlayerPrefs.GetInt("RemoveAds", 0) == 1;
        set => PlayerPrefs.SetInt("RemoveAds", value ? 1 : 0);
    }
    
    public static int FlipCard
    {
        get => PlayerPrefs.GetInt("FlipCard", 1);
        set => PlayerPrefs.SetInt("FlipCard", value);
    }
    
    public static bool IsLoaded { get; set; }

    [SerializeField] private AssetLabelReference _labelName;
    private Dictionary<Level.LevelType, Dictionary<int, Level>> _levels = new();

    #endregion

    #region =========================== UNITY CORES ===========================

    private void Start()
    {
        LoadAddressable<GameObject>();
    }

    #endregion

    #region =========================== MAIN ===========================

    private void LoadAddressable<T>()
    {
        AddressablesUtility.DownloadAssetsAsync<T>(_labelName, (list) =>
        {
            if (list == null) return;
            foreach (var item in list)
            {
                var obj = item as GameObject;
                if (obj.TryGetComponent(out Level level))
                {
                    if (_levels.TryGetValue(level.Type, out var levelsByType))
                    {
                        levelsByType[level.LevelNo] = level;
                    }
                    else
                    {
                        _levels[level.Type] = new Dictionary<int, Level> { { level.LevelNo, level } };
                    }
                }
                Debug.LogError("Level Loaded" + _levels.Count + " " + "Type: " + level.Type);
            }
            IsLoaded = true;
        });
    }

    public static Level GetLevel(Level.LevelType type, int levelNo)
    {
        if (Instance?._levels.TryGetValue(type, out var levelsByType) == true && levelsByType != null)
        {
            levelsByType.TryGetValue(levelNo, out var level);
            return level;
        }
    
        return null;
    }

    #endregion
}