using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCandyCheck : MonoBehaviour
{
    [SerializeField] private int _winCount;
    [SerializeField] private GameObject _pen;
    [SerializeField] private GameObject _text;
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
                GameManager.Instance.GamePause(true);
                OffPen();
                _text.SetActive(false);
                ObjectPooler.Instance.MoveToPool();
                StartCoroutine(NextLevel());
            }
        }
    }
    
    private IEnumerator NextLevel()
    {
        GameUIManager.Instance.CompletedLevel1(true);
        yield return _wait;
        GameUIManager.Instance.Reload();
    }
    
    private void OffPen()
    {
        _pen.SetActive(false);
    }
    
    private IEnumerator Retry()
    {
        yield return _wait;
        GameUIManager.Instance.Retry(true);
    }
}
