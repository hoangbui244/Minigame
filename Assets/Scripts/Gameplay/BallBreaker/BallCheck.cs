using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCheck : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects;
    [SerializeField] private List<Obstacle> _obstacles;
    private WaitForSeconds _wait = new WaitForSeconds(1.5f);
    
    private void OnEnable()
    {
        GameEventManager.BallBreaker += Check;
        GameEventManager.ResetLevel += Reset;
        Reset();
    }
    
    private void OnDisable()
    {
        GameEventManager.BallBreaker -= Check;
        GameEventManager.ResetLevel -= Reset;
    }
    
    private void Reset()
    {
        foreach (var obj in _obstacles)
        {
            obj.Reset();
        }
    }
    
    private void Check()
    {
        foreach (var obj in _obstacles)
        {
            if (!obj.IsFinish) return;
        }
        StartCoroutine(Delay());
    }
    
    private IEnumerator Delay()
    {
        GameManager.Instance.GameState = GameManager.EnumGameState.Finish;
        yield return _wait;
        if (ResourceManager.BallBreaker < 10)
        {
            ResourceManager.BallBreaker++;
        }
        else
        {
            ResourceManager.BallBreaker = 1;
        }
        GameUIManager.Instance.Confetti(true);
    }
}