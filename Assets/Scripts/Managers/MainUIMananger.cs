using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIMananger : Singleton<MainUIMananger>
{
    #region =========================== PROPERTIES ===========================

    public static SceneTransition Scene => Instance?.GetComponent<SceneTransition>();
    public bool PopupOpened;

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
