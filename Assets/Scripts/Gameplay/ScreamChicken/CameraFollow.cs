using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    public Transform Obj;
    public float Smoothing = 11f;

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(Obj.position.x + 5f, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Smoothing * Time.deltaTime);
    }
}