using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCandyCheck : MonoBehaviour
{
    [SerializeField] private int _winCount;
    [SerializeField] private GameObject _pen;
    [SerializeField] private GameObject _text;
    [SerializeField] private GameObject _anim;
    private int _currentCount = 0;
    private WaitForSeconds _wait = new WaitForSeconds(0.8f);
    private WaitForSeconds _wait1 = new WaitForSeconds(0.15f);
    
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
                if (ResourceManager.BreakCandy < 10)
                {
                    ResourceManager.BreakCandy++;
                }
                else
                {
                    ResourceManager.BreakCandy = 1;
                }
                GameManager.Instance.GamePause(true);
                OffPen();
                _text.SetActive(false);
                ObjectPooler.Instance.MoveToPool();
                GameUIManager.Instance.CompletedLevel1(true);
            }
        }
    }
    
    private void OffPen()
    {
        _pen.SetActive(false);
    }
    
    private IEnumerator Retry()
    {
        yield return _wait1;
        AudioManager.PlaySound("CandyBreakAll");
        _anim.SetActive(true);
        yield return _wait;
        GameUIManager.Instance.Retry(true);
    }
}
