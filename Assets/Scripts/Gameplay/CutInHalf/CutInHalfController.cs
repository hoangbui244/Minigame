using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutInHalfController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _borders;
    [SerializeField] private List<GameObject> _ads;
    private readonly WaitForSeconds _wait = new WaitForSeconds(2f);
    
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
        StartCoroutine(Wait(value));
    }

    private IEnumerator Wait(int value)
    {
        yield return _wait;
        if (value >= 47 && value <= 53)
        {
            if (ResourceManager.CutInHalf < 10)
            {
                ResourceManager.CutInHalf++;
            }
            else
            {
                ResourceManager.CutInHalf = 1;
            }
            GameUIManager.Instance.Confetti(true);
        }
        else
        {
            GameUIManager.Instance.Retry(true);
        }
    }
    
    public void NextLevel(int index)
    {
        int levelKey = 0;

        if (index == 3) levelKey = 9;
        else if (index == 6) levelKey = 10;
        else if (index == 10) levelKey = 11;

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
                ResourceManager.CutInHalf = index;
                GameUIManager.Instance.Reload();
            }
        }
        else
        {
            ResourceManager.CutInHalf = index;
            GameUIManager.Instance.Reload();
        }
    }
    
    private void SetupLevel()
    {
        _ads[0].SetActive(!PlayerPrefs.HasKey("9"));
        _ads[1].SetActive(!PlayerPrefs.HasKey("10"));
        _ads[2].SetActive(!PlayerPrefs.HasKey("11"));
        
        int num = ResourceManager.CutInHalf - 1;
        foreach (var border in _borders)
        {
            border.SetActive(false);
        }

        _borders[num].SetActive(true);
    }
}
