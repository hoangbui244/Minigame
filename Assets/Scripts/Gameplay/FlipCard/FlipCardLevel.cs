using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCardLevel : MonoBehaviour
{
    [SerializeField] private List<bool> _winCondition;
    [SerializeField] private int _num;
    [SerializeField] private GameObject _text;
    
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
            _text.SetActive(false);
            if (ResourceManager.FlipCard < 10)
            {
                ResourceManager.FlipCard++;
            }
            else
            {
                ResourceManager.FlipCard = 1;
            }
            GameUIManager.Instance.Confetti(true);
        }
    }
}
