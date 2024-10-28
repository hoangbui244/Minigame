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
    private int _currentScore;
    private bool _locked;

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
        if (index == 3 || index == 6)
        {
            _locked = true;
        }
        else
        {
            _locked = false;
        }
        if (!_locked)
        {
            ResourceManager.FruitCutting = index;
            GameUIManager.Instance.Reload();
        }
        else
        {
            Debug.LogError("Watch Ads");
            //GameUIManager.Instance.WatchAds();
        }
    }
    
    private void SetupLevel()
    {
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
        if (_currentScore > 2)
        {
            ResourceManager.FruitCutting++;
            GameUIManager.Instance.ScreenShot();
            StartCoroutine(NewLevel());
        }
        else
        {
            GameUIManager.Instance.Retry(true);
        }
    }
    
    private IEnumerator NewLevel()
    {
        yield return new WaitForSeconds(0.3f);
        GameUIManager.Instance.CompletedLevel(true);
    }
}
