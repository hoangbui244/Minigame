using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public enum EggType
    {
        Gold,
        White,
        Black
    }

    public EggType Type;
    public bool Can;
    
    [SerializeField] private Sprite _gold;
    [SerializeField] private Sprite _white;
    [SerializeField] private Sprite _black;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        RandomEgg();
    }
    
    private void OnDisable()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }
    
    private void RandomEgg()
    {
        Type = (EggType)UnityEngine.Random.Range(0, 3);
        switch (Type)
        {
            case EggType.Gold:
                _spriteRenderer.sprite = _gold;
                Can = true;
                break;
            case EggType.White:
                _spriteRenderer.sprite = _white;
                Can = false;
                break;
            case EggType.Black:
                _spriteRenderer.sprite = _black;
                Can = false;
                break;
        }
    }

    public void UpSpeed()
    {
        _rigidbody2D.gravityScale += 0.15f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
    }
}
