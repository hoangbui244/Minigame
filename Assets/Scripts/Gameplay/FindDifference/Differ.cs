using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Differ : MonoBehaviour
{
    public bool IsTrapped;

    private void OnMouseDown()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            // AudioManager.PlaySound("Tooth");
            // AudioManager.PlayVibration(true);
            if (IsTrapped)
            {
                GameUIManager.Instance.CompletedLevel(true);
            }
        }
    }
}
