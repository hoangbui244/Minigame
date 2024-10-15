using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    public bool IsFinished;
    [SerializeField] private bool _isTrapped;
    [SerializeField] private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        if (!_isTrapped && !MainUIMananger.Instance.PopupOpened)
        {
            // AudioManager.PlaySound("Tooth");
            // AudioManager.PlayVibration(true);
            _collider.enabled = false;
            IsFinished = true;
        }
    }
    
    private void OnMouseUp()
    {
        if (!_isTrapped && !MainUIMananger.Instance.PopupOpened)
        {
            // AudioManager.PlaySound("Tooth");
            // GameEventManager.CheckTeeth?.Invoke(true);
        }
    }
}
