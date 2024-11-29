using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreamChickenUI : MonoBehaviour
{
    [SerializeField] private ChickenController _chickenController;
    [SerializeField] private List<Sprite> _petSprites;
    [SerializeField] private Button _button;
    [SerializeField] private List<Sprite> _buttons;
    [SerializeField] private Image _pet;
    [SerializeField] private Image _pet1;
    [SerializeField] private Image _pet2;
    private int _petIndex;
    private int _buttonIndex;

    private void OnEnable()
    {
        Setup();
    }
    
    private void Setup()
    {
        _buttonIndex = MainUIMananger.Instance.ScreamChickenChar;
        _petIndex = 0;
        if (MainUIMananger.Instance.ScreamChickenTime == 1)
        {
            _pet1.sprite = _petSprites[_buttonIndex];
            _pet2.sprite = _petSprites[_buttonIndex];
        }

        if (!PlayerPrefs.HasKey("Pet0") && !PlayerPrefs.HasKey("Pet1"))
        {
            PlayerPrefs.SetInt("Pet0", 1);
            PlayerPrefs.SetInt("Pet1", 1);
        }
        _pet.sprite = _petSprites[_petIndex];
        _button.image.sprite = _buttons[0];
    }

    public void ChoosePet()
    {
        var name = "Pet" + _petIndex;
        if (!PlayerPrefs.HasKey(name))
        {
            AdsManager.Instance.ShowRewarded(completed =>
            {
                if (completed)
                {
                    PlayerPrefs.SetInt(name, 1);
                    _button.image.sprite = _buttons[1];
                    _buttonIndex = _petIndex;
                    _chickenController.UpdateSprite(_petIndex);
                }
                else
                {
                    _button.image.sprite = _buttons[2];
                }
            });
        }
        else
        {
            _button.image.sprite = _buttons[1];
            MainUIMananger.Instance.ScreamChickenChar = _petIndex;
            _buttonIndex = _petIndex;
            _chickenController.UpdateSprite(_petIndex);
        }
    }
    
    private void UpdateButton(int value)
    {
        var name = "Pet" + value;
        _button.image.sprite = !PlayerPrefs.HasKey(name) ? _buttons[2] : _buttons[0];
        if (_buttonIndex == value)
        {
            _button.image.sprite = _buttons[1];
        }
    }

    public void Next()
    {
        _petIndex = (_petIndex + 1) % _petSprites.Count;
        UpdateButton(_petIndex);
        UpdatePet();
    }
    
    public void Previous()
    {
        _petIndex = (_petIndex - 1 + _petSprites.Count) % _petSprites.Count;
        UpdateButton(_petIndex);
        UpdatePet();
    }
    
    private void UpdatePet()
    {
        _pet.sprite = _petSprites[_petIndex];
    }
}
