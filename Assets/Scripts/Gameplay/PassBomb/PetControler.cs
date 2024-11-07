using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class PetControler : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private Sprite _handExplode;
    [SerializeField] private Sprite _headExplode;
    [SerializeField] private Sprite _headNor;
    [SerializeField] private Sprite _headFear;
    private SpriteRenderer _headSpriteRenderer;
    private SpriteRenderer _handSpriteRenderer;
    private Sequence _shakeSequence;
    private readonly Vector3 _rotation = new Vector3(0, 0, 45);
    private readonly float _time = 0.5f;
    public bool HasBomb;
    public int ID;
    public bool IsBot;
    public bool IsExplode;

    private void Awake()
    {
        _headSpriteRenderer = _head.GetComponentInChildren<SpriteRenderer>();
        _handSpriteRenderer = _hand.GetComponentInChildren<SpriteRenderer>();
        _headSpriteRenderer.sprite = _headNor;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (HasBomb && !IsBot)
            {
                PassBomb();
            }
        }
    }

    public void Explode()
    {
        IsExplode = true;
        HasBomb = false;
        _bomb.SetActive(false);
        _shakeSequence?.Kill();
        _headSpriteRenderer.sprite = _headExplode;
        _handSpriteRenderer.sprite = _handExplode;
        GameEventManager.PassBomb?.Invoke();
    }
    
    public void GetBomb()
    {
        _headSpriteRenderer.sprite = _headFear;
        HasBomb = true;
        _bomb.SetActive(true);
        
        _shakeSequence = DOTween.Sequence();
        _shakeSequence.Append(_head.transform.DOShakePosition(0.5f, 0.2f, 5, 50, false, true))
            .SetLoops(-1, LoopType.Yoyo);
        
        if (IsBot)
        {
            StartCoroutine(AutoPassBomb());
        }
    }
    
    private IEnumerator AutoPassBomb()
    {
        float delay = Random.Range(0f, 4f);
        yield return new WaitForSeconds(delay);

        if (HasBomb)
        {
            PassBomb();
        }
    }
    
    private void PassBomb()
    {
        _shakeSequence?.Kill();

        _headSpriteRenderer.sprite = _headNor;
        HasBomb = false;

        _hand.transform.DOLocalRotate(_rotation, _time, RotateMode.LocalAxisAdd)
            .SetEase(Ease.OutBack).OnComplete(() =>
            {
                _bomb.SetActive(false);
                GameEventManager.NextBomb?.Invoke(this.ID);
                _hand.transform.DOLocalRotate(-_rotation, _time, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutBack);
            });
    }
}