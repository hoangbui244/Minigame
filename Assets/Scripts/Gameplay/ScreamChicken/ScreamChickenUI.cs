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
    private int _petIndex;

    private void OnEnable()
    {
        Setup();
    }
    
    private void Setup()
    {
        _petIndex = 0;
        if (!PlayerPrefs.HasKey("Pet0"))
        {
            PlayerPrefs.SetInt("Pet0", 1);
        }
        _pet.sprite = _petSprites[_petIndex];
        _button.image.sprite = _buttons[0];
    }

    public void ChoosePet()
    {
        _button.image.sprite = _buttons[1];
        _chickenController.UpdateSprite(_petIndex);
    }
    
    private void UpdateButton(int value)
    {
        var name = "Pet" + value;
        _button.image.sprite = !PlayerPrefs.HasKey(name) ? _buttons[2] : _buttons[0];
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
