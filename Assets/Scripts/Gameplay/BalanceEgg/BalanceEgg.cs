using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BalanceEgg : MonoBehaviour
{
    public bool IsHolded;
    [SerializeField] private float _instabilityForce = 1f;
    [SerializeField] private float _instabilityIncreaseRate = 0.1f;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 randomForce = new Vector2(Random.Range(-1f, 1f), 0) * _instabilityForce;
        _rb.AddForce(randomForce);

        _instabilityForce += _instabilityIncreaseRate * Time.fixedDeltaTime;
    }
    
    private void GameOver()
    {
        GameEventManager.BalanceEgg?.Invoke();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("spoon"))
        {
            IsHolded = true;
        }
        else
        {
            gameObject.SetActive(false);
            Invoke(nameof(GameOver), 0.5f);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("spoon"))
        {
            IsHolded = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("spoon"))
        {
            IsHolded = false;
        }
    }
}