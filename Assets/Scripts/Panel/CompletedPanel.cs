using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompletedPanel : MonoBehaviour
{
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
    }
    
    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
    }

    public void Setting()
    {
        // Logic cho setting
    }

    public void SaveImage()
    {
        // Logic cho việc lưu ảnh
    }
}