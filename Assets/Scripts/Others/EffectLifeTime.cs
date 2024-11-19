using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{
    private readonly WaitForSeconds _wait = new WaitForSeconds(2f);
    private void OnEnable()
    {
        StartCoroutine(Off());
    }
    
    private IEnumerator Off()
    {
        yield return _wait;
        gameObject.SetActive(false);
    }
}
