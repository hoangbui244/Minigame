using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject _completedPanel;
    [SerializeField] private GameObject _replayPanel;

    private void OnEnable()
    {
        _replayPanel.SetActive(false);
        _completedPanel.SetActive(false);
    }

    public void Reload()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainUIMananger.LoadScene("Gameplay");
    }

    public void Back()
    {
        ObjectPooler.Instance.MoveToPool();
        MainUIMananger.LoadScene("HomeScreen");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
}