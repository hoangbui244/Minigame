using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    public bool IsTrapped;

    private void OnMouseDown()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            AudioManager.PlaySound("CrocodileTouch");
            AudioManager.LightFeedback();
            GameEventManager.CheckTeeth?.Invoke(!IsTrapped);
            gameObject.SetActive(false);
        }
    }
}
