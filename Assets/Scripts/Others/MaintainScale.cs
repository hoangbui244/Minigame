using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainScale : MonoBehaviour
{
    private Vector3 _initialScale;

    void Start()
    {
        _initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        Vector3 parentScale = transform.parent.localScale;
        transform.localScale = new Vector3(_initialScale.x / parentScale.x, _initialScale.y / parentScale.y, _initialScale.z / parentScale.z);
    }
}
