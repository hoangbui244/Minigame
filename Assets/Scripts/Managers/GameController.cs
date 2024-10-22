using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Knife _knife;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    private int _currentScore;

    private void OnEnable()
    {
        GameEventManager.FruitCutting += Check;
    }
    
    private void OnDisable()
    {
        GameEventManager.FruitCutting -= Check;
    }
    
    private void Start()
    {
        int score = ResourceManager.HighScore;
        _highScoreText.text = "High Score: " + score.ToString();
        _scoreText.text = "0";
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {
                        _knife.Chop();
                        _currentScore++;
                        _scoreText.text = _currentScore.ToString();
                        if (_currentScore > ResourceManager.HighScore)
                        {
                            ResourceManager.HighScore = _currentScore;
                            _highScoreText.text = "High Score: " + _currentScore.ToString();
                        }
                    }
                }
            }
        }
    }
    
    private void Check()
    {
        if (_currentScore > 99)
        {
            ResourceManager.FruitCutting++;
            GameUIManager.Instance.CompletedLevel(true);
        }
        else
        {
            GameUIManager.Instance.Retry(true);
        }
    }
}