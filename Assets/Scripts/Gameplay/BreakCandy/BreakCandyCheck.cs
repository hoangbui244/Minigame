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
    [SerializeField] private List<Pieces> _pieces;
    private WaitForSeconds _wait = new WaitForSeconds(0.8f);
    private WaitForSeconds _wait1 = new WaitForSeconds(0.15f);
    private int _currentCount = 0;
    private bool Done;

    private void OnEnable()
    {
        Done = false;
        GameEventManager.BreakCandy += UpdateResult;
    }

    private void OnDisable()
    {
        GameEventManager.BreakCandy -= UpdateResult;
    }

    private void UpdateResult(bool result)
    {
        if (!result && !Done)
        {
            Done = true;
            AudioManager.PlaySound("CandyBreakAll");
            StartCoroutine(Retry());
            return;
        }
        
        _currentCount = 0;
        foreach (var item in _pieces)
        {
            if (item.Done)
            {
                _currentCount++;
            }
        }
    
        if (_currentCount >= _winCount && !Done)
        {
            Done = true; 
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
            GameUIManager.Instance.Confetti(true);
        }
    }


    private void OffPen()
    {
        _pen.SetActive(false);
    }

    private IEnumerator Retry()
    {
        yield return _wait1;
        _anim.SetActive(true);
        yield return _wait;
        GameUIManager.Instance.Retry(true);
    }
}