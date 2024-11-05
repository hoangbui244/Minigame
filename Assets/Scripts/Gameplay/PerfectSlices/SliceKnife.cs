using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliceKnife : MonoBehaviour
{
    [SerializeField] private List<Vector2> _points;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private SpriteRenderer _spriteRenderer;
    private int _order;
    private int _currentScore;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _order = _spriteRenderer.sortingOrder;
        _currentScore = 0;
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

                    if (!isPointerOverUI && touch.phase == TouchPhase.Began)
                    {
                        Chop();
                        UpdateOrder();
                        _currentScore++;
                        _scoreText.text = _currentScore.ToString();
                    }
                }
            }
        }
    }
    
    private void Chop()
    {
        
    }
    
    private void UpdateOrder()
    {
        _order -= 2;
        _spriteRenderer.sortingOrder = _order;
    }
}