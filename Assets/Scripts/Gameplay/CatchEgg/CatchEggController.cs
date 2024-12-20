using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatchEggController : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPos;
    [SerializeField] private TextMeshProUGUI _point;
    [SerializeField] private Color _color;
    private readonly WaitForSeconds _time = new WaitForSeconds(1.1f);
    private readonly WaitForSeconds _wait = new WaitForSeconds(0.5f);
    private int _currentPoint = 0;
    private bool _start = true;
    private Coroutine _spawnCoroutine;

    private void OnEnable()
    {
        GameEventManager.CatchEgg += Check;
        _point.text = "0";
        _currentPoint = 0;
        _start = true;
        _spawnCoroutine = StartCoroutine(Spawn());
    }
    
    private void OnDisable()
    {
        GameEventManager.CatchEgg -= Check;
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }
    }
    
    private IEnumerator Spawn()
    {
        while (_start)
        {
            yield return _time;
            int random = Random.Range(0, _spawnPos.Count);
            GameObject egg = ObjectPooler.Instance.SpawnFromPool("Egg", _spawnPos[random].position);
            egg.SetActive(true);
            egg.GetComponent<Egg>().UpSpeed();
        }
    }
    
    private void Check(int value)
    {
        _currentPoint += value;
        _point.text = _currentPoint.ToString();
        if (_currentPoint > 0)
        {
            _point.color = _color; 
        }
        if (_currentPoint >= 6)
        {
            _start = false;

            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }

            StartCoroutine(NewLevel());
        }
    }

    private IEnumerator NewLevel()
    {
        yield return _wait;
        GameUIManager.Instance.ScreenShot();
        yield return _wait;
        GameUIManager.Instance.CompletedLevel(true);
    }
}
