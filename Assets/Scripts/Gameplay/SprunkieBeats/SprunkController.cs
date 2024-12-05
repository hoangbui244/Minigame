using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprunkController : Singleton<SprunkController>
{
    [Header("======= Character =======")] [SerializeField]
    private Transform _parent;

    [SerializeField] private List<CharacterController> _characters;

    [Header("======= Beat =======")] [SerializeField]
    private Transform _beatParent;

    [SerializeField] private List<BeatController> _beats;

    private void OnEnable()
    {
        GameEventManager.MuteOther += MuteOther;
        GameEventManager.UnselectCharacter += ResetBeat;
        GameEventManager.Play += Play;
    }

    private void OnDisable()
    {
        GameEventManager.MuteOther -= MuteOther;
        GameEventManager.UnselectCharacter -= ResetBeat;
        GameEventManager.Play -= Play;
    }

    private void OnValidate()
    {
        _characters.Clear();
        _beats.Clear();
        for (int i = 0; i < _parent.childCount; i++)
        {
            var character = _parent.GetChild(i).GetComponent<CharacterController>();
            if (character != null)
            {
                _characters.Add(character);
            }
        }

        for (int i = 0; i < _beatParent.childCount; i++)
        {
            Transform child = _beatParent.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                var beat = child.GetChild(j).GetComponent<BeatController>();
                if (beat != null)
                {
                    _beats.Add(beat);
                }
            }
        }
    }

    private void ResetBeat(int id)
    {
        foreach (var item in _beats)
        {
            if ((int)item.Type == id)
            {
                item.ResetBeat();
                SprunkSoundController.Instance.StopSound(id);
            }
        }
    }

    public void ResetGame()
    {
        foreach (var item in _beats)
        {
            item.ResetBeat();
        }

        foreach (var item in _characters)
        {
            item.ResetCharacter();
        }
    }

    private void MuteOther(bool state, int id)
    {
        foreach (var item in _characters)
        {
            if ((int)item.Type != id && item.Type != CharacterController.CharType.Default)
            {
                if (state)
                {
                    item.IsMuted = true;
                    item.IsMutedOther = false;
                    item.SetSprite(1);
                }
                else
                {
                    item.IsMuted = false;
                    item.IsMutedOther = false;
                    item.SetSprite(2);
                }
            }
        }
    }

    public void Play()
    {
        foreach (var item in _characters)
        {
            if (item.Type != CharacterController.CharType.Default && !item.IsMuted)
            {
                SprunkSoundController.Instance.PlayLoopSound((int)item.Type);
            }
        }
    }
}