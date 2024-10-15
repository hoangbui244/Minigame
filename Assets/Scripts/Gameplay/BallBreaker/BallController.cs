using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private LayerMask _layer;
    private TrailRenderer _trailRenderer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        GameEventManager.ResetLevel += Reset;
        GameEventManager.ThrowBall += Throw;
    }
    
    private void OnDisable()
    {
        GameEventManager.ResetLevel -= Reset;
        GameEventManager.ThrowBall -= Throw;
        _trailRenderer.Clear();
    }
    
    private void Reset()
    {
        this.gameObject.SetActive(false);
    }

    private void Throw(Vector3 dir)
    {
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir.normalized * _force);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & _layer) != 0)
        {
            gameObject.SetActive(false);
            if (GameManager.Instance.GameState != GameManager.EnumGameState.Finish)
            {
                GameEventManager.ResetLevel?.Invoke();
            }
        }
    }
}
