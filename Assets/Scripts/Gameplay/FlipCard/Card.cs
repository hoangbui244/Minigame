using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    [SerializeField] private Sprite _front;
    [SerializeField] private Sprite _back;
    private static readonly Vector3 _halfFlip = new(0, 90, 0);
    private static readonly Vector3 _fullFlip = new(0, 180, 0);
    private SpriteRenderer _spriteRenderer;
    public bool Flipped;
    public bool Finished;
    public int Id;
    public static int PairCount;
    private readonly WaitForSeconds _delayTime = new(0.5f);

    private void Start()
    {
        PairCount = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (PairCount >= 2) return;
        if (!MainUIMananger.Instance.PopupOpened && !Finished && !Flipped)
        {
            PairCount++;
            //AudioManager.PlaySound("PickDown");
            FlipUp();
        }
    }

    private void FlipUp()
    {
        Flipped = true;
        transform.DORotate(_halfFlip, 0.2f).OnComplete(() =>
        {
            _spriteRenderer.sprite = _front;
            transform.DORotate(_fullFlip, 0.2f).OnComplete(() => { GameEventManager.FlipCard?.Invoke(this.Id); });
        });
    }

    public void FlipDown()
    {
        if (Flipped)
        {
            StartCoroutine(FlipDownCoroutine());
        }
    }

    private IEnumerator FlipDownCoroutine()
    {
        yield return _delayTime;

        Flipped = false;
        transform.DORotate(_halfFlip, 0.2f).OnComplete(() =>
        {
            _spriteRenderer.sprite = _back;
            transform.DORotate(Vector3.zero, 0.2f).OnComplete(() =>
            {
                Invoke(nameof(ResetPairCount), 0.1f);
            });
        });
    }
    
    public void End()
    {
        transform.DOScale(0, 0.35f).OnComplete(() => { gameObject.SetActive(false); });
    }
    
    
    private void ResetPairCount()
    {
        PairCount = 0;
    }
}