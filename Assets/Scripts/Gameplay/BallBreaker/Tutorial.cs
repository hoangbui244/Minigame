using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _finger;
    [SerializeField] private GameObject _line;
    [SerializeField] private float _time;
    [SerializeField] private Vector3 _endPos;

    private void OnEnable()
    {
        GameEventManager.ResetLevel += Off;
        Move();
    }

    private void OnDisable()
    {
        GameEventManager.ResetLevel -= Off;
    }

    private void Off()
    {
        _finger.SetActive(false);
        _line.SetActive(false);
    }

    private void Move()
    {
        _finger.transform.DOMove(_endPos, _time).SetLoops(-1, LoopType.Yoyo);
    }
}