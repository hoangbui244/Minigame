using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MyPurchaseID
{
    public const string BecomeVip = "com.ocdgames.satisfeel.vip";
    public const string Popular = "com.ocdgames.satisfeel.popular";
    public const string BestChoice = "com.ocdgames.satisfeel.bestchoice";
    public const string RemoveAds = "com.ocdgames.satisfeel.removeads";
    public const string Ticket200 = "com.ocdgames.satisfeel.200tickets";
    public const string Ticket400 = "com.ocdgames.satisfeel.400tickets";
    public const string Ticket800 = "com.ocdgames.satisfeel.800tickets";
    public const string Ticket1000 = "com.ocdgames.satisfeel.1000tickets";
}

public class IAPProduct : MonoBehaviour
{
    [SerializeField] private string _purchaseID;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _discount;
    [SerializeField] private Sprite _icon;

    public string PurchaseID => _purchaseID;

    public delegate void PurchaseEvent(Product Model, Action OnComplete);

    public event PurchaseEvent OnPurchase;
    private Product _model;

    private void Start()
    {
        RegisterPurchase();
        RegisterEventButton();
    }

    protected virtual void RegisterPurchase()
    {
        StartCoroutine(IAPManager.Instance.CreateHandleProduct(this));
    }

    public void Setup(Product product, string code, string price)
    {
        _model = product;
        if (_price != null)
        {
            _price.text = price + " " + code;
        }

        if (_discount != null)
        {
            if (code.Equals("VND"))
            {
                var round = Mathf.Round(float.Parse(price) + float.Parse(price) * .4f);
                _discount.text = code + " " + round;
            }
            else
            {
                var priceFormat = $"{float.Parse(price) + float.Parse(price) * .4f:0.00}";
                _discount.text = code + " " + priceFormat;
            }
        }
    }

    private void RegisterEventButton()
    {
        _purchaseButton.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Click");
            Purchase();
        });
    }

    private void Purchase()
    {
        OnPurchase?.Invoke(_model, HandlePurchaseComplete);
    }

    private void HandlePurchaseComplete()
    {
        switch (_purchaseID)
        {
            case MyPurchaseID.BecomeVip:
                BecomeVipPack();
                FirebaseManager.Instance.LogEventName("BecomeVipPack");
                break;
            case MyPurchaseID.RemoveAds:
                RemoveAdsPack();
                FirebaseManager.Instance.LogEventName("RemoveAdsPack");
                break;
            case MyPurchaseID.Popular:
                PopularPack();
                FirebaseManager.Instance.LogEventName("PopularPack");
                break;
            case MyPurchaseID.BestChoice:
                BestChoicePack();
                FirebaseManager.Instance.LogEventName("BestChoicePack");
                break;
            case MyPurchaseID.Ticket200:
                RewardTickets(200);
                FirebaseManager.Instance.LogEventName("Pack200");
                break;
            case MyPurchaseID.Ticket400:
                RewardTickets(400);
                FirebaseManager.Instance.LogEventName("Pack400");
                break;
            case MyPurchaseID.Ticket800:
                RewardTickets(800);
                FirebaseManager.Instance.LogEventName("Pack800");
                break;
            case MyPurchaseID.Ticket1000:
                RewardTickets(1000);
                FirebaseManager.Instance.LogEventName("Pack1000");
                break;
        }

        if (_icon != null)
        {
            _purchaseButton.gameObject.GetComponent<Image>().sprite = _icon;
            _purchaseButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }

        AudioManager.PlaySound("Reward");
    }

    private void BecomeVipPack()
    {
        RewardTickets(250);
        if (ResourceManager.RemoveAds) return;
        ResourceManager.RemoveAds = true;
    }

    private void RemoveAdsPack()
    {
        ResourceManager.RemoveAds = true;
        // if (SceneManager.GetActiveScene().name == "GamePlay")
        // {
        //     GameUIManager.Instance.RemoveAdsPopup.Close();
        // }
        //
        // if (SceneManager.GetActiveScene().name == "HomeScreen")
        // {
        //     HomeUIManager.Instance.RemoveAdsPopup.Close();
        // }
    }

    private void PopularPack()
    {
        RewardTickets(150);
        if (ResourceManager.RemoveAds) return;
        ResourceManager.RemoveAds = true;
    }

    private void BestChoicePack()
    {
        RewardTickets(250);
        if (ResourceManager.RemoveAds) return;
        ResourceManager.RemoveAds = true;
    }

    private void RewardTickets(int value)
    {
        GameEventManager.UpdateTicket?.Invoke();
    }
}