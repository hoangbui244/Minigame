using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCandyController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    private bool _locked;
    
    private void OnEnable()
    {
        SetupLevel();
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
            ResourceManager.BreakCandy = index;
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
        int num = ResourceManager.BreakCandy - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}
