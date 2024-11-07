using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject _completedPanel;
    [SerializeField] private GameObject _completedPanel1;
    [SerializeField] private GameObject _replayPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private RawImage _screenShot;
    [SerializeField] private List<Sprite> _lvSprites;
    [SerializeField] private GameObject _nextLv;
    [SerializeField] private Image _nextLvImage;
    
    private Texture2D _screenshot;
    private readonly Vector2 _scaleEnd = new Vector2(0.25f, 0.25f);
    private readonly WaitForSeconds _wait = new WaitForSeconds(2f);
    private readonly WaitForSeconds _wait1 = new WaitForSeconds(3f);
    
    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        _completedPanel.SetActive(false);
        _completedPanel1.SetActive(false);
        _replayPanel.SetActive(false);
        _settingPanel.SetActive(false);
        _nextLv.SetActive(false);
        _nextLv.transform.localScale = Vector2.zero;
        StartCoroutine(OpenNextLv());
    }
    
    private IEnumerator OpenNextLv()
    {
        yield return _wait1;
        _nextLv.SetActive(true);
        int num = MainUIMananger.Instance.LevelTypeToLoad;
        _nextLvImage.sprite = num < 10 ? _lvSprites[num] : _lvSprites[0];
        _nextLv.transform.DOScale(_scaleEnd, 0.5f);
    }

    private IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        int captureWidth = Mathf.Min(screenWidth, (int)(screenHeight / 1.4f));
        int captureHeight = (int)(captureWidth * 1.4f);
        int startX = (screenWidth - captureWidth) / 2;
        int startY = (screenHeight - captureHeight) / 2 + 220;
    
        _screenshot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        Rect rect = new Rect(startX, startY, captureWidth, captureHeight);
        _screenshot.ReadPixels(rect, 0, 0);
        _screenshot.Apply();

        _screenShot.texture = _screenshot;
    }
    
    public Texture2D GetScreenshot()
    {
        return _screenshot;
    }

    public void ScreenShot()
    {
        StartCoroutine(TakeScreenShot());
    }

    public void Setting()
    {
        _settingPanel.SetActive(true);
    }

    public void Reload()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainUIMananger.Instance.SceneEnd();
        StartCoroutine(LoadScene(2));
    }

    public void Back()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainUIMananger.Instance.SceneEnd();
        ObjectPooler.Instance.MoveToPool();
        StartCoroutine(LoadScene(1));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MainUIMananger.Instance.SceneStart();
        ObjectPooler.Instance.MoveToPool();
        int currentLevelType = MainUIMananger.Instance.LevelTypeToLoad;
        GameEventManager.LoadLevel?.Invoke(currentLevelType);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void NextGame()
    {
        if (MainUIMananger.Instance.LevelTypeToLoad == 12)
        {
            MainUIMananger.Instance.LevelTypeToLoad = 1;
        }
        else
        {
            MainUIMananger.Instance.LevelTypeToLoad++;
        }
        Reload();
    }
    
    public void CompletedLevel(bool active)
    {
        _completedPanel.SetActive(active);
    }

    public void CompletedLevel1(bool active)
    {
        _completedPanel1.SetActive(active);
        Invoke(nameof(Reload), 2f);
    }
    
    public void Retry(bool active)
    {
        _replayPanel.SetActive(active);
    }

    private IEnumerator LoadScene(int index)
    {
        yield return _wait;
        SceneManager.LoadScene(index);
    }
}