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

    public static int BallBreaker
    {
        get => PlayerPrefs.GetInt("BallBreaker", 1);
        set => PlayerPrefs.SetInt("BallBreaker", value);
    }

    public static int TapAway
    {
        get => PlayerPrefs.GetInt("TapAway", 1);
        set => PlayerPrefs.SetInt("TapAway", value);
    }
    
    public static int PieceTogether
    {
        get => PlayerPrefs.GetInt("PieceTogether", 1);
        set => PlayerPrefs.SetInt("PieceTogether", value);
    }
    
    public static int BreakCandy
    {
        get => PlayerPrefs.GetInt("BreakCandy", 1);
        set => PlayerPrefs.SetInt("BreakCandy", value);
    }
    
    public static int FindDifference
    {
        get => PlayerPrefs.GetInt("FindDifference", 0);
        set => PlayerPrefs.SetInt("FindDifference", value);
    }
    
    public static int CrocodileDentist
    {
        get => PlayerPrefs.GetInt("CrocodileDentist", 1);
        set => PlayerPrefs.SetInt("CrocodileDentist", value);
    }
    
    public static int CutInHalf
    {
        get => PlayerPrefs.GetInt("CutInHalf", 1);
        set => PlayerPrefs.SetInt("CutInHalf", value);
    }
    
    public static int DefuseBomb
    {
        get => PlayerPrefs.GetInt("DefuseBomb", 1);
        set => PlayerPrefs.SetInt("DefuseBomb", value);
    }
    
    public static int FruitCutting
    {
        get => PlayerPrefs.GetInt("FruitCutting", 1);
        set => PlayerPrefs.SetInt("FruitCutting", value);
    }
    
    public static int PerfectSlices
    {
        get => PlayerPrefs.GetInt("PerfectSlices", 1);
        set => PlayerPrefs.SetInt("PerfectSlices", value);
    }
    
    public static int HighScore
    {
        get => PlayerPrefs.GetInt("HighScore", 0);
        set => PlayerPrefs.SetInt("HighScore", value);
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