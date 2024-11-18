using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAdsPanel : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField] private Sprite _purchase;
    
    private void OnEnable()
    {
        if (ResourceManager.RemoveAds)
        {
            _btn.image.sprite = _purchase;
            _btn.interactable = false;
        }
    }

    public void Close()
    {
        AudioManager.PlaySound("Click");
        gameObject.SetActive(false);
    }
}
