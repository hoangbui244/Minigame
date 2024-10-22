using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCandyCheck : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventManager.BreakCandy += UpdateResult;
    }
    
    private void OnDisable()
    {
        GameEventManager.BreakCandy -= UpdateResult;
    }
    
    private void UpdateResult(bool result)
    {
        if (!result)
        {
            StartCoroutine(Retry());
        }
    }
    
    private IEnumerator Retry()
    {
        yield return new WaitForSeconds(0.5f);
        GameUIManager.Instance.Retry(true);
    }
}
