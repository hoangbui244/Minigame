using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBreakerController : MonoBehaviour
{
    [SerializeField] private float _minThrowDistance = 0.5f;
    private Camera _cam;
    private Vector3 _startPos;
    private Vector3 _currentPos;
    private LineRenderer _lr;
    private GameObject _spawnedBall;
    private bool _isDragging = false;

    private void Start()
    {
        _cam = Camera.main;
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameManager.EnumGameState.Finish)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                if (!_isDragging)
                {
                    _startPos = mousePos;
                    _isDragging = true;

                    GameEventManager.ResetLevel?.Invoke();
                    OnMouseDragStart();
                }

                OnMouseDrag();
            }

            if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                OnMouseDragEnd();
                _isDragging = false;
            }
        }
    }

    private void OnMouseDragStart()
    {
        RaycastHit2D hit = Physics2D.Raycast(_startPos, Vector2.zero);
        if (hit.collider == null)
        {
            _spawnedBall = ObjectPooler.Instance.SpawnFromPool("Ball", _startPos);
            _lr.enabled = true;
            _lr.SetPosition(0, _startPos); 
        }
    }

    private void OnMouseDrag()
    {
        _currentPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _currentPos.z = 0;
        _lr.SetPosition(1, _currentPos);
    }

    private void OnMouseDragEnd()
    {
        Vector3 endPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        endPos.z = 0;
        _lr.enabled = false;
        float distance = Vector3.Distance(_startPos, endPos);

        if (distance < _minThrowDistance)
        {
            _spawnedBall.SetActive(false);
        }
        else
        {
            Vector3 direction = endPos - _startPos;
            GameEventManager.ThrowBall?.Invoke(direction);
        }
    }
}
