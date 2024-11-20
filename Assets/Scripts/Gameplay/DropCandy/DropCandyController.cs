using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropCandyController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _candyType;
    [SerializeField] private CandyHolder _candyHolder;
    [SerializeField] private int _currentPoint;
    [SerializeField] private TextMeshProUGUI _point;
    [SerializeField] private TextMeshProUGUI _timer;
    private readonly WaitForSeconds _wait = new WaitForSeconds(1.5f);
    private readonly WaitForSeconds _waitTime = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds _countdown = new WaitForSeconds(1f);
    private Vector2 _pos = new Vector2(0, 25f);
    private bool _start = true;
    private Coroutine _spawnCoroutine;
    private Coroutine _timerCoroutine;
    private int _elapsedTime;

    private void OnEnable()
    {
        _canvas.worldCamera = Camera.main;
        GameEventManager.DropCandy += Check;
        _start = true;
        _candyHolder.CandyType = _candyType;
        _spawnCoroutine = StartCoroutine(SpawnCandy());
        _timerCoroutine = StartCoroutine(CountdownTimer(30));
        _elapsedTime = 0;
    }
    
    private void OnDisable()
    {
        GameEventManager.DropCandy -= Check;
        
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }
    
    private void Check(int value)
    {
        _currentPoint += value;
        _point.text = "X" + _currentPoint.ToString();
        
        if (_currentPoint <= 0)
        {
            _candyHolder.gameObject.GetComponent<Collider2D>().enabled = false;
            _start = false;

            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }

            if (ResourceManager.DropCandy < 8)
            {
                ResourceManager.DropCandy++;
            }
            else
            {
                ResourceManager.DropCandy = 1;
            }
            GameUIManager.Instance.Effect(true);
            StartCoroutine(NewLevel());
        }
    }
    
    private IEnumerator NewLevel()
    {
        yield return _waitTime;
        GameUIManager.Instance.ScreenShot();
        yield return _waitTime;
        GameUIManager.Instance.Effect(false);  
        GameUIManager.Instance.CompletedLevel(true);
    }
    
    private IEnumerator SpawnCandy()
    {
        while (_start)
        {
            yield return _wait;
            var num = Random.Range(-12f, 12f);
            _pos.x = num;
            var candy = ObjectPooler.Instance.SpawnFromPool("Candy", _pos);
            candy.SetActive(true);
            var script = candy.GetComponent<Candy>();
            script.InitCandy(_candyType);
            script.UpSpeed();
        }
    }

    private IEnumerator CountdownTimer(int seconds)
    {
        int timeRemaining = seconds;
        
        while (timeRemaining > 0)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
            _timer.text = timeSpan.ToString(@"mm\:ss");
            yield return _countdown;
            timeRemaining--;
            _elapsedTime++;
        }
        
        _start = false;
        _timer.text = "00:00";
        
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
        
        GameUIManager.Instance.Retry(true);
    }
}
