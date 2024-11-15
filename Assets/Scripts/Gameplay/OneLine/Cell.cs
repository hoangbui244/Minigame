using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite _color;
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _end;
    [SerializeField] private GameObject _des;
    public bool IsStart;
    public bool IsVisited;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _desSpriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _default;
        IsVisited = false;
    }

    private void OnValidate()
    {
        _des = transform.GetChild(0).gameObject;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void End()
    {
       SpriteRenderer end = _des.gameObject.GetComponentInChildren<SpriteRenderer>();
       end.sprite = _end;
    }
    
    public void Picked()
    {
        if (!IsVisited)
        {
            AudioManager.LightFeedback();
            _spriteRenderer.sprite = _color;
            IsVisited = true;
        }
    }

    public void ResetCell()
    {
        _spriteRenderer.sprite = _default;
        IsVisited = false;
    }
}