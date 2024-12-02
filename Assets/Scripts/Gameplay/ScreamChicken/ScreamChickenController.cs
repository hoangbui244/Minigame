using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamChickenController : MonoBehaviour
{
    [SerializeField] private ChickenController _chickenController;
    [SerializeField] private GameObject _choicePanel;
    [SerializeField] private GameObject _choosePanel;
    [SerializeField] private int _sample = 64;
    [SerializeField] private float _threshold = 0.1f;
    private AudioClip _microphoneClip;
    private readonly WaitForSeconds _wait = new(0.3f);
    
    private void OnEnable()
    {
        Init();
        if (MainUIMananger.Instance.ScreamChickenTime == 0)
        {
            MainUIMananger.Instance.ScreamChickenTime = 1;
            _choicePanel.SetActive(true);
            _choosePanel.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("Pet0") == 1 && PlayerPrefs.GetInt("Pet1") == 1 && PlayerPrefs.GetInt("Pet2") == 1)
            {
                _choicePanel.SetActive(true);
                return;
            }
            _choicePanel.SetActive(false);
            _choosePanel.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (Camera.main != null)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                Destroy(cameraFollow); 
            }
        }
    }

    private void Start()
    {
        MicrophoneToAudio();
    }

    private void Init()
    {
        _choicePanel.SetActive(false);
        _choosePanel.SetActive(false);
        if (Camera.main != null)
        {
            Camera.main.gameObject.AddComponent<CameraFollow>();
            Camera.main.gameObject.GetComponent<CameraFollow>().Obj = _chickenController.transform;
        }
    }
    
    public void NoThanks()
    {
        AudioManager.PlaySound("Click");
        _choosePanel.SetActive(false);
        _chickenController.StartGame = true;
    }

    public void ChooseType(int type)
    {
        AudioManager.PlaySound("Click");
        MainUIMananger.Instance.ScreamChickenType = type;
        _chickenController.ChooseType(type);
        StartCoroutine(TurnPanelOff());
    }
    
    public void ChoosePet()
    {
        AudioManager.PlaySound("Click");
        AdsManager.Instance.ShowRewarded(completed =>
        {
            if (completed)
            {
                PlayerPrefs.SetInt("Pet2", 1);
                MainUIMananger.Instance.ScreamChickenChar = 2;
                _chickenController.UpdateSprite(2);
                _choosePanel.SetActive(false);
                _chickenController.StartGame = true;
            }
            else
            {
                _choosePanel.SetActive(false);
                _chickenController.StartGame = true;
            }
        });
    }
    
    private IEnumerator TurnPanelOff()
    {
        yield return _wait;
        _choicePanel.SetActive(false);
        _chickenController.StartGame = true;
    }

    private void MicrophoneToAudio()
    {
        if (Microphone.devices.Length > 0)
        {
            string microphone = Microphone.devices[0];
            _microphoneClip = Microphone.Start(microphone, true, 20, 44100);
        }
        else
        {
            Debug.LogError("No microphone detected! Please connect a microphone.");
        }
    }
    
    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]),_microphoneClip);
    }
    
    private float GetLoudnessFromAudioClip(int position, AudioClip audioClip)
    {
        int startPosition = position - _sample;
        if (startPosition < 0) return 0;

        float[] waveData = new float[_sample];
        audioClip.GetData(waveData, startPosition);

        float totalLoudness = 0;
        for (int i = 0; i < _sample; i++)
        {
            waveData[i] = Mathf.Clamp(waveData[i], -1f, 1f);

            if (Mathf.Abs(waveData[i]) > _threshold)
            {
                totalLoudness += Mathf.Abs(waveData[i]);
            }
        }

        return totalLoudness / _sample;
    }
}
