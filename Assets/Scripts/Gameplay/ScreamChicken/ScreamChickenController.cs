using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamChickenController : MonoBehaviour
{
    [SerializeField] private ChickenController _chickenController;
    [SerializeField] private GameObject _choicePanel;
    [SerializeField] private int _sample = 64;
    [SerializeField] private float _threshold = 0.1f;
    private AudioClip _microphoneClip;
    private readonly WaitForSeconds _wait = new(0.3f);
    
    private void OnEnable()
    {
        _choicePanel.SetActive(true);
        if (Camera.main != null)
        {
            Camera.main.gameObject.AddComponent<CameraFollow>();
            Camera.main.gameObject.GetComponent<CameraFollow>().Obj = _chickenController.transform;
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

    public void ChooseType(int type)
    {
        AudioManager.PlaySound("Click");
        _chickenController.ChooseType(type);
        StartCoroutine(TurnPanelOff());
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
