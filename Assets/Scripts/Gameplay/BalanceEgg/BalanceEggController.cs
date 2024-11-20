using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BalanceEggController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private BalanceEgg _balanceEgg;
    [SerializeField] private List<GameObject> _ads;
    private float _currentScore;
    private int _highScore;

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
        if (!MainUIMananger.Instance.PopupOpened && !IsPointerOverUI())
        {
            if (!_balanceEgg.IsStart && Input.GetMouseButtonDown(0))
            {
                _balanceEgg.IsStart = true;
            }

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
    }

    private bool IsPointerOverUI()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return true;
            }
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            return results.Count > 0;
        }

        return false;
    }

    public void NextLevel(int index)
    {
        int levelKey = 0;

        if (index == 3) levelKey = 1;
        else if (index == 5) levelKey = 18;
        else if (index == 6) levelKey = 2;
        else if (index == 8) levelKey = 19;
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
        _ads[3].SetActive(!PlayerPrefs.HasKey("18"));
        _ads[4].SetActive(!PlayerPrefs.HasKey("19"));

        _currentScore = 0;
        _currentScoreText.text = "00:00";
        _balanceEgg.IsStart = false;
        int num = ResourceManager.BalanceEgg - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}