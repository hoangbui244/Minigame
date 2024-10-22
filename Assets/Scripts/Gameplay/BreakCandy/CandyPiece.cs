using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CandyPiece : MonoBehaviour
{
    [SerializeField] private Vector3 _endPos;
    [SerializeField] private float _time;
    [SerializeField] private GameObject _parent;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Point")) 
        {
            _collider.enabled = false;
            GameEventManager.BreakCandy?.Invoke(true);

            Sequence sequence = DOTween.Sequence();
            sequence.Join(_parent.transform.DOMove(_endPos, _time))
                .Join(_parent.GetComponent<SpriteRenderer>().DOFade(0, _time))
                .OnComplete(() => 
                {
                    _parent.SetActive(false);
                });
        }
    }
}