using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    [SerializeField] private Material sliceMaterial;
    private List<float> _slicePositionsX = new ();
    private float[] _slicePositionsArray;
    private static readonly int SlicePositionsX = Shader.PropertyToID("_SlicePositionsX");
    private static readonly int SliceCount = Shader.PropertyToID("_SliceCount");

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = new Vector3(_startPosition.x + 3.6f, _startPosition.y - 1f, _startPosition.z);
        _slicePositionsArray = new float[300]; 
        sliceMaterial.SetFloatArray(SlicePositionsX, _slicePositionsArray);
        sliceMaterial.SetInt(SliceCount, 0);
        Move();
    }

    private void Move()
    {
        transform.DOMoveX(_endPosition.x, 20f);
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