using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SprunkController : Singleton<SprunkController>
{
    [Header("======= Character =======")] 
    [SerializeField] private Transform _parent;
    [SerializeField] private List<CharacterController> _characters;

    [Header("======= Beat =======")] 
    [SerializeField] private Transform _beatParent;
    [SerializeField] private List<BeatController> _beats;

    private void OnEnable()
    {
        GameEventManager.MuteOther += MuteOther;
        GameEventManager.UnselectCharacter += ResetBeat;
        GameEventManager.CheckMuteOther += CheckMuteOther;
        GameEventManager.Play += Play;
    }

    private void OnDisable()
    {
        GameEventManager.MuteOther -= MuteOther;
        GameEventManager.UnselectCharacter -= ResetBeat;
        GameEventManager.CheckMuteOther -= CheckMuteOther;
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
        foreach (var item in _beats) item.ResetBeat();
        foreach (var item in _characters) item.ResetCharacter();
    }

    private void MuteOther(bool state, int id)
    {
        foreach (var item in _characters)
        {
            if ((int)item.Type != id && item.Type != CharacterController.CharType.Default)
            {
                item.IsMuted = state;
                item.IsMutedOther = false;
                item.SetSprite(state ? 1 : 2);
            }
        }
    }

    private void CheckMuteOther(bool state, int id)
    {
        foreach (var item in _characters)
        {
            if ((int)item.Type == id && item.Type != CharacterController.CharType.Default)
            {
                if (!state && CheckMute() == 1)
                {
                    item.IsMutedOther = true;
                    item.SetSprite(4);
                    UpdateOtherCharacters(id, 3);
                }
                else if (state && CheckMute() == 1)
                {
                    UpdateAllUnmutedCharacters(4);
                }
                else
                {
                    item.IsMutedOther = false;
                    item.SetSprite(3);
                    UpdateOtherCharacters(id, 3);
                }
            }
        }
    }

    private int CheckMute()
    {
        return _characters.Count(c => c.Type != CharacterController.CharType.Default && !c.IsMuted);
    }

    private void UpdateOtherCharacters(int id, int spriteId)
    {
        foreach (var character in _characters)
        {
            if ((int)character.Type != id && character.Type != CharacterController.CharType.Default)
            {
                character.IsMutedOther = false;
                character.SetSprite(spriteId);
            }
        }
    }

    private void UpdateAllUnmutedCharacters(int spriteId)
    {
        foreach (var character in _characters)
        {
            if (!character.IsMuted && character.Type != CharacterController.CharType.Default)
            {
                character.IsMutedOther = true;
                character.SetSprite(spriteId);
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