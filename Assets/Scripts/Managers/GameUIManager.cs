using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject _completedPanel;
    [SerializeField] private GameObject _completedPanel1;
    [SerializeField] private GameObject _replayPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private RawImage _screenShot;
    
    private Texture2D _screenshot;
    private WaitForSeconds _wait = new WaitForSeconds(2f);

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

    public void CompletedLevel(bool active)
    {
        _completedPanel.SetActive(active);
    }

    public void CompletedLevel1(bool active)
    {
        _completedPanel1.SetActive(active);
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