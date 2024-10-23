using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Slide : MonoBehaviour
{
    [SerializeField] private Transform _whiteSlider;
    [SerializeField] private Transform _greenZone;   
    [SerializeField] private Vector3 _startPos;        
    [SerializeField] private Vector3 _endPos;         
    [SerializeField] private float _time;              
    [SerializeField] private GameObject _mask;
    public bool IsFinished;
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
                IsFinished = true;
                _mask.SetActive(false);
                gameObject.SetActive(false);
                GameEventManager.DefuseBomb?.Invoke(true);
            }
            else
            {
                GameEventManager.DefuseBomb?.Invoke(false);
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
        if (!IsFinished && !MainUIMananger.Instance.PopupOpened)
        {
            bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isPointerOverUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }
            if (!isPointerOverUI)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Stop();
                }
            }
        }
    }
}
