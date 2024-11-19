using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
using UnityEngine.Serialization;

public class Stick : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff = Vector2.zero;
    private Vector2 _lastPointPrefabPos;
    [SerializeField] private Transform _startPos;
    [SerializeField] private float _pointOffset = 0.5f;
    [SerializeField] private float _dragResistance = 0.1f;
    [SerializeField] private LayerMask _penLayerMask;
    private Vector2 targetPosition;

    private void Start()
    {
        _camera = Camera.main;
        _lastPointPrefabPos = _startPos.position;
    }
    
    private void OnMouseDown()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero, 1000f, _penLayerMask);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject && CompareTag("Pen"))
                {
                    _diff = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
                    break;
                }
            }
        }
    }


    private void OnMouseDrag()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            targetPosition = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - _diff;
            transform.position = Vector2.Lerp(transform.position, targetPosition, 1 - _dragResistance);

            if (Vector2.Distance(_lastPointPrefabPos, _startPos.position) >= _pointOffset)
            {
                ObjectPooler.Instance.SpawnFromPool("Point", _startPos.position);
                HapticFeedback.LightFeedback();
                _lastPointPrefabPos = _startPos.position;
            }
        }
    }
}
