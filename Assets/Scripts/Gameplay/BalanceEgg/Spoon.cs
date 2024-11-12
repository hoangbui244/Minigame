using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spoon : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff;
    private bool _isFinished;

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
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 newPos = mousePos + _diff;

                float screenWidth = _camera.orthographicSize * _camera.aspect;
                float screenHeight = _camera.orthographicSize;
                float minX = -screenWidth + 2.5f;
                float maxX = screenWidth - 2.5f;
                float minY = -screenHeight + 2.5f;
                float maxY = screenHeight - 2.5f;

                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
                newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

                transform.position = newPos;
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}