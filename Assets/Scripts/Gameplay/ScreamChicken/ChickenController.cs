using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChickenController : MonoBehaviour
{
    public bool IsVoice;

    [Header("Touch & Hold")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _forwardSpeed = 2f;
    [SerializeField] private int _maxJumps = 5;

    [Header("Voice")]
    [SerializeField] private float _sensitivity = 100f;
    [SerializeField] private float _threshold = 0.1f;
    [SerializeField] private ScreamChickenController _scr;
    private AudioSource _audioSource;
    private Queue<float> _loudnessBuffer = new Queue<float>();
    private const int _bufferSize = 5;
    
    public bool StartGame;
    private Rigidbody2D _rb;
    private bool _isHolding = false;
    private bool _isGrounded = false;
    private int _numberOfJumps = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _isGrounded = true;
    }

    private void Update()
    {
        if (!StartGame) return;
        if (IsVoice)
        {
            float loudness = GetSmoothedLoudness(_scr.GetLoudnessFromMicrophone()) * _sensitivity;
            if (loudness > _threshold && _isGrounded)
            {
                _isHolding = true;
            }
            else if (loudness < _threshold && _isHolding)
            {
                _isHolding = false;
            }
        }
        else
        {
            HandleTouchInput();
        }
    }
    
    private void FixedUpdate()
    {
        if (_isHolding)
        {
            ApplyJump();
        }

        if (!_isGrounded)
        {
            MaintainForwardSpeed();
        }
    }

    private float GetSmoothedLoudness(float currentLoudness)
    {
        if (_loudnessBuffer.Count >= _bufferSize)
            _loudnessBuffer.Dequeue();
    
        _loudnessBuffer.Enqueue(currentLoudness);

        float sum = 0;
        foreach (float loudness in _loudnessBuffer)
            sum += loudness;
    
        return sum / _loudnessBuffer.Count;
    }

    private void HandleTouchInput()
    {
        if (Input.GetMouseButton(0) && _isGrounded)
        {
            _isHolding = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isHolding = false;
        }
    }

    private void ApplyJump()
    {
        if (_numberOfJumps < _maxJumps)
        {
            Vector2 velocity = _rb.velocity;
            velocity.y = _jumpForce;
            _rb.velocity = velocity;

            _numberOfJumps++;
        }
        else
        {
            _isHolding = false;
        }
        
        if (IsVoice)
        {
            _isGrounded = false;
        }
    }

    private void MaintainForwardSpeed()
    {
        Vector2 velocity = _rb.velocity;
        velocity.x = _forwardSpeed;
        _rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            _isGrounded = true;
            _numberOfJumps = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            _isGrounded = false;
        }
    }
    
    public void ChooseType(int type)
    {
        IsVoice = type == 1;
    }
}

