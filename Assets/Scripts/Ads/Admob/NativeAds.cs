using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;

public class NativeAds : MonoBehaviour
{
    [SerializeField] private RawImage _adsImage;
    [SerializeField] private RawImage _adsContent;
    [SerializeField] private RawImage _adsChoiceIcon;
    [SerializeField] private TextMeshProUGUI _adsHeadline;
    [SerializeField] private TextMeshProUGUI _adsBody;
    [SerializeField] private TextMeshProUGUI _adsCallToAction;
    [SerializeField] private TextMeshProUGUI _starRating;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private GameObject _star;

    private void OnEnable()
    {
        if (AdsManager.Instance.LoadedNativeAd != null)
        {
            ShowAds(AdsManager.Instance.LoadedNativeAd);
        }
        else
        {
            AdsManager.Instance.OnNativeAdLoaded += HandleNativeAdLoaded;
        }
    }

    private void OnDisable()
    {
        AdsManager.Instance.OnNativeAdLoaded -= HandleNativeAdLoaded;
    }

    private void HandleNativeAdLoaded()
    {
        ShowAds(AdsManager.Instance.LoadedNativeAd);
    }

    private void ShowAds(NativeAd nativeAd)
    {
        // Check if any critical data is missing
        if (nativeAd.GetImageTextures() == null || nativeAd.GetImageTextures().Count == 0 ||
            string.IsNullOrEmpty(nativeAd.GetHeadlineText()) ||
            string.IsNullOrEmpty(nativeAd.GetBodyText()) ||
            string.IsNullOrEmpty(nativeAd.GetCallToActionText()))
        {
            Debug.LogWarning("Native ad is missing critical data, hiding ads UI.");
            gameObject.SetActive(false); // Hide the whole Native Ads UI
            AdsManager.Instance.RequestNativeAd();
            return;
        }

        // Show ads UI
        gameObject.SetActive(true);

        // Set Icon Image
        if (nativeAd.GetIconTexture() != null)
        {
            _adsImage.gameObject.SetActive(true);
            _adsImage.texture = nativeAd.GetIconTexture();
        }
        else
        {
            _adsImage.gameObject.SetActive(false);
        }

        // Set Main Image
        _adsContent.texture = nativeAd.GetImageTextures()[0];

        // Set Headline
        _adsHeadline.text = nativeAd.GetHeadlineText();

        // Set Body Text
        _adsBody.text = nativeAd.GetBodyText();

        // Set Call to Action
        _adsCallToAction.text = nativeAd.GetCallToActionText();

        // Set AdChoices Logo
        if (nativeAd.GetAdChoicesLogoTexture() != null)
        {
            _adsChoiceIcon.texture = nativeAd.GetAdChoicesLogoTexture();
        }

        // Set Star Rating
        if (nativeAd.GetStarRating() > 0)
        {
            _starRating.gameObject.SetActive(true);
            _star.SetActive(true);
            _starRating.text = nativeAd.GetStarRating().ToString();
        }
        else
        {
            _star.SetActive(false);
            _starRating.gameObject.SetActive(false);
        }

        // Set Price
        if (nativeAd.GetPrice() == null)
        {
            _price.gameObject.SetActive(false);
        }
        else
        {
            _price.gameObject.SetActive(true);
            _price.text = nativeAd.GetPrice();
        }

        // Register Ad components for click tracking
        nativeAd.RegisterHeadlineTextGameObject(_adsHeadline.gameObject);
        nativeAd.RegisterBodyTextGameObject(_adsBody.gameObject);
        nativeAd.RegisterCallToActionGameObject(_adsCallToAction.gameObject);
        nativeAd.RegisterIconImageGameObject(_adsImage.gameObject);
        nativeAd.RegisterPriceGameObject(_price.gameObject);

        // Request next ad
        AdsManager.Instance.RequestNativeAd();
    }
}