using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Swaper : MonoBehaviour
{
    [SerializeField] private List<Differ> _list;
    [SerializeField] private List<Sprite> _trappedSprite;
    [SerializeField] private List<Sprite> _defaultSprite;
    [SerializeField] private TextMeshProUGUI _point;
    private WaitForSeconds _wait = new WaitForSeconds(0.3f);

    private void OnEnable()
    {
        _point.text = "0";
        GameEventManager.FindDifference += NextLevel;
    }
    
    private void OnDisable()
    {
        GameEventManager.FindDifference -= NextLevel;
    }

    void Start()
    {
        SetRandom();
    }
    
    private void SetRandom()
    {
        int count = Random.Range(0, _list.Count);
        int num = ResourceManager.FindDifference;
        _list[count].IsTrapped = true;
        foreach (var item in _list)
        {
            item.GetComponent<SpriteRenderer>().sprite = item.IsTrapped ? _trappedSprite[num] : _defaultSprite[num];
        }
    }

    private void NextLevel()
    {
        _point.text = (int.Parse(_point.text) + 1).ToString();
        foreach (var item in _list)
        {
            item.gameObject.SetActive(false);
        }

        StartCoroutine(NewLevel());
    }
    
    private IEnumerator NewLevel()
    {
        yield return _wait;
        foreach (var item in _list)
        {
            item.gameObject.SetActive(true);
        }
        SetRandom();
    }
}