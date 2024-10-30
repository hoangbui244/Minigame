using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompletedPanel : MonoBehaviour
{
    [SerializeField] private GameObject _notiText;
    [SerializeField] private GameObject _obj1;
    [SerializeField] private GameObject _obj2;
    [SerializeField] private Animator _anim;
    
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
        _anim.Play("Completed");
        _notiText.SetActive(false);
        _obj1.SetActive(true);
        _obj2.SetActive(true);
    }
    
    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
        _obj1.SetActive(false);
        _obj2.SetActive(false);
    }
    
    public void ShareCompleted()
    {
        _notiText.SetActive(true);
    }
}