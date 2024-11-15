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
    [SerializeField] private List<GameObject> _ads;
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
        int levelKey = 0;

        if (index == 3) levelKey = 1;
        else if (index == 6) levelKey = 2;
        else if (index == 10) levelKey = 3;

        if (levelKey != 0)
        {
            if (PlayerPrefs.GetInt(levelKey.ToString(), 0) == 0)
            {
                GameUIManager.Instance.WatchAds();
                MainUIMananger.Instance.LevelUnlocked = levelKey;
                MainUIMananger.Instance.LevelUnlockedIndex = index;
            }
            else
            {
                ResourceManager.BalanceEgg = index;
                GameUIManager.Instance.Reload();
            }
        }
        else
        {
            ResourceManager.BalanceEgg = index;
            GameUIManager.Instance.Reload();
        }
    }

    
    private void SetupLevel()
    {
        _ads[0].SetActive(!PlayerPrefs.HasKey("1"));
        _ads[1].SetActive(!PlayerPrefs.HasKey("2"));
        _ads[2].SetActive(!PlayerPrefs.HasKey("3"));

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