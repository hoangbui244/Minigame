using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SpriteSlicer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jelly : MonoBehaviour
    {
        public bool maxSlicesReached = false;

        public List<(float, float)> parameters = new();
        [Range(2, 8)] [SerializeField] int maxSliceAllowed = 8;
        [SerializeField] private Vector3 _obj1;
        [SerializeField] private Vector3 _obj2;
        [SerializeField] private float _time;
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private TextMeshProUGUI _text;
        private float _originalArea;
        private int _finalValue;
        private SpriteRenderer _sr;
        private static float _num;

        void Start()
        {
            NullCheck();
            _originalArea = CalculateColliderArea(GetComponent<Collider2D>());
            _num = 0;
        }
        
        private float CalculateSlicePercentages()
        {
            float areaPart1 = CalculateColliderArea(GetComponent<Collider2D>());

            float percentagePart1 = (areaPart1 / _originalArea) * 100f;
            _num = Mathf.Round(percentagePart1);
            return _num;
        }
        
        private float CalculateColliderArea(Collider2D collider)
        {
            if (collider is PolygonCollider2D polyCollider)
            {
                float area = 0f;
                Vector2[] points = polyCollider.points;

                for (int i = 0; i < points.Length; i++)
                {
                    Vector2 point1 = points[i];
                    Vector2 point2 = points[(i + 1) % points.Length];
                    area += point1.x * point2.y - point2.x * point1.y;
                }

                return Mathf.Abs(area) / 2f;
            }

            return 0f;
        }

        void NullCheck()
        {
            if (_sr == null)
            {
                _sr = GetComponent<SpriteRenderer>();
                _sr.material = SliceManager.Instance.sliceMaterial;
            }
        }

        public void SetNewShaderParameters(float _degree, float _edge)
        {
            parameters.Add((_degree, _edge));
            maxSlicesReached = (parameters.Count >= maxSliceAllowed) ? true : false;

            UpdateShaderParameters();
        }

        void UpdateShaderParameters()
        {
            NullCheck();

            var sliceIndex = 1;
            foreach (var param in parameters)
            {
                _sr.material.SetFloat("_Degree_" + sliceIndex, param.Item1);
                _sr.material.SetFloat("_Edge_" + sliceIndex, param.Item2);

                sliceIndex++;
            }
        }

        public void InvokeEnableCollider()
        {
            Invoke(nameof(EnableCollider), 0.15f);
        }

        void EnableCollider()
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }

        public void ExertForce(Vector2 dir)
        {
            GetComponent<Rigidbody2D>().AddForce(dir * SliceManager.Instance.force);
        }

        public void MoveObj1()
        {
            transform.DOMove(_obj1, _time).OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0, 90), _time).OnComplete(() =>
                {
                    _canvas.anchoredPosition = new Vector2(3, 0);
                    _canvas.rotation = Quaternion.Euler(0, 0, 0);
                    _canvas.gameObject.SetActive(true);
                    float finalPercentage = CalculateSlicePercentages();
                    TweenPercentageText(_text, 0, finalPercentage, 1f);
                    Invoke(nameof(Check), 1.2f);
                });
            });
        }

        public void MoveObj2()
        {
            transform.DOMove(_obj2, _time).OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0, -90), _time).OnComplete(() =>
                {
                    _canvas.anchoredPosition = new Vector2(-3, 0); 
                    _canvas.rotation = Quaternion.Euler(0, 0, 0);
                    _canvas.gameObject.SetActive(true);
                    float finalPercentage = 100 - _num;
                    TweenPercentageText(_text, 0, finalPercentage, 1f);
                });
            });
        }
        
        private void TweenPercentageText(TextMeshProUGUI text, float startValue, float endValue, float duration)
        {
            DOTween.To(() => startValue, x => startValue = x, endValue, duration)
                .OnUpdate(() => 
                {
                    text.text = $"{Mathf.Round(startValue)} %";
                })
                .OnComplete(() => 
                {
                    _finalValue = Mathf.RoundToInt(endValue);
                    text.text = $"{_finalValue} %";
                });
        }

        private void Check()
        {
            GameEventManager.CutInHalf?.Invoke(_finalValue);
        }
    }
}