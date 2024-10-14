using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairCheck : MonoBehaviour
{
    [SerializeField] private List<Card> _list;
    private bool _pairCheck;
    private int _pairID;
    

    private void OnEnable()
    {
        GameEventManager.FlipCard += UpdateResult;
    }

    private void OnDisable()
    {
        GameEventManager.FlipCard -= UpdateResult;
    }

    private void UpdateResult(int id)
    {
        if (!_pairCheck)
        {
            _pairID = 0;
            _pairID = id;
            _pairCheck = true;
        }
        else
        {
            if (_pairID == id)
            {
                foreach (var item in _list)
                {
                    if (item.Flipped && !item.Finished && item.Id == _pairID)
                    {
                        item.Finished = true;
                        item.End();
                        GameEventManager.Check?.Invoke(true);
                        Card.PairCount = 0;
                    }
                }
            }
            else
            {
                foreach (var item in _list)
                {
                    if (item.Flipped && !item.Finished)
                    {
                        item.FlipDown();
                        item.Flipped = false;
                    }
                }
            }
            _pairID = 0;
            _pairCheck = false;
        }
    }
}