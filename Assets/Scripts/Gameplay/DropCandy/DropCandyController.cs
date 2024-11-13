using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCandyController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private void OnEnable()
    {
        _canvas.worldCamera = Camera.main;
    }
}
