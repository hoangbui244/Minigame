using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCandyController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    [SerializeField] private List<GameObject> _ads;
    
    private void OnEnable()
    {
        SetupLevel();
    }
    
    public void NextLevel(int index)
    {
        int levelKey = 0;

        if (index == 3) levelKey = 4;
        else if (index == 6) levelKey = 5;
        else if (index == 8) levelKey = 15;
        else if (index == 10) levelKey = 6;
        
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
                ResourceManager.BreakCandy = index;
                GameUIManager.Instance.Reload();
            }
        }
        else
        {
            ResourceManager.BreakCandy = index;
            GameUIManager.Instance.Reload();
        }
    }
    
    private void SetupLevel()
    {
        _ads[0].SetActive(!PlayerPrefs.HasKey("4"));
        _ads[1].SetActive(!PlayerPrefs.HasKey("5"));
        _ads[2].SetActive(!PlayerPrefs.HasKey("6"));
        _ads[3].SetActive(!PlayerPrefs.HasKey("15"));
        
        int num = ResourceManager.BreakCandy - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}
