using System;
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

    private void OnEnable()
    {
        GameEventManager.LoadLevel += LoadLevel;
    }
    
    private void OnDisable()
    {
        GameEventManager.LoadLevel -= LoadLevel;
    }

    #endregion

    #region =========================== MAIN ===========================

    private void LoadLevel(int type)
    {
        switch (type)
        {
            case 1:
                var num = ResourceManager.FlipCard;
                var level = ResourceManager.GetLevel(Level.LevelType.FlipCard ,num);
                _level = Instantiate(level, transform);
                break;
            case 2:
                var num2 = ResourceManager.BallBreaker;
                var level2 = ResourceManager.GetLevel(Level.LevelType.BallBreaker ,num2);
                _level = Instantiate(level2, transform);
                break;
            case 3:
                var num3 = ResourceManager.TapAway;
                var level3 = ResourceManager.GetLevel(Level.LevelType.TapAway ,num3);
                _level = Instantiate(level3, transform);
                break;
            case 4:
                var num4 = ResourceManager.PieceTogether;
                var level4 = ResourceManager.GetLevel(Level.LevelType.PieceTogether ,num4);
                _level = Instantiate(level4, transform);
                break;
            case 5:
                var num5 = ResourceManager.BreakCandy;
                var level5 = ResourceManager.GetLevel(Level.LevelType.BreakCandy ,num5);
                _level = Instantiate(level5, transform);
                break;
            case 6:
                var num6 = ResourceManager.FindDifference;
                var level6 = ResourceManager.GetLevel(Level.LevelType.FindDifference ,num6);
                _level = Instantiate(level6, transform);
                break;
            case 7:
                var num7 = ResourceManager.CrocodileDentist;
                var level7 = ResourceManager.GetLevel(Level.LevelType.CrocodileDentist ,num7);
                _level = Instantiate(level7, transform);
                break;
            case 8:
                var num8 = ResourceManager.CutInHalf;
                var level8 = ResourceManager.GetLevel(Level.LevelType.CutInHalf ,num8);
                _level = Instantiate(level8, transform);
                break;
            case 9:
                var num9 = ResourceManager.DefuseBomb;
                var level9 = ResourceManager.GetLevel(Level.LevelType.DefuseBomb ,num9);
                _level = Instantiate(level9, transform);
                break;
            case 10:
                var num10 = ResourceManager.FruitCutting;
                var level10 = ResourceManager.GetLevel(Level.LevelType.FruitCutting ,num10);
                _level = Instantiate(level10, transform);
                break;
        }
    }
    
    #endregion
}
