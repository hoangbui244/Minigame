using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tooth : MonoBehaviour
{
    public bool IsTrapped;

    private void OnMouseDown()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            // AudioManager.PlaySound("Tooth");
            // AudioManager.PlayVibration(true);
            GameEventManager.CheckTeeth?.Invoke(!IsTrapped);
            gameObject.SetActive(false);
        }
    }
}
