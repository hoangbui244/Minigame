using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPanel : MonoBehaviour
{
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
    }
    
    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
    }
}
