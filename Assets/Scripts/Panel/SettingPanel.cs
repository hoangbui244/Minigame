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
    
    [SerializeField] private float _time = 0.3f;
    private Vector2 _endPos = new Vector2(205, 0);

    private void Start()
    {
        _musicToggle.isOn = AudioManager.IsMusicEnable;
        _soundToggle.isOn = AudioManager.IsSoundEnable;
        _vibrationToggle.isOn = AudioManager.IsVibrationEnable;
        
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
    }

    private void SoundToggle(bool isOn)
    {
        AudioManager.Instance.OnSoundStateChanged(isOn);
        AudioManager.IsSoundEnable = isOn;
        _sound.DOAnchorPos(isOn ? _endPos : Vector2.zero, _time);
    }

    private void VibrationToggle(bool isOn)
    {
        AudioManager.Instance.OnVibraStateChanged(isOn);
        AudioManager.IsVibrationEnable = isOn;
        _vibration.DOAnchorPos(isOn ? _endPos : Vector2.zero, _time);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}