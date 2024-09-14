using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    //          ++++++ Description at the bottom! ++++++

    public static AudioManager instance;

    [Header("---- Audio Clips ----\n")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    
    [Header("---- Audio Sources ----\n")]
    public AudioSource sfxSource;
    public MusicSource[] musicSourceList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (GetComponent<MixerFXManager>() == null)
            {
                Debug.LogWarning("MixerFXManager component is missing!");
                Debug.LogWarning("Please add component before running!");
            }

            // From here, sets up audio sources.

            sfxSource.loop = false;

            foreach (MusicSource i in musicSourceList)
            {
                i.songName = MusicSource.defaultName;
                i.musicSource.loop = true;

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
        MusicSource source = Array.Find(musicSourceList, y => y.songSelected == false);

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
            source.musicSource.volume = sound.volume;
            source.musicSource.pitch = sound.pitch;
            source.musicSource.panStereo = sound.panning;

            source.songSelected = true;
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
            source.songSelected = false;
        }
    }

    public void StopAllMusic()
    {
        bool playingCheck = false;

        foreach (MusicSource source in musicSourceList)
        {
            playingCheck = playingCheck | source.songSelected;
            
            source.musicSource.Stop();
            source.songName = MusicSource.defaultName;
            source.songSelected = false;
        }

        if (playingCheck)
        {
            Debug.LogWarning("Error, no music was playing!");
        }
    }

    public void PauseAllMusic()
    {
        bool playingCheck = false;

        foreach (MusicSource source in musicSourceList)
        {
            playingCheck = playingCheck | source.songSelected;

            source.musicSource.Pause();
        }

        if (playingCheck)
        {
            Debug.LogWarning("Error, no music to pause!");
        }
    }

    public void PlayAllMusic()
    {
        bool playingCheck = false;

        foreach (MusicSource source in musicSourceList)
        {
            playingCheck = playingCheck | source.songSelected;

            source.musicSource.UnPause();
        }

        if (playingCheck)
        {
            Debug.LogWarning("Error, no music to resume!");
        }
    }

    public void PlaySFX(string name, bool isOneShot)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.LogWarning("Error, sfx sound " + sound + " not found!");
        }
        else
        {
            if (isOneShot)
            {
                sfxSource.PlayOneShot(sound.clip);
            }
            else
            {
                sfxSource.clip = sound.clip;
                sfxSource.Play();
            }
        }
    }
    
    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void SFXToggleMute()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    //Functions:
    //    - PlayMusic(string name)                  Plays sound with 'name' on loop
    //    - StopMusic(string name)                  Stops sound with 'name'
    //    - StopAllMusic()                          Stops all looping tracks
    //    - PauseAllMusic()                         Pauses all looping tracks
    //    - PlayAllMusic()                          Resumes all looping tracks
    //
    //    - PlaySFX(string name, bool isOneShot)    Plays sound with 'name' once (with a 'OneShot' option)
    //    - StopSFX()                               Stops sound with 'name' that isn't looped
}
