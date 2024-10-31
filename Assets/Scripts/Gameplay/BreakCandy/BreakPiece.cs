using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BreakPiece : MonoBehaviour
{
    private void OnEnable()
    {
        Anim();
    }
    
    private void Anim()
    {
        transform.DOScale(Vector3.zero, 0.8f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
