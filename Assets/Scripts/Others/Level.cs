using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public enum LevelType
    {
        FlipCard = 1,
        BallBreaker = 2,
        TapAway = 3,
        PieceTogether = 4,
        BreakCandy = 5,
        FindDifference = 6,
        CrocodileDentist = 7,
        CutInHalf = 8,
        DefuseBomb = 9,
        FruitCutting = 10,
        PerfectSlices = 11,
        PassBomb = 12,
        OneLine = 13,
        CatchEgg = 14,
        PencilTap = 15,
        DropCandy = 16,
        BalanceEgg = 17,
        PetalCount = 18,
        ScreamChicken = 19,
        Default = 0,
    }
    
    public LevelType Type;
    
    [SerializeField] private int _levelNo;
    [SerializeField] private Canvas _canvas;
    public int LevelNo => _levelNo;

    private void Start()
    {
        if (_canvas == null) return;
        _canvas.worldCamera = Camera.main;
        _canvas.sortingLayerName = "Object";
        _canvas.sortingOrder = 15;
    }

    private void OnValidate()
    {
        _canvas = GetComponentInChildren<Canvas>();
        if (_canvas != null)
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }
}