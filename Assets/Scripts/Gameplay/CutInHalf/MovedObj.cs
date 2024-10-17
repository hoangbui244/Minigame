using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovedObj : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff = Vector2.zero;
    [SerializeField] private float _time;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !MainUIMananger.Instance.PopupOpened)
        {
            _diff = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        }
    }

    private void OnMouseDrag()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePos.x - _diff.x, transform.position.y);
        }
    }

    private void OnMouseUp()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            float targetY = transform.position.y - 100f;
            transform.DOMoveY(targetY, _time).SetEase(Ease.InQuad).OnComplete(() => { gameObject.SetActive(false); });
        }
    }

}
