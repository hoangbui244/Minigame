using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _minThrowDistance = 0.5f;
    private Vector3 _spawnPoint = new Vector3(0, -8.5f, 0);
    private Rigidbody2D _rb;
    private Camera _cam;
    private Vector3 _startPos;
    private Vector3 _currentPos;
    private LineRenderer _lr;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        _cam = Camera.main;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !MainUIMananger.Instance.PopupOpened)
        {
            // AudioManager.PlaySound("PickUp");
            // AudioManager.PlayVibration(true);
            _startPos = transform.position;
            _lr.enabled = true;
            _lr.SetPosition(0, _startPos);
        }
    }

    private void OnMouseDrag()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            _currentPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            _currentPos.z = 0;
            _lr.SetPosition(1, _currentPos);
        }
    }

    private void OnMouseUp()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            _lr.enabled = false;

            Vector3 endPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0;
            float distance = Vector3.Distance(_startPos, endPos);

            if (distance >= _minThrowDistance)
            {
                Vector3 direction = endPos - _startPos;
                Throw(direction);
            }
        }
    }

    private void Throw(Vector3 dir)
    {
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir.normalized * _force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & _layer) != 0)
        {
            gameObject.SetActive(false);
            if (GameManager.Instance.GameState != GameManager.EnumGameState.Finish)
            {
                GameEventManager.ResetLevel?.Invoke();
            }
        }
    }
}
