using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swaper : MonoBehaviour
{
    [SerializeField] private List<Transform> _list;
    void Start()
    {
        for (int i = 0; i < _list.Count; i++)
        {
            ObjectPooler.Instance.SpawnFromPool("Differ", _list[i].position);
        }
    }
}