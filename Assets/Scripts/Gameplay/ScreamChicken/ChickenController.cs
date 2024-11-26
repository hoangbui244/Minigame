using System;
using System.Collections;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    public bool IsVoice;

    [Header("Touch & Hold")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _forwardSpeed = 2f;
    [SerializeField] private int _maxJumps = 5;

    [Header("Voice Control")]
    [SerializeField] private float sensitivity = 100f;
    private AudioSource _audioSource;
    private float[] _samples = new float[256];
    private bool _isMicrophoneInitialized = false;

    public bool StartGame;
    private Rigidbody2D _rb;
    private bool _isHolding = false;
    private bool _isGrounded = false;
    private int _numberOfJumps = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _isGrounded = true;
        if (IsVoice)
        {
            InitializeMicrophone();
        }
    }

    private void Update()
    {
        if (!StartGame) return;
        if (IsVoice)
        {
            if (_isMicrophoneInitialized)
            {
                AnalyzeSound();
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
    
    public void ChooseType(int type)
    {
        IsVoice = type == 1;
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
    }

    private void MaintainForwardSpeed()
    {
        Vector2 velocity = _rb.velocity;
        velocity.x = _forwardSpeed;
        _rb.velocity = velocity;
    }

    private void InitializeMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = Microphone.Start(null, true, 10, 44100);
            _audioSource.loop = true;

            StartCoroutine(WaitForMicrophoneStart());
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    private IEnumerator WaitForMicrophoneStart()
    {
        while (!(Microphone.GetPosition(null) > 0))
        {
            yield return null;
        }

        _audioSource.Play();
        _isMicrophoneInitialized = true;
    }

    private void AnalyzeSound()
    {
        _audioSource.GetOutputData(_samples, 0);
        float soundLevel = 0f;

        foreach (var sample in _samples)
        {
            soundLevel += Mathf.Abs(sample);
        }

        soundLevel = soundLevel / _samples.Length * sensitivity;

        _isHolding = soundLevel > 1f;
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
}

