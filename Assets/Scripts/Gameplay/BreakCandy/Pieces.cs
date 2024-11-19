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
    public bool Done;
    
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
        if (Done) return;
        Done = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(gameObject.transform.DOMove(_endPos, _time))
            .Join(gameObject.GetComponent<SpriteRenderer>().DOFade(0, _time))
            .OnComplete(() => 
            {
                AudioManager.PlaySound("CandyBreakOne");
                GameEventManager.BreakCandy?.Invoke(true);
                sequence.Kill();
            });
    }

}
