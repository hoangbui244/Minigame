using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCheck : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Candy")) 
        {
            ChangeColor(Color.red);
            GameEventManager.BreakCandy?.Invoke(false);
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Candy")) 
        {
            ChangeColor(Color.red);
        }
    }

    private void ChangeColor(Color newColor)
    {
         _spriteRenderer.color = newColor;
    }
}
