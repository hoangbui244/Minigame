using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrocodileDentistCheck : MonoBehaviour
{
    [SerializeField] private List<Tooth> _teeth;
    [SerializeField] private int _winCount;
    private int _currentWinCount = 0;
    
    private void OnEnable()
    {
        RandomTeeth();
        GameEventManager.CheckTeeth += CheckTeeth;
    }
    
    private void OnDisable()
    {
        GameEventManager.CheckTeeth -= CheckTeeth;
    }
    
    private void RandomTeeth()
    {
        int num = Random.Range(0, _teeth.Count);
        _teeth[num].IsTrapped = true;
    }
    
    private void CheckTeeth(bool isWin)
    {
        if (isWin)
        {
            _currentWinCount++;
            if (_currentWinCount >= _winCount)
            {
                GameUIManager.Instance.CompletedLevel(true);
            }
        }
        else
        {
            GameUIManager.Instance.Retry(true);
        }
    }
}
