using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HomeUIManager : Singleton<HomeUIManager>
{
    [SerializeField] private RemoveAdsPanel _removeAdsPanel;
    [SerializeField] private SettingPanel _settingPanel;
    [SerializeField] private CheckInternetPanel _checkInternetPanel;
    private int _levelTypeToLoad;
    private readonly WaitForSeconds _wait = new(2f);

    private void OnEnable()
    {
        Init();
        // AdsManager.Instance.ShowBanner();
    }

    private void Init()
    {
        _checkInternetPanel.gameObject.SetActive(false);
        _removeAdsPanel.gameObject.SetActive(false);
        _settingPanel.gameObject.SetActive(false);
    }

    public void Setting()
    {
        AudioManager.PlaySound("Click");
        _settingPanel.gameObject.SetActive(true);
    }

    public void RemoveAds()
    {
        AudioManager.PlaySound("Click");
        _removeAdsPanel.gameObject.SetActive(true);
    }

    public void LoadLevel(int type)
    {
        AudioManager.PlaySound("Click");
        if (IsInternetAvailable())
        {
            _levelTypeToLoad = type;
            MainUIMananger.Instance.LevelTypeToLoad = type;

            SceneManager.sceneLoaded += OnSceneLoaded;
            MainUIMananger.Instance.SceneEnd();
            StartCoroutine(LoadScene());
        }
        else
        {
            _checkInternetPanel.gameObject.SetActive(true);
            MainUIMananger.Instance.PopupOpened = true;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ObjectPooler.Instance.MoveToPool();
        GameEventManager.LoadLevel?.Invoke(_levelTypeToLoad);
        MainUIMananger.Instance.SceneStart();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    private IEnumerator LoadScene()
    {
        yield return _wait;
        SceneManager.LoadScene(2);
    }

    public void OnClickRandom()
    {
        AudioManager.PlaySound("Click");
        int random = Random.Range(1, 19);
        LoadLevel(random);
    }
}