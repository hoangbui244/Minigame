using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassBombController : MonoBehaviour
{
    [SerializeField] private List<PetControler> _pets;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private GameObject _explode;
    [SerializeField] private int _countdownTime = 15; 
    private readonly Vector3 _radius = new Vector3(5f, 5f, 5f);
    private readonly WaitForSeconds _wait = new (1);
    private readonly WaitForSeconds _delay = new (0.2f);
    public float PassTime = 2f;
    
    private void OnEnable()
    {
        GameEventManager.NextBomb += NextBomb;
        GameEventManager.PassBomb += Check;
    }

    private void Start()
    {
        RandomBomb();
        SetupTime();
    }

    private void OnDisable()
    {
        GameEventManager.NextBomb -= NextBomb;
        GameEventManager.PassBomb -= Check;
    }
    
    private void Check()
    {
        foreach (var pet in _pets)
        {
            if (pet.IsExplode)
            {
                if (pet.IsBot)
                {
                    GameUIManager.Instance.ScreenShot();
                    if (ResourceManager.PassBomb < 5)
                    {
                        ResourceManager.PassBomb++;
                    }
                    else
                    {
                        ResourceManager.PassBomb = 1;
                    }
                    StartCoroutine(NewLevel());
                }
                else
                {
                    StartCoroutine(Retry());
                }
            }
        }
    }
    
    private IEnumerator NewLevel()
    {
        yield return _wait;
        GameUIManager.Instance.CompletedLevel(true);
    }

    private IEnumerator Retry()
    {
        yield return _wait;
        GameUIManager.Instance.Retry(true);
    }
    
    private void SetupTime()
    {
        StartCoroutine(Countdown());
    }
    
    private void RandomBomb()
    {
        int random = Random.Range(0, _pets.Count);
        _pets[random].GetBomb();
    }
    
    private void NextBomb(int id)
    {
        int nextId = id == _pets.Count - 1 ? 0 : id + 1;

        _pets[nextId].ReceiveBomb(); 
        _pets[nextId].GetBomb(); 
    }
    
    private IEnumerator Countdown()
    {
        while (_countdownTime > 0)
        {
            _time.text = "00:" + _countdownTime.ToString("D2");
            if (_countdownTime <= 7)
            {
                AudioManager.PlaySound("PassBombTick");
            }
            yield return _wait;
            _countdownTime--;
            PassTime = _countdownTime switch
            {
                15 => 1.5f,
                10 => 1f,
                5 => 0.5f,
                _ => PassTime
            };
        }
        _time.text = "00:00";
        yield return _delay;
        AnimExplode();
    }
    
    private void AnimExplode()
    {
        AudioManager.PlaySound("PassBombExplosion");
        _explode.SetActive(true);
        _explode.transform.DOScale(_radius, 0.5f).onComplete += () =>
        {
            _explode.SetActive(false);
            _explode.transform.localScale = Vector3.zero;
            ExplodeBomb();
        };
    }

    private void ExplodeBomb()
    {
        foreach (var pet in _pets)
        {
            if (pet.HasBomb)
            {
                StopAllCoroutines();
                pet.Explode();
                break;
            }
        }
    }
}