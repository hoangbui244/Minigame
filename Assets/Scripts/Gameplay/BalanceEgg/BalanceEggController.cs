using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalanceEggController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private BalanceEgg _balanceEgg;
    private float _currentScore;
    private int _highScore;
    private bool _locked;

    private void OnEnable()
    {
        SetupLevel();
        _highScore = ResourceManager.BalanceEggHighScore;
        _highScoreText.text = "Highscore: " + _highScore.ToString();
        GameEventManager.BalanceEgg += Check;
    }
    
    private void OnDisable()
    {
        GameEventManager.BalanceEgg -= Check;
    }
    
    private void Check()
    {
        if (_currentScore < _highScore)
        {
            GameUIManager.Instance.ReplayLose(Mathf.FloorToInt(_currentScore));
        }
        else
        {
            GameUIManager.Instance.ReplayWin();
        }
    }

    private void Update()
    {
        if (_balanceEgg.IsHolded)
        {
            _currentScore += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(_currentScore);
            string formattedTime = timeSpan.ToString(@"mm\:ss");

            _currentScoreText.text = formattedTime;

            if (_currentScore > _highScore)
            {
                _highScore = (int)_currentScore;
                ResourceManager.BalanceEggHighScore = _highScore;
            }
        }
    }
    
    public void NextLevel(int index)
    {
        if (index == 11)
        {
            _locked = true;
        }
        else
        {
            _locked = false;
        }
        if (!_locked)
        {
            ResourceManager.BalanceEgg = index;
            GameUIManager.Instance.Reload();
        }
        else
        {
            Debug.LogError("Watch Ads");
        }
    }
    
    private void SetupLevel()
    {
        _currentScore = 0;
        _currentScoreText.text = "00:00";
        int num = ResourceManager.BalanceEgg - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}