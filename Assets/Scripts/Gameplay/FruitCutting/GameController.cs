using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private Knife _knife;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private List<GameObject> _borders;
    [SerializeField] private List<GameObject> _ads;
    private readonly WaitForSeconds _wait = new WaitForSeconds(0.3f);
    private int _currentScore;

    private void OnEnable()
    {
        GameEventManager.FruitCutting += Check;
        SetupLevel();
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

    public void NextLevel(int index)
    {
        int levelKey = 0;

        if (index == 3) levelKey = 12;
        else if (index == 6) levelKey = 13;

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
                ResourceManager.FruitCutting = index;
                GameUIManager.Instance.Reload();
            }
        }
        else
        {
            ResourceManager.FruitCutting = index;
            GameUIManager.Instance.Reload();
        }
    }

    private void SetupLevel()
    {
        _ads[0].SetActive(!PlayerPrefs.HasKey("12"));
        _ads[1].SetActive(!PlayerPrefs.HasKey("13"));
        
        int num = ResourceManager.FruitCutting - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }

    private void Update()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        isPointerOverUI = true;
                        break;
                    }

                    if (!isPointerOverUI && touch.phase == TouchPhase.Began)
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
        if (ResourceManager.FruitCutting < 7)
        {
            ResourceManager.FruitCutting++;
        }
        else
        {
            ResourceManager.FruitCutting = 1;
        }
        GameUIManager.Instance.ScreenShot();
        StartCoroutine(NewLevel());
    }

    private IEnumerator NewLevel()
    {
        yield return _wait;
        GameUIManager.Instance.CompletedLevel(true);
    }
}