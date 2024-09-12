using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("---- Audio Clips ----\n")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    
    [Header("---- Audio Sources ----\n")]
    public AudioSource sfxSource;
    public MusicSource[] musicSourceList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (MusicSource i in musicSourceList)
            {
                i.songName = MusicSource.defaultName;

                foreach (MusicSource j in musicSourceList)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (i.musicSource == j.musicSource)
                    {
                        Debug.LogWarning("Error, music source duplicate found!");
                    }
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        MusicSource source = Array.Find(musicSourceList, y => y.playing == false);

        if (sound == null)
        {
            Debug.LogWarning("Error, music sound " + sound + " not found!");
        }
        else if (source == null)
        {
            Debug.LogWarning("Error, no music source available!");
        }
        else
        {
            source.playing = true;
            source.songName = sound.name;
            source.musicSource.clip = sound.clip;
            source.musicSource.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        MusicSource source = Array.Find(musicSourceList, y => y.songName == name);

        if (sound == null)
        {
            Debug.LogWarning("Error, music sound " + sound + " not found!");
        }
        else if (source == null)
        {
            Debug.LogWarning("Error, no music source found playing " + sound + "!");
        }
        else
        {
            source.musicSource.Stop();
            source.songName = MusicSource.defaultName;
            source.playing = false;
        }
    }

    public void StopAllMusic()
    {
        bool playingCheck = false;

        foreach (MusicSource source in musicSourceList)
        {
            playingCheck = playingCheck | source.playing;
            
            source.musicSource.Stop();
            source.songName = MusicSource.defaultName;
            source.playing = false;
        }

        if (playingCheck)
        {
            Debug.LogWarning("Error, no music was playing!");
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.LogWarning("Error, sfx sound " + sound + " not found!");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }
    
    public void SFXToggleMute()
    {
        sfxSource.mute = !sfxSource.mute;
    }

}
