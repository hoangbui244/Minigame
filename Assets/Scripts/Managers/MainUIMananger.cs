using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIMananger : Singleton<MainUIMananger>
{
    #region =========================== PROPERTIES ===========================

    private static SceneTransition Scene => Instance?.GetComponent<SceneTransition>();
    public bool PopupOpened;
    public int LevelTypeToLoad;

    #endregion

    #region =========================== UNITY CORES ===========================

    private void OnEnable()
    {
        PopupOpened = false;
    }

    #endregion

    #region =========================== MAIN ===========================

    public static void LoadScene(string sceneName)
    {
        Scene.PerformTransition(sceneName);
    }
    
    #endregion

}
