using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CandyPiece : MonoBehaviour
{
    public bool IsBroken;
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
            IsBroken = true;
            GameEventManager.BreakCandyPiece?.Invoke();
        }
    }
}