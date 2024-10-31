using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrocodileDentistCheck : MonoBehaviour
{
    [SerializeField] private List<Tooth> _teeth;
    [SerializeField] private int _winCount;
    [SerializeField] private GameObject _pet;
    [SerializeField] private Sprite _petBited;
    [SerializeField] private GameObject _vfx;
    private SpriteRenderer _petSprite;
    private int _currentWinCount = 0;
    private WaitForSeconds _wait = new WaitForSeconds(1.2f);
    private WaitForSeconds _vfxTime = new WaitForSeconds(0.4f);
    private bool _locked;
    
    private void OnEnable()
    {
        RandomTeeth();
        _petSprite = _pet.GetComponent<SpriteRenderer>();
        GameEventManager.CheckTeeth += CheckTeeth;
    }
    
    private void OnDisable()
    {
        GameEventManager.CheckTeeth -= CheckTeeth;
    }
    
    private void RandomTeeth()
    {
        int num = Random.Range(0, _teeth.Count);
        _teeth[num].IsTrapped = true;
    }
    
    private void CheckTeeth(bool isWin)
    {
        if (isWin)
        {
            _currentWinCount++;
            if (_currentWinCount >= _winCount)
            {
                if (ResourceManager.CrocodileDentist < 7)
                {
                    ResourceManager.CrocodileDentist++;
                }
                else
                {
                    ResourceManager.CrocodileDentist = 1;
                }
                GameUIManager.Instance.CompletedLevel1(true);
            }
        }
        else
        {
            StartCoroutine(PlayAnim());
        }
    }
    
    private IEnumerator PlayAnim()
    {
        _vfx.SetActive(true);
        _petSprite.sprite = _petBited;
        _petSprite.sortingOrder = 6;
        yield return _vfxTime;
        _vfx.SetActive(false);
        yield return _wait;
        GameUIManager.Instance.Retry(true);
    }
    
    public void NextLevel(int index)
    {
        if (index == 8)
        {
            _locked = true;
        }
        else
        {
            _locked = false;
        }
        if (!_locked)
        {
            ResourceManager.CrocodileDentist = index;
            GameUIManager.Instance.Reload();
        }
        else
        {
            Debug.LogError("Watch Ads");
            //GameUIManager.Instance.WatchAds();
        }
    }
}
