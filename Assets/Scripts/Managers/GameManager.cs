using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum EnumGameState
    {
        None = 0,
        Play = 1,
        Pause = 2,
        Finish = 3
    }

    private EnumGameState _gameState;
    
    public EnumGameState GameState
    {
        get => _gameState;
        set => _gameState = value;
    }

    private void Start()
    {
        _gameState = EnumGameState.Pause;
    }
    
    public EnumGameState GamePause(bool isPause)
    {
        return GameState = isPause ? EnumGameState.Pause : EnumGameState.Play;
    }
}
