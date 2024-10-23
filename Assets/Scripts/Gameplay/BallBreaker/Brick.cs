using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Brick : MonoBehaviour
{
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayAnim();
    }

    private void PlayAnim()
    {
        _collider2D.enabled = false; 
        GameEventManager.BallBreaker?.Invoke(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one * 0.9f, 0.25f));
        sequence.Append(_spriteRenderer.DOFade(0, 0.3f));
        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}