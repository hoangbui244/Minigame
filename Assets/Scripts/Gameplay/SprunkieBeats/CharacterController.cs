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
    [SerializeField] private Image _bodyImage;
    [SerializeField] private Image _eyeImage;
    [SerializeField] private Image _mouthImage;
    private Color _defaultColor;

    [Header("======== Other ========")] 
    [SerializeField] private GameObject _board;
    [SerializeField] private Image _muteImage;
    [SerializeField] private Image _muteOtherImage;
    [SerializeField] private List<Sprite> _sprites;
    public bool IsMuted;
    public bool IsMutedOther;
    private Animator _animator;
    private readonly WaitForSeconds _wait = new WaitForSeconds(8f);
    
    private void Start()
    {
        _defaultColor = _bodyImage.color;
        _muteImage.sprite = _sprites[1];
        _muteOtherImage.sprite = _sprites[3];
        _animator = GetComponent<Animator>();
        StartCoroutine(PlayAnim());
    }
    
    private IEnumerator PlayAnim()
    {
        while (true)
        {
            yield return _wait;
            UpdateAnim();
        }
    }

    public void SetHover(bool isHovering)
    {
        if (!IsMuted)
        {
            _bodyImage.color = isHovering ? _hoverColor : _defaultColor;
            _eyeImage.color = isHovering ? _hoverColor : _defaultColor;
        }
        else
        {
            if (isHovering)
            {
                _bodyImage.color = _hoverColor;
                _eyeImage.color = _hoverColor;
            }
            else
            {
                SetAlpha(0.5f);
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
        UpdateAnim();
    }

    public void Mute()
    {
        IsMuted = !IsMuted;
        // 0 là muted và 1 là unmuted
        _muteImage.sprite = _sprites[IsMuted ? 0 : 1];
        SetAlpha(IsMuted ? 0.5f : 1f);

        if (IsMutedOther && IsMuted)
        {
            IsMutedOther = false;
            SetSprite(3);
        }

        if (IsMuted) SprunkSoundController.Instance.StopSound((int)Type);
        else SprunkSoundController.Instance.PlayLoopSound((int)Type);

        GameEventManager.CheckMuteOther?.Invoke(IsMuted, (int)Type);
    }
    
    public void MuteOther()
    {
        IsMutedOther = !IsMutedOther;
        // 2 là muted other và 3 là unmuted other
        _muteOtherImage.sprite = _sprites[IsMutedOther ? 2 : 3];
        GameEventManager.MuteOther?.Invoke(IsMutedOther,(int)Type);
        if (IsMutedOther)
        {
            IsMuted = false;
            _muteImage.sprite = _sprites[1];
            SetAlpha(1f);
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
                SetAlpha(0.5f);
                _muteOtherImage.sprite = _sprites[3];
                break;
            case 2:
                _muteImage.sprite = _sprites[1];
                SetAlpha(1f);
                break;
            case 3:
                _muteOtherImage.sprite = _sprites[3];
                break;
            case 4:
                _muteOtherImage.sprite = _sprites[2];
                break;
        }
    }
    
    private void SetAlpha(float alpha)
    {
        var color = _bodyImage.color;
        color.a = alpha;
        _bodyImage.color = color;
        _eyeImage.color = color;
        _mouthImage.color = color;
    }
    
    public void ResetCharacter()
    {
        _bodyImage.color = _defaultColor;
        IsMuted = false;
        IsMutedOther = false;
        _muteImage.sprite = _sprites[1];
        _muteOtherImage.sprite = _sprites[3];
        SetAlpha(1f);
        SprunkSoundController.Instance.StopSound((int)Type);
        Type = CharType.Default;
        _board.SetActive(false);
        UpdateAnim();
    }
    
    private void UpdateAnim()
    {
        _animator.Play("Char" + (int)Type, 0, 0);
    }
}