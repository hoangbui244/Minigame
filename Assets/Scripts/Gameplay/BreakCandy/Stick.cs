using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class Stick : MonoBehaviour
{
    private Camera _camera;
    private Vector2 _diff = Vector2.zero;
    private Vector2 _lastPointPrefabPos;
    [SerializeField] private Transform _startPos;
    [SerializeField] private float _pointOffset = 0.5f;
    [SerializeField] private float dragResistance = 0.1f;
    private Vector2 targetPosition;
    
    private void Start()
    {
        _camera = Camera.main;
        _lastPointPrefabPos = _startPos.position;
    }
    
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !MainUIMananger.Instance.PopupOpened && GameManager.Instance.GameState == GameManager.EnumGameState.Play)
        {
            //AudioManager.PlaySound("PickUp");
            _diff = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        }
    }

    private void OnMouseDrag()
    {
        if (!MainUIMananger.Instance.PopupOpened && GameManager.Instance.GameState == GameManager.EnumGameState.Play)
        {
            targetPosition = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition) - _diff;
            transform.position = Vector2.Lerp(transform.position, targetPosition, 1 - dragResistance);
            if (Vector2.Distance(_lastPointPrefabPos, _startPos.position) >= _pointOffset)
            {
                ObjectPooler.Instance.SpawnFromPool("Point", _startPos.position);
                HapticFeedback.LightFeedback();
                _lastPointPrefabPos = _startPos.position;
            }
        }
    }
}
