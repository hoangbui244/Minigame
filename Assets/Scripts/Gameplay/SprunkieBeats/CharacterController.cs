using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public enum CharType
    {
        Default,
        Char1 = 1,
        Char2 = 2,
        Char3 = 3,
        Char4 = 4,
        Char5 = 5,
        Char6 = 6,
        Char7 = 7,
        Char8 = 8,
        Char9 = 9,
        Char10 = 10,
        Char11 = 11,
        Char12 = 12,
        Char13 = 13,
        Char14 = 14,
        Char15 = 15,
        Char16 = 16,
        Char17 = 17,
        Char18 = 18,
        Char19 = 19,
        Char20 = 20,
    }
    [Header("======== Char Type ========")]
    public CharType Type;
    
    [Header("======== Char Color ========")]
    [SerializeField] private Color _hoverColor;
    private Color _defaultColor;
    private Image _image;

    [Header("======== Other ========")] 
    [SerializeField] private GameObject _board;
    [SerializeField] private Image _muteImage;
    [SerializeField] private Image _muteOtherImage;
    [SerializeField] private List<Sprite> _sprites;
    public bool IsMuted;
    public bool IsMutedOther;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
        _muteImage.sprite = _sprites[1];
        _muteOtherImage.sprite = _sprites[3];
    }

    public void SetHover(bool isHovering)
    {
        if (!IsMuted)
        {
            _image.color = isHovering ? _hoverColor : _defaultColor;
        }
        else
        {
            if (isHovering)
            {
                _image.color = _hoverColor;
            }
            else
            {
                var color = _image.color;
                color.a = 0.5f;
                _image.color = color;
            }
        }
    }
    
    public void CharacterSelected(bool isOn)
    {
        if (!isOn)
        {
            GameEventManager.UnselectCharacter?.Invoke((int)Type);
            ResetCharacter();
        }
        _board.SetActive(isOn);
    }

    public void Mute()
    {
        IsMuted = !IsMuted;
        if (IsMuted)
        {
            // 0 là muted
            _muteImage.sprite = _sprites[0];
            var color = _image.color;
            color.a = 0.5f;
            _image.color = color;
            SprunkSoundController.Instance.StopSound((int)Type);
        }
        else
        {
            // 1 là unmuted
            _muteImage.sprite = _sprites[1];
            var color = _image.color;
            color.a = 1f;
            _image.color = color;
            SprunkSoundController.Instance.PlayLoopSound((int)Type);
        }
    }
    
    public void MuteOther()
    {
        IsMutedOther = !IsMutedOther;
        // 2 là muted other và 3 là unmuted other
        _muteOtherImage.sprite = IsMutedOther ? _sprites[2] : _sprites[3];
        GameEventManager.MuteOther?.Invoke(IsMutedOther,(int)Type);
        if (IsMutedOther)
        {
            IsMuted = false;
            _muteImage.sprite = _sprites[1];
            SprunkSoundController.Instance.StopAllAnotherSounds((int)Type);
        }
        else
        {
            SprunkController.Instance.Play();
        }
    }

    public void SetSprite(int id)
    {
        switch (id)
        {
            case 1:
                _muteImage.sprite = _sprites[0];
                _muteOtherImage.sprite = _sprites[3];
                break;
            case 2:
                _muteImage.sprite = _sprites[1];
                break;
        }
    }
    
    public void ResetCharacter()
    {
        _image.color = _defaultColor;
        var color = _image.color;
        color.a = 1f;
        _image.color = color;
        IsMuted = false;
        IsMutedOther = false;
        _muteImage.sprite = _sprites[1];
        _muteOtherImage.sprite = _sprites[3];
        SprunkSoundController.Instance.StopSound((int)Type);
        Type = CharType.Default;
        _board.SetActive(false);
    }
}