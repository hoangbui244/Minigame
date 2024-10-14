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
    [SerializeField] private float _time;
    [SerializeField] private Ease _ease;
    [SerializeField] private bool _hasConsent;
    private int count = 0;
    
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
    
    protected virtual void LoadProgress()
    {
        _slider.DOAnchorPosX(_value.x, _time)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                AdsManager.Instance.ShowOpen((completed) =>
                {
                    if (!completed && count < 1)
                    {
                        StartCoroutine(CallOpen());
                    }
                    else if (completed)
                    {
                        MainUIMananger.LoadScene("HomeScreen");
                        AudioManager.PlayLoopSound("MainTheme");
                        AdsManager.Instance.CanShowInters = false;
                        ResourceManager.FirstOpen = false;
                    }
                });

                FirebaseManager.Instance.LogEventName("open_app");
            });
    }

    private IEnumerator CallOpen()
    {
        yield return new WaitForSeconds(2.5f);
        count++;
        AdsManager.Instance.ShowOpen((completed) =>
        {
            MainUIMananger.LoadScene("HomeScreen");
            AudioManager.PlayLoopSound("MainTheme");
            AdsManager.Instance.CanShowInters = false;
            ResourceManager.FirstOpen = false;
        });
    }
}