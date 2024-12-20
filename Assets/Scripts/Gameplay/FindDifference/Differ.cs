using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Differ : MonoBehaviour
{
    public bool IsTrapped;
    [SerializeField] private List<Sprite> _sprites;
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        SetupLevel();
    }

    private void SetupLevel()
    {
        int num = ResourceManager.FindDifference;
        _spriteRenderer.sprite = _sprites[num];
    }
    
    private void OnMouseDown()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            AudioManager.PlaySound("Click");
            AudioManager.LightFeedback();
            if (!IsTrapped)
            {
                GameUIManager.Instance.Retry(true);
            }
            else
            {
                if (ResourceManager.FindDifference < 11)
                {
                    ResourceManager.FindDifference++;
                }
                else
                {
                    ResourceManager.FindDifference = 0;
                }
                GameEventManager.FindDifference?.Invoke();
            }
        }
    }
}
