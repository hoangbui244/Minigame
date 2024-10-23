using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BombCheck : MonoBehaviour
{
    [SerializeField] private List<Slide> _slides;
    [SerializeField] private TextMeshProUGUI _timeTxt;
    [SerializeField] private int _startTime = 30;
    private WaitForSeconds _wait = new WaitForSeconds(1f);

    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private Vector3 _scale = new Vector3(0.12f, 0.072f, 1f);

    private void OnEnable()
    {
        GameEventManager.DefuseBomb += UpdateResult;
    }
    
    private void OnDisable()
    {
        GameEventManager.DefuseBomb -= UpdateResult;
    }
    
    private void Start()
    {
        ResetGame();
        StartCoroutine(TimeCount());
    }
    
    private void UpdateResult(bool action)
    {
        if (action)
        {
            foreach (var slide in _slides)
            {
                if (!slide.IsFinished)
                {
                    slide.gameObject.SetActive(true);
                    return;
                }
            }
            GameUIManager.Instance.CompletedLevel(true);
        }
        else
        {
            _bomb.SetActive(true);
            _explosion.SetActive(true);
            _explosion.transform.DOScale(_scale, 0.6f).OnComplete(() =>
            {
                StartCoroutine(Retry());
            });
        }
    }

    private IEnumerator Retry()
    {
        yield return new WaitForSeconds(0.3f);
        GameUIManager.Instance.Retry(true);
    }
    
    private void ResetGame()
    {
        foreach (var slide in _slides)
        {
            slide.IsFinished = false;
        }
    }
    
    private IEnumerator TimeCount()
    {
        int currentTime = _startTime;

        while (currentTime > 0)
        {
            _timeTxt.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss");
            yield return _wait;
            currentTime--;
        }

        _timeTxt.text = "00:00"; 
        GameUIManager.Instance.Retry(true);
    }
}
