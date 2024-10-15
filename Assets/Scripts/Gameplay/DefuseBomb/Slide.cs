using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [SerializeField] private Transform _whiteSlider;
    [SerializeField] private Transform _greenZone;   
    [SerializeField] private Vector3 _startPos;        
    [SerializeField] private Vector3 _endPos;         
    [SerializeField] private float _time;              
    [SerializeField] private bool _isFinished;        
    private Tween _moveTween;                          

    private void Start()
    {
        Move();
    }

    private void Move()
    {
        _moveTween = _whiteSlider.DOMove(_endPos, _time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void Stop()
    {
        if (_moveTween != null && _moveTween.IsPlaying())
        {
            _moveTween.Kill();
            if (IsInGreenZone())
            {
                Debug.LogError("Success");
                _isFinished = true;
            }
        }
    }

    private bool IsInGreenZone()
    {
        return _whiteSlider.position.x >= _greenZone.position.x - (_greenZone.localScale.x / 2) &&
               _whiteSlider.position.x <= _greenZone.position.x + (_greenZone.localScale.x / 2);
    }

    private void Update()
    {
        if (!_isFinished && !MainUIMananger.Instance.PopupOpened)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Stop();
            }
        }
    }
}
