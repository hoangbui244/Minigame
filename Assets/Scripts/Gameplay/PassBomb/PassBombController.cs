using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassBombController : MonoBehaviour
{
    [SerializeField] private List<PetControler> _pets;
    
    private void OnEnable()
    {
        GameEventManager.NextBomb += NextBomb;
        RandomBomb();
    }
    
    private void OnDisable()
    {
        GameEventManager.NextBomb -= NextBomb;
    }
    
    private void RandomBomb()
    {
        int random = Random.Range(0, _pets.Count);
        _pets[random].GetBomb();
    }
    
    private void NextBomb(int id)
    {
        if (id == _pets.Count - 1)
        {
            _pets[0].GetBomb();
            return;
        }
        _pets[id + 1].GetBomb();
    }
}
