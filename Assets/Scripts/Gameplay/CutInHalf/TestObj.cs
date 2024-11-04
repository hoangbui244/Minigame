using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TestObj : MonoBehaviour
{
    public GameObject Mask;
    private SpriteRenderer _spriteRenderer;
    private float _width;
    private GameObject _otherHalf;

    [Header("Setup objects")]
    private readonly Vector3 _obj1Rota = new Vector3(0, 0, -90);
    private readonly Vector3 _obj2Rota = new Vector3(0, 0, 90);
    [SerializeField] private Vector2 _obj1Pos;
    [SerializeField] private Vector2 _obj2Pos;

    [Header("Setup canvas")] 
    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private TextMeshProUGUI _text2;
    private float _timeRun = 1.25f;

    private readonly float _time = 0.4f;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Bounds bounds = _spriteRenderer.sprite.bounds;
        _width = bounds.size.x;
    }

    private void OnEnable()
    {
        GameEventManager.Test += OnMask;
    }

    private void OnDisable()
    {
        GameEventManager.Test -= OnMask;
    }

    private void OnMask(float des)
    {
        float num = des * 0.55f + _width * 0.55f;
        Mask.transform.localPosition = new Vector2(num, 0);

        float cal1 = num / (_width * 1.1f) * 100;
        int round1 = Mathf.RoundToInt(cal1);
        int round2 = 100 - round1;
        
        _otherHalf = Instantiate(this.gameObject);
        GameObject mask2 = _otherHalf.GetComponent<TestObj>().Mask;
        mask2.transform.localPosition = new Vector2(num - _width * 1.1f, 0);

        DOTween.Sequence()
            .Append(this.transform.DOMove(_obj1Pos, _time).SetEase(Ease.OutQuad))
            .Join(this.transform.DORotate(_obj1Rota, _time))
            .AppendCallback(() => 
            {
                _text1.gameObject.SetActive(true);
                TweenPercentageText(_text1, 0, round2, _timeRun);
                GameEventManager.CutInHalf?.Invoke(round1);
            });

        DOTween.Sequence()
            .Append(_otherHalf.transform.DOMove(_obj2Pos, _time).SetEase(Ease.OutQuad))
            .Join(_otherHalf.transform.DORotate(_obj2Rota, _time))
            .AppendCallback(() => 
            {
                _text2.gameObject.SetActive(true);
                TweenPercentageText(_text2, 0, round1, _timeRun);
            });
    }

    private void TweenPercentageText(TextMeshProUGUI text, int startValue, int endValue, float duration)
    {
        DOTween.To(() => startValue, x => text.text = $"{Mathf.Round(x)} %", endValue, duration)
            .SetEase(Ease.Linear);
    }
}
