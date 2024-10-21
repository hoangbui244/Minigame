using UnityEngine.SceneManagement;

public class GameUIManager : Singleton<GameUIManager>
{
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
}