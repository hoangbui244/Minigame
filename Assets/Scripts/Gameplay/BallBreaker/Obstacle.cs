using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private enum ObjType
    {
        Once,
        Blur,
        Unbreakable,
        Through
    }

    public bool IsFinish => _isFinish;
    [SerializeField] private ObjType _type;
    [SerializeField] private float _fadeTime = 0.5f;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private bool _isBlur;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private bool _isFinish;
    private bool _isTrigger;
    private static readonly Vector3 StartScale = Vector3.zero;
    private static readonly Vector3 ReScale = Vector3.one;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        gameObject.SetActive(true);
        transform.localScale = StartScale;

        if (!_isBlur)
        {
            _spriteRenderer.DOFade(1, _fadeTime).SetAutoKill(false).Restart();
            transform.DOScale(_scale, _fadeTime).SetEase(Ease.OutQuad).SetAutoKill(false).Restart();
        }
        else
        {
            _spriteRenderer.DOFade(0.6f, _fadeTime).SetAutoKill(false).Restart();
            transform.DOScale(_scale, _fadeTime).SetEase(Ease.OutQuad).SetAutoKill(false).Restart();
        }
    }

    public void Reset()
    {

        if (!_isBlur)
        {
            if (!_isFinish) return;
            gameObject.SetActive(true);
            _isFinish = false;
            _collider2D.enabled = true;

            transform.localScale = ReScale;

            _spriteRenderer.DOFade(1, _fadeTime).SetAutoKill(false).Restart();
            transform.DOScale(_scale, _fadeTime).SetEase(Ease.OutQuad).SetAutoKill(false).Restart();
        }
        else
        {
            if (!_isTrigger) return;
            gameObject.SetActive(true);
            _isTrigger = false;
            _type = ObjType.Blur;
            _collider2D.enabled = true;
            _collider2D.isTrigger = true;
            transform.localScale = ReScale;

            _spriteRenderer.DOFade(0.6f, _fadeTime).SetAutoKill(false).Restart();
            transform.DOScale(_scale, _fadeTime).SetEase(Ease.OutQuad).SetAutoKill(false).Restart();
        }
    }

    private void FadeOut()
    {
        if (_type == ObjType.Unbreakable)
        {
            float time = _fadeTime * 1.2f;
            transform.DOScale(new Vector3(0, 0, 0), time).SetEase(Ease.InQuad).SetAutoKill(false).Restart();
            _spriteRenderer.DOFade(0, time).SetAutoKill(false).Restart();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (_type)
        {
            case ObjType.Once:
                _isFinish = true;
                PlayAnimOnce();
                break;
            case ObjType.Unbreakable:
                PlayAnimUnbreakable();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (_type)
        {
            case ObjType.Blur:
                _type = ObjType.Once;
                _collider2D.isTrigger = false;
                _isTrigger = true;
                PlayAnimBlur();
                break;
            case ObjType.Through:
                
                break;
        }
    }

    private void PlayAnimOnce()
    {
        _collider2D.enabled = false;
        GameEventManager.BallBreaker?.Invoke();
        transform.DOScale(new Vector3(0, 0, 0), _fadeTime).SetEase(Ease.InQuad).SetAutoKill(false).Restart();
        _spriteRenderer.DOFade(0, _fadeTime).SetAutoKill(false).Restart();
    }

    private void PlayAnimUnbreakable()
    {
        transform.DOShakeScale(_fadeTime * 0.6f, 0.1f, 1, 0.2f).OnComplete(() =>
            {
                transform.DOScale(_scale, 0.2f).SetEase(Ease.OutQuad);
            })
            .SetAutoKill(false).Restart();
    }

    private void PlayAnimBlur()
    {
        _spriteRenderer.DOFade(1, _fadeTime).SetAutoKill(false).Restart();
    }
}