using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsPanel : MonoBehaviour
{
    public void Close()
    {
        AudioManager.PlaySound("Click");
        gameObject.SetActive(false);
    }
}
