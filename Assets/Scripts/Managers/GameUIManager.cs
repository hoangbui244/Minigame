using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject _completedPanel;
    [SerializeField] private GameObject _replayPanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private RawImage _screenShot;
    
    // [SerializeField] private Camera _screenshotCam;
    // private Texture2D _texture;
    private Texture2D _screenshot;
    private WaitForSeconds _wait = new WaitForSeconds(2f);

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        _completedPanel.SetActive(false);
        _replayPanel.SetActive(false);
        _settingPanel.SetActive(false);
        // _screenshotCam.gameObject.SetActive(false);
    }

    // public Texture2D TakeScreenShot(int size = 512)
    // {
    //     RenderTexture renderTexture = new RenderTexture(size, size, 24);
    //     _screenshotCam.targetTexture = renderTexture;
    //     _screenshotCam.Render();
    //
    //     RenderTexture.active = renderTexture;
    //     Texture2D texture = new Texture2D(size, size, TextureFormat.RGB24, false);
    //     texture.ReadPixels(new Rect(0, 0, size, size), 0, 0);
    //     texture.Apply();
    //
    //     _screenshotCam.targetTexture = null;
    //     RenderTexture.active = null;
    //     Destroy(renderTexture);
    //
    //     return texture;
    // }

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
    
    public void SaveImage()
    {
        if (_screenshot != null)
        {
            byte[] bytes = _screenshot.EncodeToPNG();
            string directoryPath = Application.persistentDataPath + "/ScreenShots";
            System.IO.Directory.CreateDirectory(directoryPath);
            string filePath = directoryPath + "/Screenshot.png";
            System.IO.File.WriteAllBytes(filePath, bytes);
        }
    }
    
    public void DestroyTexture()
    {
        _screenshot = null;
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