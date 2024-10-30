using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompletedPanel : MonoBehaviour
{
    [SerializeField] private GameObject _notiText;
    [SerializeField] private Animator _anim;
    
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
        _anim.Play("Completed");
        _notiText.SetActive(false);
    }
    
    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
    }
    
    public void ShareCompleted()
    {
        _notiText.SetActive(true);
    }
}