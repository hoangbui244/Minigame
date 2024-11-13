using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyHolder : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff;
    private Vector2 _initialPosition;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Candy>(out var candy))
        {
            if (candy.Can)
            {
                GameEventManager.DropCandy?.Invoke(1);
            }
            else
            {
                GameUIManager.Instance.Retry(true);
            }
        }
    }
}
