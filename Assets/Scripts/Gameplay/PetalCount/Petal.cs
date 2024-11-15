using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Petal : MonoBehaviour
{
    [SerializeField] private float _torque;
    private Rigidbody2D _rb;
    private Collider2D _collider2D;
    
    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
    }

    private void OnMouseDown()
    {
        _collider2D.enabled = false;
        int num = Random.Range(0, 100);
        if (num <= 35)
        {
            Rota();
        }
        AudioManager.PlaySound("Click");
        AudioManager.LightFeedback();
        _rb.gravityScale = 1.5f;
    }

    private void Rota()
    {
        _rb.AddTorque(_torque);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            GameEventManager.PetalCount?.Invoke(1);
            gameObject.SetActive(false);
        }
    }
}
