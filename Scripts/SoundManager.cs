using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public AudioSource gameMusic;
    public AudioSource uiMusic;
    public AudioSource successMusic;
    public AudioSource gameOverMusic;
    public AudioSource badBeep;
    public AudioSource goodBeep;
    public AudioSource gameOverBeep;
    public AudioSource warpEffect;
    public AudioSource[] bounceEffects;

    private AudioSource _currentAudio;
    private float _speed;
    private bool _volumeDown;
    private bool _volumeUp;

    private void Awake()
    {
        _speed = 2f;
    }

    public void _playWarp()
    {
        warpEffect.Play();
    }

    private void FixedUpdate()
    {
        if (_volumeDown && _currentAudio.volume > 0)
        {
            _currentAudio.volume -= Time.deltaTime * _speed;
        }
        else
        {
            _volumeDown = false;
        }

        if(_volumeUp && _currentAudio.volume < 1f)
        {
            _currentAudio.volume += Time.deltaTime * _speed;
        }
        else
        {
            _volumeUp = false;
        }
    }

    public void VolumeDown(AudioSource audio)
    {
        _currentAudio = audio;
        _volumeDown = true;
    }

    public void VolumeUp(AudioSource audio)
    {
        _currentAudio = audio;
        audio.volume = 0;
        _volumeUp = true;
    }
}
