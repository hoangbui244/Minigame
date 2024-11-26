using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamChickenController : MonoBehaviour
{
    [SerializeField] private ChickenController _chickenController;
    [SerializeField] private GameObject _choicePanel;
    private readonly WaitForSeconds _wait = new(0.3f);
    
    private void OnEnable()
    {
        _choicePanel.SetActive(true);
        if (Camera.main != null)
        {
            Camera.main.gameObject.AddComponent<CameraFollow>();
            Camera.main.gameObject.GetComponent<CameraFollow>().Obj = _chickenController.transform;
        }
    }

    private void OnDisable()
    {
        if (Camera.main != null)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                Destroy(cameraFollow); 
            }
        }
    }
    
    public void ChooseType(int type)
    {
        _chickenController.ChooseType(type);
        StartCoroutine(TurnPanelOff());
    }
    
    private IEnumerator TurnPanelOff()
    {
        yield return _wait;
        _choicePanel.SetActive(false);
        _chickenController.StartGame = true;
    }
}
