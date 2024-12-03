using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public enum CharType
    {
        Default,
        Char1 = 1,
        Char2 = 2,
        Char3 = 3,
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
    private bool _isMuted;
    private bool _isMutedOther;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
        // _muteImage.sprite = _sprites[1];
        // _muteOtherImage.sprite = _sprites[3];
    }

    public void SetHover(bool isHovering)
    {
        _image.color = isHovering ? _hoverColor : _defaultColor;
    }
    
    public void CharacterSelected(bool isOn)
    {
        GameEventManager.UnselectCharacter?.Invoke((int)Type);
        _board.SetActive(isOn);
    }

    public void Mute()
    {
        _isMuted = !_isMuted;
        if (_isMuted)
        {
            // 0 là muted
            // _muteImage.sprite = _sprites[0];
            var color = _image.color;
            color.a = 0.5f;
            _image.color = color;
            SprunkSoundController.Instance.StopSound((int)Type);
        }
        else
        {
            // 1 là unmuted
            // _muteImage.sprite = _sprites[1];
            var color = _image.color;
            color.a = 1f;
            _image.color = color;
            SprunkSoundController.Instance.PlayLoopSound((int)Type);
        }
    }
    
    public void MuteOther()
    {
        _isMutedOther = !_isMutedOther;
        // 2 là muted other và 3 là unmuted other
        // _muteOtherImage.sprite = _isMutedOther ? _sprites[2] : _sprites[3];
        GameEventManager.MuteOther?.Invoke((int)Type);
    }
}