using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Candy : MonoBehaviour
{
    public enum CandyType
    {
        Candy1,
        Candy2,
        Candy3,
        Candy4,
        Candy5,
        Candy6,
        Candy7,
        Candy8,
    }
    
    public CandyType Type;
    [SerializeField] private List<Sprite> _candies;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnDisable()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void InitCandy(int type)
    {
        int chance = Random.Range(0, 100);
    
        if (chance < 50)
        {
            Type = (CandyType)type;
        }
        else
        {
            int randomType;
            do
            {
                randomType = Random.Range(0, _candies.Count);
            } while (randomType == type);
        
            Type = (CandyType)randomType;
        }

        _spriteRenderer.sprite = _candies[(int)Type];
    }
    
    public void UpSpeed()
    {
        _rigidbody2D.gravityScale += 0.25f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
    }
}
