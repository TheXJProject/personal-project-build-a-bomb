using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    readonly bool debugTests = false; // 'true' for debugging only!!!!!

    readonly bool gamePaused = false; // This temporarily is used in place of a global variable

    //          ++++++ Description at the bottom! ++++++

    public static AudioManager instance;

    [Header("---- Audio Clips ----\n")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    
    [Header("---- Audio Sources ----\n")]
    public SoundSource[] sfxSourceList;
    public SoundSource[] musicSourceList;

    TempSoundAspectsStorage[] tempSoundAspectsList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (GetComponent<MixerFXManager>() == null) // (Yu Gui Oh fusion solution)
            {
                Debug.LogWarning("MixerFXManager component is missing!");
                Debug.LogWarning("Please add component before running!");
            }

            // Initialise sound aspect array and it's elements.

            tempSoundAspectsList = new TempSoundAspectsStorage[sfxSourceList.Length];

            for (int i = 0; i < sfxSourceList.Length; i++)
            {
                tempSoundAspectsList[i].volume = sfxSourceList[i].audioSource.volume;
                tempSoundAspectsList[i].pitch = sfxSourceList[i].audioSource.pitch;
                tempSoundAspectsList[i].panning = sfxSourceList[i].audioSource.panStereo;
            }

            // From here, sets up audio sources.
            // Set up SFX audio sources:

            foreach (SoundSource sfxSource in sfxSourceList)
            {
                sfxSource.soundName = SoundSource.defaultName;
                sfxSource.audioSource.loop = false;

                foreach (SoundSource j in sfxSourceList)
                {
                    if (sfxSource == j)
                    {
                        continue;
                    }

                    if (sfxSource.audioSource == j.audioSource)
                    {
                        Debug.LogWarning("Error, SFX source " + sfxSource.audioSource.name + " duplicate found!");
                    }
                }
            }

            // Set up Music audio sources:

            foreach (SoundSource musicSource in musicSourceList)
            {
                musicSource.soundName = SoundSource.defaultName;
                musicSource.audioSource.loop = true;

                foreach (SoundSource j in musicSourceList)
                {
                    if (musicSource == j)
                    {
                        continue;
                    }

                    if (musicSource.audioSource == j.audioSource)
                    {
                        Debug.LogWarning("Error, music source " + musicSource.audioSource.name + " duplicate found!");
                    }
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Functions for music sources.

    public void PlayMusic(string name)
    {
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

            if (debugTests) Debug.Log("Music Played: " + source.soundName);
            if (debugTests) Debug.Log("Name Check: " + sound.clip.name);
            if (debugTests) Debug.Log("Source Used: " + source.audioSource.name);
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

    public void PauseAllMusic()
    {
        bool playingCheck = false;

        foreach (SoundSource source in musicSourceList)
        {
            playingCheck |= source.soundIsSelected;

            source.audioSource.Pause();
        }

        if (playingCheck)
        {
            Debug.LogWarning("Error, no music to pause!");
        }
    }

    public void ResumeAllMusic()
    {
        bool playingCheck = false;

        foreach (SoundSource source in musicSourceList)
        {
            playingCheck |= source.soundIsSelected;

            source.audioSource.UnPause();
        }

        if (playingCheck)
        {
            Debug.LogWarning("Error, no music to resume!");
        }
    }

    // Functions for SFX sources.

    public void PlaySFX(string name, float? volumeTemp = null)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        SoundSource source = Array.Find(sfxSourceList, y => y.audioSource.isPlaying == false);

        float volume = volumeTemp ?? sound.volume;

        if (volume > 1 || volume < 0)
        {
            Debug.LogWarning("Error, volume, " + volume + ", is outside of range!");
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
            source.audioSource.volume = volume;
            source.audioSource.pitch = sound.pitch;
            source.audioSource.panStereo = sound.panning;

            if (gamePaused)
            {
                source.audioSource.PlayOneShot(sound.clip, volume);
            }
            else
            {
                source.soundName = sound.name;
                source.audioSource.clip = sound.clip;
                source.audioSource.Play();
            }

            if (debugTests) Debug.Log("Music Played: " + source.soundName);
            if (debugTests) Debug.Log("Name Check: " + sound.clip.name);
            if (debugTests && gamePaused) Debug.Log("(Game should be paused, so these above two may be different!)");
            if (debugTests) Debug.Log("Source Used: " + source.audioSource.name);
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

    public void PauseSFX()
    {
        bool playingCheck = true;

        if (gamePaused)
        {
            for (int i = 0; i < sfxSourceList.Length; i++)
            {
                tempSoundAspectsList[i].volume = sfxSourceList[i].audioSource.volume;
                tempSoundAspectsList[i].pitch = sfxSourceList[i].audioSource.pitch;
                tempSoundAspectsList[i].panning = sfxSourceList[i].audioSource.panStereo;
            }
        }
        else
        {
            Debug.LogWarning("Error, game not currently paused!");
            return;
        }

        foreach (SoundSource source in sfxSourceList)
        {
            source.audioSource.Pause();

            if (source.soundName != SoundSource.defaultName)
            {
                playingCheck = false;
            }
        }

        if (playingCheck)
        {
            Debug.LogWarning("Caution, no SFX have been played since start or last 'StopSFX()' call!");
        }
    }

    public void ResumeSFX()
    {
        bool playingCheck = true;

        if (!gamePaused)
        {
            for (int i = 0; i < sfxSourceList.Length; i++)
            {
                sfxSourceList[i].audioSource.volume = tempSoundAspectsList[i].volume;
                sfxSourceList[i].audioSource.pitch = tempSoundAspectsList[i].pitch;
                sfxSourceList[i].audioSource.panStereo = tempSoundAspectsList[i].panning;
            }
        }
        else
        {
            Debug.LogWarning("Error, game is still paused!");
            return;
        }

        foreach (SoundSource source in sfxSourceList)
        {
            source.audioSource.UnPause();

            if (source.soundName != SoundSource.defaultName)
            {
                playingCheck = false;
            }
        }

        if (playingCheck)
        {
            Debug.LogWarning("Caution, no SFX have been played since start or last 'StopSFX()' call!");
        }
    }

    // The AudioManager functions affect the basic elements of the sounds.
    // They change only the most fundamental aspects such as, whether a sound
    // is playing, stopped or it's base volume.

    //Functions:
    //  Functions for music sources.
    //    - PlayMusic(string name)                  Plays sound with 'name' on loop
    //    - StopMusic(string name)                  Stops sound with 'name'
    //    - StopAllMusic()                          Stops all looping tracks
    //    - PauseAllMusic()                         Pauses all music
    //    - ResumeAllMusic()                        Resumes all music
    //
    //  Functions for SFX sources.
    //    - PlaySFX(string name, float? volumeTemp = null)
    //                                              Plays sound with 'name' once at 'volumeTemp'
    //                                              (if 'volumeTemp' is null the sound will play
    //                                              at default volume set in inspecter)
    //    - StopSFX()                               Stops all SFX and clears the names in the SFX sources
    //    - PauseSFX()                              Pauses all current playing  SFX
    //    - ResumeSFX()                             Resumes all previously paused SFX
}
