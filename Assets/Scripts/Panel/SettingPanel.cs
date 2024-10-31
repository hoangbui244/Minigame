using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _vibrationToggle;
    
    [SerializeField] private RectTransform _music;
    [SerializeField] private RectTransform _sound;
    [SerializeField] private RectTransform _vibration;

    [SerializeField] private Image[] _Bg;
    [SerializeField] private Sprite _on;
    [SerializeField] private Sprite _off;
    
    [SerializeField] private float _time = 0.2f;
    private Vector2 _endPos = new Vector2(205, 0);

    private void Start()
    {
        Init();   
    }

    private void Init()
    {
        _musicToggle.isOn = AudioManager.IsMusicEnable;
        _soundToggle.isOn = AudioManager.IsSoundEnable;
        _vibrationToggle.isOn = AudioManager.IsVibrationEnable;
        
        _Bg[0].sprite = AudioManager.IsMusicEnable ? _on : _off;
        _Bg[1].sprite = AudioManager.IsSoundEnable ? _on : _off;
        _Bg[2].sprite = AudioManager.IsVibrationEnable ? _on : _off;
        
        _music.anchoredPosition = !AudioManager.IsMusicEnable ? Vector2.zero : _endPos;
        _sound.anchoredPosition = !AudioManager.IsSoundEnable ? Vector2.zero : _endPos;
        _vibration.anchoredPosition = !AudioManager.IsVibrationEnable ? Vector2.zero : _endPos;

        _musicToggle.onValueChanged.AddListener(MusicToggle);
        _soundToggle.onValueChanged.AddListener(SoundToggle);
        _vibrationToggle.onValueChanged.AddListener(VibrationToggle);
    }

    private void MusicToggle(bool isOn)
    {
        AudioManager.Instance.OnMusicStateChanged(isOn);
        AudioManager.IsMusicEnable = isOn;
        _music.DOAnchorPos(isOn ? _endPos : Vector2.zero, _time);
        _Bg[0].sprite = isOn ? _on : _off;
    }

    private void SoundToggle(bool isOn)
    {
        AudioManager.Instance.OnSoundStateChanged(isOn);
        AudioManager.IsSoundEnable = isOn;
        _sound.DOAnchorPos(isOn ? _endPos : Vector2.zero, _time);
        _Bg[1].sprite = isOn ? _on : _off;
    }

    private void VibrationToggle(bool isOn)
    {
        AudioManager.Instance.OnVibraStateChanged(isOn);
        AudioManager.IsVibrationEnable = isOn;
        _vibration.DOAnchorPos(isOn ? _endPos : Vector2.zero, _time);
        _Bg[2].sprite = isOn ? _on : _off;
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Rate()
    {
        
    }
}