using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;
    private AudioSource _audioSource;

    void Start()
    {
        
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string soundName)
    {
        switch (soundName)
        {
            case "mainMenuMusic":
                _audioSource.clip = mainMenuMusic;
                _audioSource.Play();
                break;
            case "inGameMusic":
                _audioSource.clip = inGameMusic;
                _audioSource.Play();
                break;
            default:
                _audioSource.clip = null;
                break;
        }
        

        
    }
}
