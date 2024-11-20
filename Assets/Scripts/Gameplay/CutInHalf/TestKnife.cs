using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestKnife : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff;
    [SerializeField] private float _time;
    [SerializeField] private float _raycastDistance = 10f;
    private bool _isFinished;
    private bool _isCut;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened && !IsPointerOverUI() && !_isFinished)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _diff = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
                GameEventManager.ResetLevel?.Invoke();
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector2(mousePos.x - _diff.x, transform.position.y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isFinished = true;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance);

                if (hit.collider != null && hit.collider.CompareTag("SlicedObj"))
                {
                    _isCut = true;
                    Vector2 contactPoint = hit.point;
                    DOTween.Sequence()
                        .AppendInterval(0.3f)
                        .OnComplete(() => GameEventManager.Test?.Invoke(contactPoint.x));
                }
                
                DOTween.Sequence()
                    .Append(transform.DOMoveY(transform.position.y - 100f, _time)
                        .SetEase(Ease.InQuad))
                    .Insert(0.2f, DOTween.To(() => 0, x => {}, 0, 0).OnComplete(() => AudioManager.PlaySound("CutInHalf")))
                    .OnComplete(() =>
                    {
                        if (!_isCut)
                        {
                            GameUIManager.Instance.Retry(true);
                        }
                        gameObject.SetActive(false);
                    });
            }
        }
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return true;

        return false;
    }

}
