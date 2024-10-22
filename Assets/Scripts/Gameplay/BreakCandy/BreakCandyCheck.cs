using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCandyCheck : MonoBehaviour
{
    [SerializeField] private int _winCount;
    private int _currentCount = 0;
    private WaitForSeconds _wait = new WaitForSeconds(0.8f);
    
    private void OnEnable()
    {
        GameEventManager.BreakCandy += UpdateResult;
    }
    
    private void OnDisable()
    {
        GameEventManager.BreakCandy -= UpdateResult;
    }
    
    private void UpdateResult(bool result)
    {
        if (!result)
        {
            StartCoroutine(Retry());
        }
        else
        {
            _currentCount++;
            if (_currentCount >= _winCount)
            {
                ResourceManager.BreakCandy++;
                StartCoroutine(NextLevel());
            }
        }
    }
    
    private IEnumerator Retry()
    {
        yield return _wait;
        GameUIManager.Instance.Retry(true);
    }
    
    private IEnumerator NextLevel()
    {
        yield return _wait;
        GameUIManager.Instance.CompletedLevel(true);
    }
}
