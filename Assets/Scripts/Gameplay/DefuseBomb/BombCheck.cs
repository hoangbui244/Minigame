using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BombCheck : MonoBehaviour
{
    [SerializeField] private List<Slide> _slides;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private Vector3 _scale = new Vector3(0.12f, 0.072f, 1f);
    private WaitForSeconds _wait = new WaitForSeconds(0.3f);

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
            if (ResourceManager.DefuseBomb < 10)
            {
                ResourceManager.DefuseBomb++;
            }
            else
            {
                ResourceManager.DefuseBomb = 1;
            }
            GameUIManager.Instance.ScreenShot();
            StartCoroutine(NewLevel());
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
    
    private IEnumerator NewLevel()
    {
        yield return _wait;
        GameUIManager.Instance.CompletedLevel(true);
    }

    private IEnumerator Retry()
    {
        yield return _wait;
        GameUIManager.Instance.Retry(true);
    }
    
    private void ResetGame()
    {
        foreach (var slide in _slides)
        {
            slide.IsFinished = false;
        }
    }
}
