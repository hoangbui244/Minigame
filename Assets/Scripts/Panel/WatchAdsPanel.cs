using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdsPanel : MonoBehaviour
{
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
    }

    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
    }

    public void WatchAds()
    {
        AudioManager.PlaySound("Click");
        AdsManager.Instance.ShowRewarded(done =>
        {
            if (done)
            {
                var num = MainUIMananger.Instance.LevelUnlocked.ToString();
                PlayerPrefs.SetInt(num, 1);
                gameObject.SetActive(false);
                switch (MainUIMananger.Instance.LevelTypeToLoad)
                {
                    case 17:
                        ResourceManager.BalanceEgg = MainUIMananger.Instance.LevelUnlockedIndex;
                        break;
                }
                GameUIManager.Instance.Reload();
            }
        });
    }
}