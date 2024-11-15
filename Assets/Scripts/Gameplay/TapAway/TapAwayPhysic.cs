using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TapAwayPhysic : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] private Direction _moveDir;
    [SerializeField] private float _time;
    [SerializeField] private float _moveDistance;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private Vector3 _shakeStrength = new(0.1f, 0.1f, 0);
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private LayerMask _layerMask;
    private Collider2D _col;

    private void Start()
    {
        _col = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        if (LevelSpawner.Instance.CanTap && !MainUIMananger.Instance.PopupOpened)
        {
            AudioManager.PlaySound("Click");
            LevelSpawner.Instance.ResetTap();
            Move();
        }
    }

    private void Move()
    {
        Vector3 direction = Vector3.zero;
        switch (_moveDir)
        {
            case Direction.Left:
                direction = Vector3.left;
                break;
            case Direction.Right:
                direction = Vector3.right;
                break;
            case Direction.Up:
                direction = Vector3.up;
                break;
            case Direction.Down:
                direction = Vector3.down;
                break;
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, _moveDistance, _layerMask);

        RaycastHit2D closestHit = default;
        float closestDistance = float.MaxValue;
        foreach (var hit in hits)
        {
            if (hit.collider != _col && hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestHit = hit;
            }
        }

        Vector3 targetPosition;
        if (closestHit.collider != null)
        {
            float distanceToObstacle = closestHit.distance;
            targetPosition = transform.position + direction * distanceToObstacle;
            if (_moveDir == Direction.Left)
            {
                targetPosition.x += _offsetX;
            }
            else if (_moveDir == Direction.Right)
            {
                targetPosition.x -= _offsetX;
            }
            else if (_moveDir == Direction.Up)
            {
                targetPosition.y -= _offsetY;
            }
            else if (_moveDir == Direction.Down)
            {
                targetPosition.y += _offsetY;
            }

            transform.DOMove(targetPosition, _time * (distanceToObstacle / _moveDistance))
                .OnComplete(() =>
                {
                    //AudioManager.PlayVibration(true);
                    Shake();
                });
        }
        else
        {
            _col.enabled = false;
            GameEventManager.Check?.Invoke(true);
            targetPosition = transform.localPosition + direction * _moveDistance;
            transform.DOMove(targetPosition, _time);
        }
    }


    private void Shake()
    {
        transform.DOShakePosition(shakeDuration, _shakeStrength);
    }
}