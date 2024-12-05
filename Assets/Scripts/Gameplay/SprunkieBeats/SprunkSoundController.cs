using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprunkSoundController : Singleton<SprunkSoundController>
{
    [SerializeField] private SprunkSound[] _sprunkSounds;
    private Dictionary<int, SprunkSound> _sprunkSoundDict = new Dictionary<int, SprunkSound>();

    private void Start()
    {
        foreach (var s in _sprunkSounds)
        {
            s.AudioSource = gameObject.AddComponent<AudioSource>();
            s.AudioSource.clip = s.AudioClip;
            s.AudioSource.volume = s.Volume;
            s.AudioSource.loop = s.Loop;

            _sprunkSoundDict.TryAdd(s.ID, s);
        }
    }

    public void PlaySound(int id)
    {
        if (_sprunkSoundDict.TryGetValue(id, out var sound))
        {
            sound.AudioSource.PlayOneShot(sound.AudioClip);
        }
    }

    public void StopSound(int id)
    {
        if (_sprunkSoundDict.TryGetValue(id, out var sound))
        {
            sound.AudioSource.Stop();
        }
    }

    public void PlayLoopSound(int id)
    {
        if (_sprunkSoundDict.TryGetValue(id, out var sound))
        {
            sound.AudioSource.Play();
        }
    }

    public void StopAllSounds()
    {
        foreach (var sound in _sprunkSoundDict.Values)
        {
            sound.AudioSource.Stop();
        }
    }
    
    public void StopAllAnotherSounds(int id)
    {
        foreach (var sound in _sprunkSoundDict.Values)
        {
            if (sound.ID != id)
            {
                sound.AudioSource.Stop();
            }
        }
        PlayLoopSound(id);
    }
}

[Serializable]
public class SprunkSound
{
    public int ID;

    public AudioClip AudioClip;
    [Range(0f, 1f)] public float Volume;

    public bool Loop;

    public AudioSource AudioSource;
}
