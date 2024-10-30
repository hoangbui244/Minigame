using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovedObj : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff = Vector2.zero;
    [SerializeField] private float _time;
    private bool _isMoving;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _diff = (Vector2)mousePos - (Vector2)transform.position;
                _isMoving = true;
            }
            
            if (Input.GetMouseButton(0) && _isMoving)
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector2(mousePos.x - _diff.x, transform.position.y);
            }
            
            if (Input.GetMouseButtonUp(0) && _isMoving)
            {
                _isMoving = false;
                float targetY = transform.position.y - 100f;
                transform.DOMoveY(targetY, _time).SetEase(Ease.InQuad).OnComplete(() => { gameObject.SetActive(false); });
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}