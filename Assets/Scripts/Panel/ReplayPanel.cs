using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPanel : MonoBehaviour
{
    [SerializeField] private GameObject _nativeAds;
    
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
        if (ResourceManager.RemoveAds || !AdsManager.Instance.VersionTrue)
        {
            _nativeAds.SetActive(false);
        }
    }
    
    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
    }
}
