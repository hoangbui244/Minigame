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
        var num = ResourceManager.PencilTap - 1;
        _pen.sprite = _pens[num];
        _highScore = ResourceManager.PencilTapHighScore;
        _currentScore = 0;
        _highScoreText.text = "Highscore: " + _highScore.ToString();
    }
    
    private void Check(int value)
    {
        _currentScore += value;
        _currentScoreText.text = _currentScore.ToString();
        if (_currentScore % 3 == 0)
        {
            GameUIManager.Instance.TeaBreak(true);
        }
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            _highScoreText.text = "Highscore: " + _highScore.ToString();
            ResourceManager.PencilTapHighScore = _highScore;
        }
    }
}
