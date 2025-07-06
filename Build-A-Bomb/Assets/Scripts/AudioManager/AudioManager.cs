using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Initialise In Inspector:
    [Header("---- Audio Clips ----\n")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    [Header("---- Audio Sources ----\n")]
    public SoundSource[] musicSourceList;
    public SoundSource[] sfxSourceList;

    // Runtime Variables:
    public static AudioManager instance;

    private void Awake()
    {
        // If we haven't already initialised an instance of the Audio manager
        if (instance == null)
        {
            // Make this instance a singleton
            DontDestroyOnLoad(gameObject);
            instance = this;

            // (Yu Gui Oh fusion solution! We make sure that both the Audio Manager
            // and Mixer Manager are present.)
            if (GetComponent<MixerFXManager>() == null)
            {
                Debug.LogWarning("MixerFXManager component is missing!");
                Debug.LogWarning("Please add component before running!");
            }

            // From here, sets up audio sources.
            // Set up SFX audio sources:
            // For each sfx source
            foreach (SoundSource sfxSource in sfxSourceList)
            {
                // For each sfx source reset the name and set it to not looping as default
                sfxSource.soundName = SoundSource.defaultName;
                sfxSource.audioSource.loop = false;

                // For each sfx source
                foreach (SoundSource j in sfxSourceList)
                {
                    // We check for duplicate usages
                    if (sfxSource == j)
                    {
                        continue;
                    }
                    else if (sfxSource.audioSource == j.audioSource)
                    {
                        Debug.LogWarning("Error, SFX source " + sfxSource.audioSource.name + " duplicate found!");
                    }
                }
            }

            // Set up Music audio sources:
            // For each music source
            foreach (SoundSource musicSource in musicSourceList)
            {
                // For each music source reset the name and set it to looping as default
                musicSource.soundName = SoundSource.defaultName;
                musicSource.audioSource.loop = true;

                // For each music 
                foreach (SoundSource j in musicSourceList)
                {
                    // Check for duplicates
                    if (musicSource == j)
                    {
                        continue;
                    }
                    else if (musicSource.audioSource == j.audioSource)
                    {
                        Debug.LogWarning("Error, music source " + musicSource.audioSource.name + " duplicate found!");
                    }
                }
            }
        }
        else
        {
            // Destroy this gameobject if another audio manager already exists (singleton)
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        // 
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        SoundSource source = Array.Find(musicSourceList, y => y.soundIsSelected == false);

        if (sound == null)
        {
            Debug.LogWarning("Error, music sound " + name + " not found!");
        }
        else if (source == null)
        {
            Debug.LogWarning("Error, no music source available!");
        }
        else
        {
            source.audioSource.volume = sound.volume;
            source.audioSource.pitch = sound.pitch;
            source.audioSource.panStereo = sound.panning;

            source.soundIsSelected = true;
            source.soundName = sound.name;
            source.audioSource.clip = sound.clip;
            source.audioSource.Play();

            if (Msg) Debug.Log("Music Played: " + source.soundName);
            if (Msg) Debug.Log("Name Check: " + sound.clip.name);
            if (Msg) Debug.Log("Source Used: " + source.audioSource.name);
        }
    }

    public void StopMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        SoundSource source = Array.Find(musicSourceList, y => y.soundName == name);

        if (sound == null)
        {
            Debug.LogWarning("Error, music sound " + name + " not found!");
        }
        else if (source == null)
        {
            Debug.LogWarning("Error, no music source found playing " + name + "!");
        }
        else
        {
            source.audioSource.Stop();
            source.soundName = SoundSource.defaultName;
            source.soundIsSelected = false;
        }
    }

    public void StopAllMusic()
    {
        bool playingCheck = false;

        foreach (SoundSource source in musicSourceList)
        {
            playingCheck |= source.soundIsSelected;
            
            source.audioSource.Stop();
            source.soundName = SoundSource.defaultName;
            source.soundIsSelected = false;
        }

        if (playingCheck)
        {
            Debug.LogWarning("Error, no music was playing!");
        }
    }

    public void PlaySFX(string name, float? volumeTemp = null)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        SoundSource source = Array.Find(sfxSourceList, y => y.audioSource.isPlaying == false);

        if (volumeTemp > 1 || volumeTemp < 0)
        {
            Debug.LogWarning("Error, volumeTemp, " + volumeTemp + ", is outside of range!");
            return;
        }

        if (sound == null)
        {
            Debug.LogWarning("Error, sfx sound " + name + " not found!");
        }
        else if (source == null)
        {
            Debug.LogWarning("Error, no sfx source available!");
        }
        else
        {
            source.audioSource.volume = volumeTemp ?? sound.volume;
            source.audioSource.pitch = sound.pitch;
            source.audioSource.panStereo = sound.panning;

            // TODO: change this
            if (true)
            {
                //source.audioSource.PlayOneShot(sound.clip, volume);
            }
            else
            {
                source.soundName = sound.name;
                source.audioSource.clip = sound.clip;
                source.audioSource.Play();
            }

            if (Msg) Debug.Log("Music Played: " + source.soundName);
            if (Msg) Debug.Log("Name Check: " + sound.clip.name);
            if (Msg) Debug.Log("Source Used: " + source.audioSource.name);
        }
    }

    
    public void StopSFX()
    {
        bool playingCheck = true;

        foreach (SoundSource source in sfxSourceList)
        {
            source.audioSource.Stop();
            
            if (source.soundName != SoundSource.defaultName)
            {
                playingCheck = false;
            }

            source.soundName = SoundSource.defaultName;
        }

        if (playingCheck)
        {
            Debug.LogWarning("Caution, no SFX have been played since start or last 'StopSFX()' call!");
        }
    }

    

    // Help

    // The AudioManager functions affect the basic elements of the sounds.
    // They change only the most fundamental aspects such as, whether a sound
    // is playing, stopped or it's base volume.

    //Functions:
    //  Functions for music sources.
    //    - PlayMusic(string name)                  Plays sound with 'name' on loop
    //    - StopMusic(string name)                  Stops sound with 'name'
    //    - StopAllMusic()                          Stops all looping tracks
    //
    //  Functions for SFX sources.
    //    - PlaySFX(string name, float? volumeTemp = null)
    //                                              Plays sound with 'name' once at 'volumeTemp'
    //                                              (if 'volumeTemp' is null the sound will play
    //                                              at default volume set in inspecter)
    //    - StopSFX()                               Stops all SFX and clears the names in the SFX sources
}
