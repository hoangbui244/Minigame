using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutInHalfController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    private bool _locked;
    
    private void OnEnable()
    {
        SetupLevel();
        GameEventManager.CutInHalf += Check;
    }
    
    private void OnDisable()
    {
        GameEventManager.CutInHalf -= Check;
    }
    
    private void Check(int value)
    {
        if (value >= 48 || value <= 52)
        {
            ResourceManager.CutInHalf++;
            GameUIManager.Instance.CompletedLevel1(true);
        }
        else
        {
            GameUIManager.Instance.Retry(true);
        }
    }
    
    public void NextLevel(int index)
    {
        if (index == 3 || index == 6 || index == 10)
        {
            _locked = true;
        }
        else
        {
            _locked = false;
        }
        if (!_locked)
        {
            ResourceManager.CutInHalf = index;
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
        int num = ResourceManager.CutInHalf - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}
