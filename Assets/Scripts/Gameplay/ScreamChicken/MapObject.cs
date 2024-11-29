using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MapObject : MonoBehaviour
{
    public enum ObjectType
    {
        Shuriken,
        Fish,
    }
    
    public ObjectType Type;
    
    [Header("Shuriken")]
    [SerializeField] private float _timeRota;
    private Vector3 _rota = new Vector3(0, 0, 360);
    
    [Header("Fish")]
    [SerializeField] private float _timeMove;
    [SerializeField] private float _timeFishRota;
    [SerializeField] private Vector3 _moveTo;
    [SerializeField] private float _timeDelay;

    private void Start()
    {
        ActiveTrap();
    }

    private void ActiveTrap()
    {
        switch (Type)
        {
            case ObjectType.Shuriken:
                Shuriken();
                break;
            case ObjectType.Fish:
                Fish();
                break;
        }
    }
    
    private void Shuriken()
    {
        transform
            .DORotate(-_rota, _timeRota, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void Fish()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .AppendInterval(_timeDelay)
            .Append(transform.DOMove(_moveTo, _timeMove)
                .SetEase(Ease.Linear))
            .SetLoops(-1, LoopType.Yoyo)
            .OnStepComplete(() =>
            {
                if (this != null && transform != null)
                {
                    transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + 180), _timeFishRota);
                }
            })
            .SetAutoKill(true);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
