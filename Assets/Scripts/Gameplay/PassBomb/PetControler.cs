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
    [SerializeField] private float _customTime = 5f;
    [SerializeField] private PassBombController _passBombController;
    private SpriteRenderer _headSpriteRenderer;
    private SpriteRenderer _handSpriteRenderer;
    private Sequence _shakeSequence;
    private readonly Vector3 _rotation = new Vector3(0, 0, 45);
    private readonly Vector3 _shake = new Vector3(0.1f, 0.02f, 0);
    private readonly float _time = 0.3f;
    public bool HasBomb;
    public int ID;
    public bool IsBot;
    public bool IsExplode;

    private void Awake()
    {
        if (_head == null)
        {
            Debug.LogError("_head is not assigned in the Inspector.");
        }
        else
        {
            _headSpriteRenderer = _head.GetComponent<SpriteRenderer>();
            if (_headSpriteRenderer == null)
            {
                Debug.LogError("No SpriteRenderer component found on _head GameObject.");
            }
        }
        _handSpriteRenderer = _hand.GetComponent<SpriteRenderer>();
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
        HasBomb = true;
        _bomb.SetActive(true);
        _headSpriteRenderer.sprite = _headFear;
        
        _shakeSequence = DOTween.Sequence();
        _shakeSequence.Append(_head.transform.DOShakePosition(
                duration: 0.3f,                     
                strength: _shake, 
                vibrato: 10,                        
                randomness: 90,                    
                snapping: false,                    
                fadeOut: true))                    
            .SetLoops(-1, LoopType.Yoyo);

        
        if (IsBot)
        {
            StartCoroutine(AutoPassBomb());
        }
    }
    
    private IEnumerator AutoPassBomb()
    {
        float delay = _passBombController.PassTime;
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
        _head.transform.localPosition = Vector3.zero;
        _hand.transform.localPosition = Vector3.zero;
        _hand.transform.localRotation = Quaternion.identity;
        HasBomb = false;

        GameEventManager.NextBomb?.Invoke(this.ID);
        _bomb.SetActive(false);
        _hand.transform.DOKill();
        _hand.transform.DOLocalRotate(_rotation, _time, RotateMode.LocalAxisAdd)
            .SetEase(Ease.OutBack).OnComplete(() =>
            {
                _hand.transform.DOLocalRotate(-_rotation, _time, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutBack);
            });
    }
    public void ReceiveBomb()
    {
        _hand.transform.DOKill();
        _hand.transform.DOLocalRotate(-_rotation, _time, RotateMode.LocalAxisAdd)
            .SetEase(Ease.OutBack).OnComplete(() =>
            {
                _hand.transform.DOLocalRotate(_rotation, _time, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutBack);
            });
    }
}
