using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCardLevel : MonoBehaviour
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
            ResourceManager.FlipCard++;
            //GameUIManager.Instance.EndGame();
        }
    }
}