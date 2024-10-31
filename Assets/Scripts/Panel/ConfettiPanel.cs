using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiPanel : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _obj1;
    [SerializeField] private GameObject _obj2;
    
    private void OnEnable()
    {
        _anim.Play("Confetti");
        _obj1.SetActive(true);
        _obj2.SetActive(true);
    }
}