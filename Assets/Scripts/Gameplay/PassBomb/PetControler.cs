using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PetControler : MonoBehaviour
{
    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private Sprite _headNor;
    [SerializeField] private Sprite _headFear;
    private SpriteRenderer _spriteRenderer;
    private readonly Vector3 _rotation = new Vector3(0, 0, 45);
    private readonly float _time = 0.5f;
    public bool HasBomb;
    public int ID;
    public bool IsBot;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _headNor;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (HasBomb)
            {
                PassBomb();
            }
        }
    }
    
    public void GetBomb()
    {
        _spriteRenderer.sprite = _headFear;
        HasBomb = true;
        _bomb.SetActive(true);
        transform.DOShakePosition(0.5f, 1, 10, 90, false, true);
    }
    
    private void PassBomb()
    {
        _spriteRenderer.sprite = _headNor;
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