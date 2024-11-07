using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spoon : MonoBehaviour
{
    private Vector3 _offset;
    private bool _isDragging = false;
    private Renderer _renderer;
    private float _objectWidth;
    private float _objectHeight;
    private Camera _camera;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _objectWidth = _renderer.bounds.size.x / 2;
        _objectHeight = _renderer.bounds.size.y / 2;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (!IsPointerOverUI(touch))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPosition = _camera.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0;
                    _offset = transform.position - touchPosition;
                    _isDragging = true;
                }
                else if (touch.phase == TouchPhase.Moved && _isDragging)
                {
                    Vector3 newPos = _camera.ScreenToWorldPoint(touch.position) + _offset;
                    newPos.z = 0;

                    Vector3 screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                    
                    newPos.x = Mathf.Clamp(newPos.x, -screenBounds.x + _objectWidth, screenBounds.x - _objectWidth);
                    newPos.y = Mathf.Clamp(newPos.y, -screenBounds.y + _objectHeight, screenBounds.y - _objectHeight);

                    transform.position = newPos;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _isDragging = false;
                }
            }
        }
    }

    private bool IsPointerOverUI(Touch touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }
}
