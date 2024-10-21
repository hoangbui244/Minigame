using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differ : MonoBehaviour
{
    [SerializeField] private bool _isTrapped;
    [SerializeField] private List<Sprite> _sprites;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (!_isTrapped && !MainUIMananger.Instance.PopupOpened)
        {
            // AudioManager.PlaySound("Tooth");
            // AudioManager.PlayVibration(true);
            // GameEventManager.CheckDiff?.Invoke(true);
        }
    }
}
