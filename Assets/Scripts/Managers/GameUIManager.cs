using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject _completedPanel;
    [SerializeField] private GameObject _replayPanel;
    private WaitForSeconds _wait = new WaitForSeconds(1.2f);

    private void OnEnable()
    {
        _replayPanel.SetActive(false);
        _completedPanel.SetActive(false);
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
        ObjectPooler.Instance.MoveToPool();
        MainUIMananger.Instance.SceneEnd();
        StartCoroutine(LoadScene(1));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ObjectPooler.Instance.MoveToPool();
        int currentLevelType = MainUIMananger.Instance.LevelTypeToLoad;
        GameEventManager.LoadLevel?.Invoke(currentLevelType);
        MainUIMananger.Instance.SceneStart();
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