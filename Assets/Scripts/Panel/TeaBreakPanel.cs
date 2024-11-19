using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeaBreakPanel : MonoBehaviour
{
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private int _totalRounds = 3;
    [SerializeField] private TextMeshProUGUI _time;
    private float _roundDuration = 1f;
    private float _currentRoundTime;
    private int _currentRound;

    private void OnEnable()
    {
        _currentRoundTime = _roundDuration;
        _timeSlider.maxValue = _roundDuration;
        _timeSlider.value = _currentRoundTime;
        _currentRound = _totalRounds;
        _time.text = _currentRound.ToString();
    }

    void Update()
    {
        if (_currentRound > 0)
        {
            if (_currentRoundTime > 0)
            {
                _currentRoundTime -= Time.deltaTime;
                _timeSlider.value = _currentRoundTime;
            }
            else
            {
                _currentRound--;
                _time.text = _currentRound.ToString();
                _currentRoundTime = _roundDuration;
                _timeSlider.value = _currentRoundTime;
                if (_currentRound <= 0)
                {
                    if (AdsManager.Instance.CanShowBreak)
                    {
                        AdsManager.Instance.ShowAdBreak((completed) =>
                        {
                            if (completed)
                            {
                                FirebaseManager.Instance.LogEventName("show_inters");
                            }

                            gameObject.SetActive(false);
                            GameUIManager.Instance.ReloadAfterTeaBreak();
                        });
                    }
                }
            }
        }
    }
}