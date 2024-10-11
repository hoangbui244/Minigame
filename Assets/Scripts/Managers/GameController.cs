using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Knife _knife;
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                {
                    _knife.Chop();
                    _scoreText.text = (int.Parse(_scoreText.text) + 1).ToString();
                }
            }
        }
    }
}
