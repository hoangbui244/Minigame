using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompletedPanel : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Home()
    {
        MainUIMananger.LoadScene("HomeScreen");
    }

    public void NextLevel()
    {
        switch (MainUIMananger.Instance.LevelTypeToLoad)
        {
            case 1:
                // Logic cho LevelType 1
                break;
            case 6:
                ResourceManager.FindDifference++;
                break;
        }

        MainUIMananger.LoadScene("Gameplay");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    { 
        ObjectPooler.Instance.MoveToPool();
        if (scene.buildIndex == 2)
        {
            int currentLevelType = MainUIMananger.Instance.LevelTypeToLoad;
            GameEventManager.LoadLevel?.Invoke(currentLevelType);
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Setting()
    {
        // Logic cho setting
    }

    public void SaveImage()
    {
        // Logic cho việc lưu ảnh
    }
}