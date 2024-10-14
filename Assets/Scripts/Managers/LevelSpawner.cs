using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : Singleton<LevelSpawner>
{
    #region =========================== PROPERTIES ===========================
    
    private bool _startLoad;
    private Level _level;

    #endregion

    #region =========================== UNITY CORES ===========================

    private void Update()
    {
        if (_startLoad || !ResourceManager.IsLoaded) return;
        LoadLevel();
        _startLoad = true;
    }

    #endregion

    #region =========================== MAIN ===========================

    // ReSharper disable Unity.PerformanceAnalysis
    private void LoadLevel()
    {
        var num = ResourceManager.FlipCard;
        var level = ResourceManager.GetLevel(Level.LevelType.FlipCard ,num);
        _level = Instantiate(level, transform);
    }
    
    #endregion
}
