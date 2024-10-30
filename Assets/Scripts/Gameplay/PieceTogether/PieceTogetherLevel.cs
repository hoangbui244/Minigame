using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTogetherLevel : MonoBehaviour
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
            if (ResourceManager.PieceTogether < 5)
            {
                ResourceManager.PieceTogether++;
            }
            else
            {
                ResourceManager.PieceTogether = 1;
            }
            GameUIManager.Instance.CompletedLevel1(true);
        }
    }
}
