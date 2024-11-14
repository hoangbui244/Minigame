using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliceKnife : MonoBehaviour
{
    [SerializeField] private List<Vector2> _points;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private int _orderNum;
    private SpriteRenderer _spriteRenderer;
    private int _order;
    private int _currentScore;
    private int _pointsCount = 1;
    private bool _isChopping = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = _orderNum;
        _order = _orderNum;
        _currentScore = 0;
        Reset();
    }

    private void Reset()
    {
        transform.position = _points[0];
        _spriteRenderer.sortingOrder = _orderNum;
        _order = _orderNum;
        _pointsCount = 1;
        _isChopping = false;
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        isPointerOverUI = true;
                        break;
                    }

                    if (!isPointerOverUI && touch.phase == TouchPhase.Began && !_isChopping)
                    {
                        Chop();
                        _currentScore++;
                        _scoreText.text = _currentScore.ToString();
                    }
                }
            }
        }
    }
    
    private void Chop()
    {
        _isChopping = true;
        transform.DOMoveY(_points[_pointsCount].y, 0.05f).OnComplete(() =>
        {
            _pointsCount++;
            GameEventManager.PerfectSlices?.Invoke(_pointsCount);
            transform.DOMoveY(_points[_pointsCount].y, 0.05f).OnComplete(() =>
            {
                _pointsCount++;
                UpdateOrder();
                if (_pointsCount == _points.Count)
                {
                    Reset();
                    GameEventManager.PerfectSlicesReset?.Invoke();
                }
                _isChopping = false;
            });
        });
    }
    
    private void UpdateOrder()
    {
        if (_order >= 2)
        {
            _order -= 2;
            _spriteRenderer.sortingOrder = _order;
        }
    }
}