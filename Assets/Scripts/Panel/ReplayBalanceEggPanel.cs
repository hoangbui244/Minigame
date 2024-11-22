using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplayBalanceEggPanel : MonoBehaviour
{
    [SerializeField] private Sprite _win;
    [SerializeField] private Sprite _lose;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _textWin;
    [SerializeField] private Sprite _textLose;
    [SerializeField] private Image _textImage;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _nativeAds;
    
    private void OnEnable()
    {
        MainUIMananger.Instance.PopupOpened = true;
        if (ResourceManager.RemoveAds || !AdsManager.Instance.VersionTrue)
        {
            _nativeAds.SetActive(false);
        }
    }
    
    private void OnDisable()
    {
        MainUIMananger.Instance.PopupOpened = false;
    }

    public void Win()
    {
        _image.sprite = _win;
        _textImage.sprite = _textWin;
        _text.color = _winColor;
        _text.text = "HIGH SCORE: " + ResourceManager.BalanceEggHighScore.ToString() + "s";
    }
    
    public void Lose(int score)
    {
        _image.sprite = _lose;
        _textImage.sprite = _textLose;
        _text.color = _loseColor;
        _text.text = "YOUR SCORE: " + score.ToString() + "s";
    }
}
