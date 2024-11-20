using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PerfectSliceController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    [SerializeField] private List<GameObject> _slices;
    [SerializeField] private List<Vector2> _pos;
    [SerializeField] private List<Vector3> _resetPos;
    [SerializeField] private float _time;
    [SerializeField] private List<GameObject> _ads;
    private readonly WaitForSeconds _wait = new WaitForSeconds(0.2f);

    private void OnEnable()
    {
        SetupLevel();
        GameEventManager.PerfectSlices += Check;
        GameEventManager.PerfectSlicesReset += ResetLevel;
    }
    
    private void OnDisable()
    {
        GameEventManager.PerfectSlices -= Check;
        GameEventManager.PerfectSlicesReset -= ResetLevel;
    }

    private void OnValidate()
    {
        _resetPos.Clear();
        foreach (var item in _slices)
        {
            _resetPos.Add(item.transform.localPosition);
        }
    }

    private void ResetLevel()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return _wait;
        for (int i = 0; i < _slices.Count; i++)
        {
            _slices[i].transform.localPosition = _resetPos[i];
        }
    }
    
    private void Check(int value)
    {
        int count = value / 2 - 1;
        _slices[count].transform.DOLocalMove(_pos[count], _time);
    }

    public void NextLevel(int index)
    {
        int levelKey = 0;

        if (index == 3) levelKey = 14;
        else if (index == 5) levelKey = 17;

        if (levelKey != 0)
        {
            if (PlayerPrefs.GetInt(levelKey.ToString(), 0) == 0)
            {
                GameUIManager.Instance.WatchAds();
                MainUIMananger.Instance.LevelUnlocked = levelKey;
                MainUIMananger.Instance.LevelUnlockedIndex = index;
            }
            else
            {
                ResourceManager.PerfectSlices = index;
                GameUIManager.Instance.Reload();
            }
        }
        else
        {
            ResourceManager.PerfectSlices = index;
            GameUIManager.Instance.Reload();
        }
    }
    
    private void SetupLevel()
    {
        _ads[0].SetActive(!PlayerPrefs.HasKey("14"));
        _ads[1].SetActive(!PlayerPrefs.HasKey("17"));
        
        int num = ResourceManager.PerfectSlices - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}
