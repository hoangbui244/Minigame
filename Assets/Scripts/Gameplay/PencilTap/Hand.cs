using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour
{
    [SerializeField] private float _moveTime = 1f;
    [SerializeField] private Vector2 _startPosX;
    [SerializeField] private Vector2 _endPosX;
    private Vector2 _startPosY;
    private Vector2 _endPosY;
    private readonly Vector2 _plusY = new Vector2(0, 15f);
    private readonly Vector2 _end = new Vector2(0, 2.1f);

    private bool _isHorizontal = true;
    private bool _canMove = true;
    private bool _done;
    private Tween _horizontalTween;
    private Tween _verticalTween;

    private void Start()
    {
        _horizontalTween = transform.DOMoveX(_endPosX.x, _moveTime * 2.2f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .Pause();

        Move();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() &&
            !MainUIMananger.Instance.PopupOpened && _canMove)
        {
            _canMove = false;
            AudioManager.PlaySound("Click");
            ToggleMovement();
        }
    }

    private void Move()
    {
        if (_isHorizontal)
        {
            _horizontalTween.Play();
        }
        else
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        _horizontalTween.Pause();
        _verticalTween = transform.DOMoveY(_endPosY.y, _moveTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                if (_done)
                {
                    AudioManager.PlaySound("PencilDone");
                    AnimDone();
                }
                else
                {
                    MoveDown();
                }
            });
    }

    private void AnimDone()
    {
        var position = transform.position;
        position.x = 0.65f;
        transform.position = position;
        
        transform.DOMoveY(transform.position.y + _end.y, _moveTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                GameEventManager.PencilTap?.Invoke(1);
                Invoke(nameof(Reset), 0.6f);
            });
    }
    
    private void MoveDown()
    {
        _verticalTween = transform.DOMoveY(_startPosY.y, _moveTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _isHorizontal = true;
                _canMove = true;
                Move();
            });
    }

    private void ToggleMovement()
    {
        _isHorizontal = !_isHorizontal;

        if (!_isHorizontal)
        {
            _startPosY = transform.position;
            _endPosY = _startPosY + _plusY;

            MoveUp();
        }
        else
        {
            Move();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _done = true;
    }
    
    private void Reset()
    {
        _done = false;
        _isHorizontal = true;
        transform.position = _startPosX;
        _canMove = true;

        _horizontalTween?.Restart();
        _verticalTween?.Kill();

        Move();
    }
}