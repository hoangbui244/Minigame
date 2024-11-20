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
    [SerializeField] private List<GameObject> _ads;
    [SerializeField] private List<GameObject> _borders;
    private SpriteRenderer _petSprite;
    private int _currentWinCount = 0;
    private WaitForSeconds _wait = new WaitForSeconds(1.2f);
    private WaitForSeconds _vfxTime = new WaitForSeconds(0.4f);
    
    private void OnEnable()
    {
        SetupLevel();
        RandomTeeth();
        _petSprite = _pet.GetComponent<SpriteRenderer>();
        GameEventManager.CheckTeeth += CheckTeeth;
    }
    
    private void OnDisable()
    {
        GameEventManager.CheckTeeth -= CheckTeeth;
    }
    
    private void SetupLevel()
    {
        _ads[0].SetActive(!PlayerPrefs.HasKey("7"));
        _ads[1].SetActive(!PlayerPrefs.HasKey("8"));
        _ads[2].SetActive(!PlayerPrefs.HasKey("16"));
        
        int num = ResourceManager.CrocodileDentist - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
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
                GameUIManager.Instance.Confetti(true);
            }
        }
        else
        {
            StartCoroutine(PlayAnim());
        }
    }
    
    private IEnumerator PlayAnim()
    {
        AudioManager.PlaySound("CrocodileCrack");
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
        int levelKey = 0;

        if (index == 3) levelKey = 7;
        else if (index == 5) levelKey = 16;
        else if (index == 7) levelKey = 8;
        
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
                ResourceManager.CrocodileDentist = index;
                GameUIManager.Instance.Reload();
            }
        }
        else
        {
            ResourceManager.CrocodileDentist = index;
            GameUIManager.Instance.Reload();
        }
    }
}
