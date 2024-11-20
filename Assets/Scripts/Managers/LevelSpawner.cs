using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : Singleton<LevelSpawner>
{
    #region =========================== PROPERTIES ===========================
    
    private bool _startLoad;
    private Level _level;
    public bool CanTap = true;
    private WaitForSeconds _wait = new (0.15f);

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
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "FlipCard");
                break;
            case 2:
                var num2 = ResourceManager.BallBreaker;
                var level2 = ResourceManager.GetLevel(Level.LevelType.BallBreaker ,num2);
                _level = Instantiate(level2, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "BallBreaker");
                break;
            case 3:
                var num3 = ResourceManager.TapAway;
                var level3 = ResourceManager.GetLevel(Level.LevelType.TapAway ,num3);
                _level = Instantiate(level3, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "TapAway");
                break;
            case 4:
                var num4 = ResourceManager.PieceTogether;
                var level4 = ResourceManager.GetLevel(Level.LevelType.PieceTogether ,num4);
                _level = Instantiate(level4, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "PieceTogether");
                break;
            case 5:
                var num5 = ResourceManager.BreakCandy;
                var level5 = ResourceManager.GetLevel(Level.LevelType.BreakCandy ,num5);
                _level = Instantiate(level5, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "BreakCandy");
                break;
            case 6:
                var level6 = ResourceManager.GetLevel(Level.LevelType.FindDifference ,1);
                _level = Instantiate(level6, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "FindDifference");
                break;
            case 7:
                var num7 = ResourceManager.CrocodileDentist;
                var level7 = ResourceManager.GetLevel(Level.LevelType.CrocodileDentist ,num7);
                _level = Instantiate(level7, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "CrocodileDentist");
                break;
            case 8:
                var num8 = ResourceManager.CutInHalf;
                var level8 = ResourceManager.GetLevel(Level.LevelType.CutInHalf ,num8);
                _level = Instantiate(level8, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "CutInHalf");
                break;
            case 9:
                var num9 = ResourceManager.DefuseBomb;
                var level9 = ResourceManager.GetLevel(Level.LevelType.DefuseBomb ,num9);
                _level = Instantiate(level9, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "DefuseBomb");
                break;
            case 10:
                var num10 = ResourceManager.FruitCutting;
                var level10 = ResourceManager.GetLevel(Level.LevelType.FruitCutting ,num10);
                _level = Instantiate(level10, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "FruitCutting");
                break;
            case 11:
                var num11 = ResourceManager.PerfectSlices;
                var level11 = ResourceManager.GetLevel(Level.LevelType.PerfectSlices ,num11);
                _level = Instantiate(level11, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "PerfectSlices");
                break;
            case 12:
                var num12 = ResourceManager.PassBomb;
                var level12 = ResourceManager.GetLevel(Level.LevelType.PassBomb ,num12);
                _level = Instantiate(level12, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "PassBomb");
                break;
            case 13:
                var num13 = ResourceManager.OneLine;
                var level13 = ResourceManager.GetLevel(Level.LevelType.OneLine ,num13);
                _level = Instantiate(level13, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "OneLine");
                break;
            case 14:
                var num14 = ResourceManager.CatchEgg;
                var level14 = ResourceManager.GetLevel(Level.LevelType.CatchEgg ,num14);
                _level = Instantiate(level14, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "CatchEgg");
                break;
            case 15:
                var level15 = ResourceManager.GetLevel(Level.LevelType.PencilTap ,1);
                _level = Instantiate(level15, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "PencilTap");
                break;
            case 16:
                var num16 = ResourceManager.DropCandy;
                var level16 = ResourceManager.GetLevel(Level.LevelType.DropCandy ,num16);
                _level = Instantiate(level16, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "DropCandy");
                break;
            case 17:
                var num17 = ResourceManager.BalanceEgg;
                var level17 = ResourceManager.GetLevel(Level.LevelType.BalanceEgg ,num17);
                _level = Instantiate(level17, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "BalanceEgg");
                break;
            case 18:
                var num18 = ResourceManager.PetalCount;
                var level18 = ResourceManager.GetLevel(Level.LevelType.PetalCount ,num18);
                _level = Instantiate(level18, transform);
                FirebaseManager.Instance.LogEventNameWithParam("play_game_type", "Game Type", "PetalCount");
                break;
        }
    }
    
    public void ResetTap()
    {
        StartCoroutine(ResetTapCoroutine());
    }
    
    private IEnumerator ResetTapCoroutine()
    {
        CanTap = false;
        yield return _wait;
        CanTap = true;
    }
    
    #endregion
}
