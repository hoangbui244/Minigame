using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class HomeUIManager : Singleton<HomeUIManager>
{
    [SerializeField] private RemoveAdsPanel _removeAdsPanel;
    [SerializeField] private SettingPanel _settingPanel;
    private int _levelTypeToLoad;

    private void OnEnable()
    {
        AdsManager.Instance.ShowBanner();
        Init();
    }
    
    private void Init()
    {
        _removeAdsPanel.gameObject.SetActive(false);
        _settingPanel.gameObject.SetActive(false);
    }
    
    public void Setting()
    {
        _settingPanel.gameObject.SetActive(true);
    }
    
    public void RemoveAds()
    {
        _removeAdsPanel.gameObject.SetActive(true);
    }
    
    public void LoadLevel(int type)
    {
        _levelTypeToLoad = type;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainUIMananger.LoadScene("Gameplay");
        MainUIMananger.Instance.LevelTypeToLoad = type;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ObjectPooler.Instance.MoveToPool();
        GameEventManager.LoadLevel?.Invoke(_levelTypeToLoad);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}