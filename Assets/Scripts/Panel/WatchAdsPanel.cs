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
        if (AdsManager.Instance.GetVersionCode())
        {
            AdsManager.Instance.ShowRewarded(done =>
            {
                if (done)
                {
                    var num = MainUIMananger.Instance.LevelUnlocked.ToString();
                    PlayerPrefs.SetInt(num, 1);
                    gameObject.SetActive(false);
                    switch (MainUIMananger.Instance.LevelTypeToLoad)
                    {
                        case 5:
                            ResourceManager.BreakCandy = MainUIMananger.Instance.LevelUnlockedIndex;
                            FirebaseManager.Instance.LogEventNameWithParam("BreakCandy", "level",
                                MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                            break;
                        case 7:
                            ResourceManager.CrocodileDentist = MainUIMananger.Instance.LevelUnlockedIndex;
                            FirebaseManager.Instance.LogEventNameWithParam("CrocodileDentist", "level",
                                MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                            break;
                        case 8:
                            ResourceManager.CutInHalf = MainUIMananger.Instance.LevelUnlockedIndex;
                            FirebaseManager.Instance.LogEventNameWithParam("CutInHalf", "level",
                                MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                            break;
                        case 10:
                            ResourceManager.FruitCutting = MainUIMananger.Instance.LevelUnlockedIndex;
                            FirebaseManager.Instance.LogEventNameWithParam("FruitCutting", "level",
                                MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                            break;
                        case 11:
                            ResourceManager.PerfectSlices = MainUIMananger.Instance.LevelUnlockedIndex;
                            FirebaseManager.Instance.LogEventNameWithParam("PerfectSlices", "level",
                                MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                            break;
                        case 17:
                            ResourceManager.BalanceEgg = MainUIMananger.Instance.LevelUnlockedIndex;
                            FirebaseManager.Instance.LogEventNameWithParam("BalanceEgg", "level",
                                MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                            break;
                    }

                    GameUIManager.Instance.Reload();
                }
            });
        }
        else
        {
            var num = MainUIMananger.Instance.LevelUnlocked.ToString();
            PlayerPrefs.SetInt(num, 1);
            gameObject.SetActive(false);
            switch (MainUIMananger.Instance.LevelTypeToLoad)
            {
                case 5:
                    ResourceManager.BreakCandy = MainUIMananger.Instance.LevelUnlockedIndex;
                    FirebaseManager.Instance.LogEventNameWithParam("BreakCandy", "level",
                        MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                    break;
                case 7:
                    ResourceManager.CrocodileDentist = MainUIMananger.Instance.LevelUnlockedIndex;
                    FirebaseManager.Instance.LogEventNameWithParam("CrocodileDentist", "level",
                        MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                    break;
                case 8:
                    ResourceManager.CutInHalf = MainUIMananger.Instance.LevelUnlockedIndex;
                    FirebaseManager.Instance.LogEventNameWithParam("CutInHalf", "level",
                        MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                    break;
                case 10:
                    ResourceManager.FruitCutting = MainUIMananger.Instance.LevelUnlockedIndex;
                    FirebaseManager.Instance.LogEventNameWithParam("FruitCutting", "level",
                        MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                    break;
                case 11:
                    ResourceManager.PerfectSlices = MainUIMananger.Instance.LevelUnlockedIndex;
                    FirebaseManager.Instance.LogEventNameWithParam("PerfectSlices", "level",
                        MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                    break;
                case 17:
                    ResourceManager.BalanceEgg = MainUIMananger.Instance.LevelUnlockedIndex;
                    FirebaseManager.Instance.LogEventNameWithParam("BalanceEgg", "level",
                        MainUIMananger.Instance.LevelUnlockedIndex.ToString());
                    break;
            }
            
            GameUIManager.Instance.Reload();
        }
    }
}