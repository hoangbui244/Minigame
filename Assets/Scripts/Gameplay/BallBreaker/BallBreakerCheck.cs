using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallBreakerCheck : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _winCount;
    private int _currentWinCount = 0;
    
    private void OnEnable()
    {
        GameEventManager.ResetLevel += Reset;
        GameEventManager.BallBreaker += Check;
    }
    
    private void OnDisable()
    {
        GameEventManager.ResetLevel -= Reset;
        GameEventManager.BallBreaker -= Check;
    }
    
    private void Start()
    {
        Reset();
    }
    
    private void Reset()
    {
        ObjectPooler.Instance.SpawnFromPool("Ball", _spawnPoint.position);
    }
    
    private void Check(bool isWin)
    {
        if (isWin)
        {
            _currentWinCount++;
            if (_currentWinCount >= _winCount)
            {
                GameUIManager.Instance.CompletedLevel(true);
            }
        }
    }
}
