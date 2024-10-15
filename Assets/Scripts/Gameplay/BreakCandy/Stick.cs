using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }
    
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !MainUIMananger.Instance.PopupOpened)
        {
            //AudioManager.PlaySound("PickUp");
            //AudioManager.PlayVibration(true);
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = _camera.ScreenToWorldPoint(mousePos);
            gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, gameObject.transform.localPosition.z);
        }
    }

    private void OnMouseDrag()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            float cameraHalfHeight = _camera.orthographicSize;
            float targetY = Mathf.Clamp(mousePos.y , -cameraHalfHeight , cameraHalfHeight );
            Vector3 targetPosition = new Vector3(mousePos.x, targetY, this.gameObject.transform.localPosition.z);
            this.gameObject.transform.position = Vector3.Lerp(this.transform.localPosition, targetPosition, 0.95f);
        }
    }

    private void OnMouseUp()
    {
        if (!MainUIMananger.Instance.PopupOpened)
        {
           
        }
    }
}
