using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilTapController : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventManager.PencilTap += Check;
    }
    
    private void OnDisable()
    {
        GameEventManager.PencilTap -= Check;
    }
    
    private void Check(int value)
    {
        
    }
}
