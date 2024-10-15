using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differ : MonoBehaviour
{
    [SerializeField] private bool _isTrapped;
    
    private void OnMouseDown()
    {
        if (!_isTrapped && !MainUIMananger.Instance.PopupOpened)
        {
            // AudioManager.PlaySound("Tooth");
            // AudioManager.PlayVibration(true);
            // GameEventManager.CheckDiff?.Invoke(true);
        }
    }
}
