using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapAwayLevel : MonoBehaviour
{
    [SerializeField] private List<bool> _winCondition;
    [SerializeField] private int _num;
    
    private void OnEnable()
    {
        GameEventManager.Check += UpdateResult;
    }
    
    private void OnDisable()
    {
        GameEventManager.Check -= UpdateResult;
    }
    
    private void UpdateResult(bool result)
    {
        if (_winCondition.Count < _num)
        {
            _winCondition.Add(result);
        }
        else
        {
            if (ResourceManager.TapAway < 5)
            {
                ResourceManager.TapAway++;
            }
            else
            {
                ResourceManager.TapAway = 1;
            }
        }
    }
}
