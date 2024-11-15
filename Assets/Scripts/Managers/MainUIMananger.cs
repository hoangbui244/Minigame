using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIMananger : Singleton<MainUIMananger>
{
    #region =========================== PROPERTIES ===========================

    private static SceneTransition Scene => Instance?.GetComponent<SceneTransition>();
    public bool PopupOpened;
    public int LevelTypeToLoad;
    [HideInInspector] public int LevelUnlocked;
    [HideInInspector] public int LevelUnlockedIndex;
    [SerializeField] private GameObject _mask;
    [SerializeField] private GameObject _dog;
    [SerializeField] private float _time;
    private readonly Vector3 _maskScaleEnd = new Vector3(25, 25, 1);
    private readonly Vector3 _dogScaleEnd = new Vector3(0.8f, 0.8f, 1);

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

    public void SceneEnd()
    {
        _mask.transform.DOScale(0, _time).SetEase(Ease.Linear);
        _dog.transform.DOScale(_dogScaleEnd, _time * 0.6f).SetEase(Ease.Linear);
    }

    public void SceneStart()
    {
        _dog.transform.DOScale(0, _time * 0.6f).SetEase(Ease.Linear);
        _mask.transform.DOScale(_maskScaleEnd, _time).SetEase(Ease.Linear).OnComplete(() =>
        {
            GameManager.Instance.GamePause(false);
        });
    }

    #endregion
}