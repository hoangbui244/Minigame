using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swaper : MonoBehaviour
{
    [SerializeField] private List<Differ> _list;
    [SerializeField] private Sprite _trappedSprite;
    void Start()
    {
        SetRandom();
    }
    
    private void SetRandom()
    {
        int num = Random.Range(0, _list.Count);
        _list[num].IsTrapped = true;
        foreach (var item in _list)
        {
            if (item.IsTrapped)
            {
                item.GetComponent<SpriteRenderer>().sprite = _trappedSprite;
            }
        }
    }
}