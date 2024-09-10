using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource sfxSource;
    public AudioSource[] musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Error, music sound not found!");
        }
        else
        {
            //musicSource.clip = s.clip;
            //musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Error, sfx sound not found!");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void MusicToggleMute()
    {
        //musicSource.mute = !musicSource.mute;
    }
    
    public void SFXToggleMute()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        //mus
    }
}
