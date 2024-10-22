using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff = Vector2.zero;
    private Vector2 _lastPointPrefabPos;
    [SerializeField] private Transform _startPos;
    [SerializeField] private float _pointOffset = 0.5f;
    
    private void Start()
    {
        _camera = Camera.main;
        _lastPointPrefabPos = _startPos.position;
    }
    
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !MainUIMananger.Instance.PopupOpened)
        {
            //AudioManager.PlaySound("PickUp");
            //AudioManager.PlayVibration(true);
            _diff = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        }
    }

    private void OnMouseDrag()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            Vector2 newPosition = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - _diff;
            transform.position = newPosition;

            if (Vector2.Distance(_lastPointPrefabPos, _startPos.position) >= _pointOffset)
            {
                ObjectPooler.Instance.SpawnFromPool("Point", _startPos.position);
                _lastPointPrefabPos = _startPos.position;
            }
        }
    }
}
