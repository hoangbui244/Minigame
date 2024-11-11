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
    private bool _locked;
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
        if (index == 11)
        {
            _locked = true;
        }
        else
        {
            _locked = false;
        }
        if (!_locked)
        {
            ResourceManager.PerfectSlices = index;
            GameUIManager.Instance.Reload();
        }
        else
        {
            Debug.LogError("Watch Ads");
            //GameUIManager.Instance.WatchAds();
        }
    }
    
    private void SetupLevel()
    {
        int num = ResourceManager.PerfectSlices - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}
