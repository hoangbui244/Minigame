using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private GameObject _correctForm;
    [SerializeField] private float _minRange = 2f;
    private SpriteRenderer _spriteRenderer => GetComponent<SpriteRenderer>();
    private Collider2D _collider2D => GetComponent<Collider2D>();
    private bool _isFinish;
    private Quaternion _startRotation;
    [SerializeField] private bool _isSorting;
    [SerializeField] private float _offset = 3f;
    private int _sortingOrder;
    private static int _highestSortingOrder = 1;


    private void Start()
    {
        _startRotation = this.transform.rotation;
        _sortingOrder = _spriteRenderer.sortingOrder;
    }
    
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !MainUIMananger.Instance.PopupOpened && !_isFinish)
        {
            AudioManager.LightFeedback();
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y + _offset, this.gameObject.transform.localPosition.z);
            _spriteRenderer.sortingOrder = ++_highestSortingOrder;
            transform.DORotate(_correctForm.transform.rotation.eulerAngles, 0.4f);
        }
    }

    private void OnMouseDrag()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float cameraHalfHeight = Camera.main.orthographicSize;
            float targetY = Mathf.Clamp(mousePos.y + _offset, -cameraHalfHeight + _offset, cameraHalfHeight - _offset);
            Vector3 targetPosition = new Vector3(mousePos.x, targetY, transform.localPosition.z);
            gameObject.transform.position = Vector3.Lerp(transform.localPosition, targetPosition, 0.95f);
        }
    }

    private void OnMouseUp()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            if (Mathf.Abs(this.transform.localPosition.x - _correctForm.transform.localPosition.x) <= _minRange &&
                Mathf.Abs(this.transform.localPosition.y - _correctForm.transform.localPosition.y) <= _minRange)
            {
                transform.position = new Vector3(_correctForm.transform.position.x, _correctForm.transform.position.y, _correctForm.transform.position.z);
                transform.rotation = _correctForm.transform.rotation;
                _isFinish = true;
                AudioManager.PlaySound("FlipCard");
                GameEventManager.Check?.Invoke(_isFinish);
                _spriteRenderer.sortingOrder = !_isSorting ? 1 : _sortingOrder;
                _collider2D.enabled = false;
            }
            else
            {
                _spriteRenderer.sortingOrder = _highestSortingOrder;
                transform.DORotate(_startRotation.eulerAngles, 0.4f);
            }
        }
    }
}
