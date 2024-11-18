using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PencilTapController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _pens;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private SpriteRenderer _pen;
    private int _currentScore;
    private int _highScore;

    private void OnEnable()
    {
        GameEventManager.PencilTap += Check;
        Init();
    }

    private void OnDisable()
    {
        GameEventManager.PencilTap -= Check;
    }

    private void Init()
    {
        InitPen();
        _highScore = ResourceManager.PencilTapHighScore;
        _currentScore = 0;
        _highScoreText.text = "Highscore: " + _highScore.ToString();
    }

    private void InitPen()
    {
        var num = ResourceManager.PencilTap - 1;
        _pen.sprite = _pens[num];
    }

    private void Check(int value)
    {
        _currentScore += value;
        _currentScoreText.text = _currentScore.ToString();
        if (ResourceManager.PencilTap < 10)
        {
            ResourceManager.PencilTap++;
        }
        else
        {
            ResourceManager.PencilTap = 1;
        }

        Invoke(nameof(InitPen), 0.6f);
        if (_currentScore % 3 == 0)
        {
            if (AdsManager.Instance.CanShowBreak)
            {
                GameUIManager.Instance.ShowTeaBreak();
            }
        }

        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            _highScoreText.text = "Highscore: " + _highScore.ToString();
            ResourceManager.PencilTapHighScore = _highScore;
        }
    }
}