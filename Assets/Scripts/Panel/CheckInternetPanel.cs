using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInternetPanel : MonoBehaviour
{
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
    }

    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
    }
    
    public void Check()
    {
        AudioManager.PlaySound("Click");

        if (IsInternetAvailable())
        {
            gameObject.SetActive(false);
            MainUIMananger.Instance.PopupOpened = false;
        }
    }

    private bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
