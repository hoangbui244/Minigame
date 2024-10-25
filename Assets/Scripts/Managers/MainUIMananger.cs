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
    [SerializeField] private GameObject _sceneTrans;
    [SerializeField] private RectTransform _circle;
    [SerializeField] private RectTransform _dog;
    [SerializeField] private RectTransform _circleChild;
    
    private Vector3 _circleChildInitialScale;
    private Vector3 _inverseScale;
    private Vector3 _cirScale = new Vector3(30f, 30f, 1);
    private Vector3 _dogScale = new Vector3(0.8f, 0.8f, 1);
    private Vector3 _cirEndScale = new Vector3(0.01f, 0.01f, 1);
    private Vector3 _cirEndChildScale = new Vector3(100f, 100f, 1);
    
    #endregion

    #region =========================== UNITY CORES ===========================

    private void Start()
    {
        _circleChildInitialScale = _circleChild.localScale;
        _inverseScale = new Vector3();
    }
    
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
        _sceneTrans.SetActive(true);

        _circle.DOScale(_cirEndScale, 0.6f);
        _circleChild.DOScale(_cirEndChildScale, 0.6f);
        _dog.DOScale(_dogScale, 0.8f);
    }

    public void SceneStart()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.8f);
        _circleChild.localScale = _cirEndScale;
        _circle.DOScale(_cirScale, 0.8f).OnUpdate(() =>
        {
            Vector3 parentScale = _circle.localScale;
            _inverseScale.x = _circleChildInitialScale.x / parentScale.x;
            _inverseScale.y = _circleChildInitialScale.y / parentScale.y;
            _inverseScale.z = _circleChildInitialScale.z / parentScale.z;

            _circleChild.localScale = _inverseScale;
        });

        _dog.DOScale(Vector3.zero, 0.6f).OnComplete(() =>
        {
            _sceneTrans.SetActive(false);
        });
    }
    
    #endregion

}
