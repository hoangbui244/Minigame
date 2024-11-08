using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EggHolder : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff;
    private bool _isFinished;
    private Vector2 _initialPosition;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened && !IsPointerOverUI())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _diff = (Vector2)transform.position - mousePos;
                _initialPosition = transform.position;
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                float newX = mousePos.x + _diff.x;

                float screenWidth = _camera.orthographicSize * _camera.aspect; 
                float minX = -screenWidth + 2.5f;
                float maxX = screenWidth - 2.5f;
                newX = Mathf.Clamp(newX, minX, maxX);

                transform.position = new Vector2(newX, _initialPosition.y);
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Egg>(out var egg))
        {
            if (egg.Can)
            {
                GameEventManager.CatchEgg?.Invoke(1);
            }
            else
            {
                GameUIManager.Instance.Retry(true);
            }
        }
    }
}