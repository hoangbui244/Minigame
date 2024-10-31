using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    [SerializeField] private List<CandyPiece> _candyPieces;
    [SerializeField] private GameObject _piece;
    [SerializeField] private Vector3 _endPos;
    [SerializeField] private float _time;
    
    private void OnEnable()
    {
        GameEventManager.BreakCandyPiece += Check;
    }
    
    private void OnDisable()
    {
        GameEventManager.BreakCandyPiece -= Check;
    }

    private void OnValidate()
    {
        _candyPieces = new List<CandyPiece>();
        foreach (var piece in _piece.GetComponentsInChildren<CandyPiece>())
        {
            _candyPieces.Add(piece);
        }
    }

    private void Check()
    {
        foreach (var piece in _candyPieces)
        {
            if (!piece.IsBroken)
            {
                return;
            }
        }
        Anim();
    }

    private void Anim()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(_piece.transform.DOMove(_endPos, _time))
            .Join(_piece.GetComponent<SpriteRenderer>().DOFade(0, _time))
            .OnComplete(() => 
            {
                GameEventManager.BreakCandy?.Invoke(true);
                _piece.SetActive(false);
            });
    }
}
