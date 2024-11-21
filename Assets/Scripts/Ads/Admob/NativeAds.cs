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
    
    private void ShowAds(NativeAd nativeAd) {
        if (nativeAd.GetIconTexture() != null)
        {
            _adsImage.gameObject.SetActive(true);
            _adsImage.texture = nativeAd.GetIconTexture();
        }
        else
        {
            _adsImage.gameObject.SetActive(false);
        }
        _adsContent.texture = nativeAd.GetImageTextures()[0];
        _adsHeadline.text = nativeAd.GetHeadlineText();
        _adsBody.text = nativeAd.GetBodyText();
        _adsCallToAction.text = nativeAd.GetCallToActionText();
        
        if (nativeAd.GetAdChoicesLogoTexture() != null)
        {
            _adsChoiceIcon.texture = nativeAd.GetAdChoicesLogoTexture();
        }
        
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
        
        if (nativeAd.GetPrice() == null)
        {
            _price.gameObject.SetActive(false);
        }
        else
        {
            _price.gameObject.SetActive(true);
            _price.text = nativeAd.GetPrice();
        }
        
        if (!nativeAd.RegisterHeadlineTextGameObject(_adsHeadline.gameObject))
        {
            Debug.Log("error registering headline");
        }
        if (!nativeAd.RegisterBodyTextGameObject(_adsBody.gameObject))
        {
            Debug.Log("error registering body");
        }
        if (!nativeAd.RegisterCallToActionGameObject(_adsCallToAction.gameObject))
        {
            Debug.Log("error registering cta");
        }
        if (!nativeAd.RegisterIconImageGameObject(_adsImage.gameObject))
        {
            Debug.Log("error registering image");
        }
        if (!nativeAd.RegisterPriceGameObject(_price.gameObject))
        {
            Debug.Log("error registering price");
        }
        
        AdsManager.Instance.RequestNativeAd();
    }
}
