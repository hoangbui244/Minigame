using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private float _time;
    [SerializeField] private Material sliceMaterial;
    private List<float> _slicePositionsX = new ();
    private float[] _slicePositionsArray;
    private static readonly int SlicePositionsX = Shader.PropertyToID("_SlicePositionsX");
    private static readonly int SliceCount = Shader.PropertyToID("_SliceCount");

    private void Start()
    {
        _slicePositionsArray = new float[300]; 
        sliceMaterial.SetFloatArray(SlicePositionsX, _slicePositionsArray);
        sliceMaterial.SetInt(SliceCount, 0);
        Move();
    }

    private void Move()
    {
        transform.DOMoveX(_endPosition.x, _time).SetEase(Ease.Linear).OnComplete(() =>
        {
            GameEventManager.FruitCutting?.Invoke();
        });
    }

    public void Chop()
    {
        transform.DOMoveY(_endPosition.y, 0.1f).OnComplete(() =>
        {
            SliceObject();
            transform.DOMoveY(_startPosition.y, 0.1f);
        });
    }

    private void SliceObject()
    {
        float currentSlicePositionX = transform.position.x;
        _slicePositionsX.Add(currentSlicePositionX);

        for (int i = 0; i < _slicePositionsX.Count; i++)
        {
            _slicePositionsArray[i] = _slicePositionsX[i];
        }
        sliceMaterial.SetFloatArray(SlicePositionsX, _slicePositionsArray);
        sliceMaterial.SetInt(SliceCount, _slicePositionsX.Count);
    }
}