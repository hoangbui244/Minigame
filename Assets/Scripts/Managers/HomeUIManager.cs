using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : Singleton<HomeUIManager>
{
    private int _levelTypeToLoad;

    public void LoadLevel(int type)
    {
        _levelTypeToLoad = type;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        MainUIMananger.LoadScene("Gameplay");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameEventManager.LoadLevel?.Invoke(_levelTypeToLoad);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}