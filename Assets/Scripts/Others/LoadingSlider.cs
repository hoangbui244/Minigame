using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    [SerializeField] private RectTransform _slider;
    [SerializeField] private GoogleMobileAdsConsentController _consentController;
    [SerializeField] private Vector2 _value;
    [SerializeField] private Vector2 _value1;
    [SerializeField] private float _time;
    [SerializeField] private Ease _ease;
    [SerializeField] private bool _hasConsent;
    private int _count = 0;
    
    private void Awake()
    {
        CheckConsent();
    }

    private void CheckConsent()
    {
        if (ResourceManager.FirstOpen)
        {
            if (_hasConsent)
            {
                _consentController.GatherConsent((string error) =>
                {
                    if (error != null)
                    {
                        Debug.LogError("Failed to gather consent: " + error);
                    }

                    if (_consentController.CanRequestAds)
                    {
                        LoadProgress();
                    }
                });
            }
            else
            {
                LoadProgress();
            }
        }
        else
        {
            LoadProgress();
        }
    }
    
    private void LoadProgress()
    {
        _slider.DOAnchorPosX(_value.x, _time * 0.8f)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                StartCoroutine(WaitForOpenAds());
            });
    }

    private IEnumerator WaitForOpenAds()
    {
        bool adCompleted = false;

        AdsManager.Instance.ShowOpen((completed) =>
        {
            adCompleted = completed;
        });

        float elapsedTime = 0f;
        while (!adCompleted && elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (adCompleted)
        {
            ContinueLoad();
        }
        else
        {
            ContinueLoad();
        }
    }

    private void ContinueLoad()
    {
        _slider.DOAnchorPosX(_value1.x, _time * 0.2f)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                MainUIMananger.LoadScene("HomeScreen");
                AudioManager.PlayLoopSound("MainTheme");
                AdsManager.Instance.StartCappingAds = false;
                ResourceManager.FirstOpen = false;
            });

        FirebaseManager.Instance.LogEventName("open_ads");
    }
}