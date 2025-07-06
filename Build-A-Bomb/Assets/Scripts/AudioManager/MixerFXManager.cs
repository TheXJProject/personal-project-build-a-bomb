using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class MixerFXManager : MonoBehaviour
{
    //          ++++++ Description at the bottom! ++++++

    public static MixerFXManager instance;

    public AudioMixer audioMixer;

    [Header("---- Mixer Groups ----\n")]
    public MixerGroupsInfo[] groups;

    private bool isFading = false;

    private void Awake()
    {
        if (instance == null)
        {
            // Make this instance a singleton
            DontDestroyOnLoad(gameObject);
            instance = this;

            // (Yu Gui Oh fusion solution! We make sure that both the Audio Manager
            // and Mixer Manager are present.)
            if (GetComponent<AudioManager>() == null) 
            {
                Debug.LogWarning("AudioManager component is missing!");
                Debug.LogWarning("Please add component before running!");
            }

            // From here, sets up audio channels.
            // Check if correct group info has been filled in and if it is valid.

            foreach (MixerGroupsInfo g in groups)
            {
                if (g.group == null)
                {
                    Debug.LogWarning("Error, group reference missing in 'Mixer Groups'!");
                    
                    g.name = MixerGroupsInfo.errorName;
                    g.parameters.volume = MixerGroupExpoParameters.errorName;
                    g.parameters.lowPassEQ = MixerGroupExpoParameters.errorName;
                    // ===== (Add in extra parameters if added to!) =====
                }
                else
                {
                    g.name = g.group.name;

                    // Check if parameters have been filled in.

                    if (g.parameters.volume == "") // ===== Volume =====
                    {
                        g.parameters.volume = MixerGroupExpoParameters.defaultName;
                    }
                    else if (!audioMixer.GetFloat(g.parameters.volume, out g.parameters.startVolume))
                    {
                        Debug.LogWarning("Error, parameter with name, " + ((g.parameters.volume == "") ? "none" : g.parameters.volume) + ", cannot be found in the mixer!");
                        g.parameters.volume = MixerGroupExpoParameters.errorName;
                    }

                    if (g.parameters.lowPassEQ == "") // ===== LowPassEQ =====
                    {
                        g.parameters.lowPassEQ = MixerGroupExpoParameters.defaultName;
                    }
                    else if (!audioMixer.GetFloat(g.parameters.lowPassEQ, out g.parameters.startLowPassEQ))
                    {
                        Debug.LogWarning("Error, parameter with name, " + ((g.parameters.lowPassEQ == "") ? "none" : g.parameters.lowPassEQ) + ", cannot be found in the mixer!");
                        g.parameters.lowPassEQ = MixerGroupExpoParameters.errorName;
                    }
                    // ===== (Add in extra parameters if added to!) =====

                    // Check if audiosources exist in Audio Manager.

                    foreach (AudioSource audioSource in g.LinkedAudioSources.audioSources)
                    {
                        SoundSource sourceMusic = Array.Find(AudioManager.instance.musicSourceList, y => y.audioSource == audioSource);
                        if(sourceMusic == null)
                        {
                            SoundSource sourceSFX = Array.Find(AudioManager.instance.sfxSourceList, y => y.audioSource == audioSource);
                            if (sourceSFX == null)
                            {
                                Debug.LogWarning("Error, audiosource, " + ((audioSource == null) ? "none" : audioSource.name) + ", not found in Audio Manager!");
                            }
                        }

                        // Check if audiosources are linked correctly.
                        if (audioSource != null)
                        {
                            if (audioSource.outputAudioMixerGroup.name != g.group.name)
                            {
                                Debug.LogWarning("Error, audiosource, " + ((audioSource == null) ? "none" : audioSource.name) + ", not outputting to this group, " + g.group.name + "!");
                            }
                        }
                    }
                }
                // rest
            }

            // Set up mixing channels initial settings:

            //foreach (var group in audioMixer.groups)
            //{
            //    group.audioMixer.SetFloat(group.name + "_Volume", 0);
            //    group.audioMixer.SetFloat(group.name + "_Mute", 0);
            //}

            // Set up post mix channels initial settings:

            //foreach (group in audioMixer.groups)
            //{
            //    group.unmute;
            //    group.volume = 0;
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Functions for mix.

    public void FadeInMusicChannel(string name)
    {

    }

    // Functions for post mix.

    public void FadeAllIn()
    {
        if (isFading)
        {
            Debug.LogWarning("Error, sounds are alreadying fading!");
            Debug.LogWarning("'FadeIn' was called.");
        }
        else
        {
            isFading = true;
            //do fade
        }
    }

    public void FadeAllOut()
    {
        if (isFading)
        {
            Debug.LogWarning("Error, sounds are alreadying fading!");
            Debug.LogWarning("'FadeOut' was called.");
        }
        else
        {
            isFading = true;
            //do fade
        }
    }

    IEnumerator Fader(bool fadeIn)
    {
        yield return null;
    }

    // The MixerFXManager functions affect the various channels in the audio mixer.
    // They change the pre-master volume and can apply FX to the channels.

    //Functions:
    //  Functions for mix.
    //    - FadeInMusicChannel(//channel)           Brings in
    //
    //  Functions for post mix.
    //    - FadeAllIn()                             Fades in all sound if previously faded out (doesn't affect other FX)
    //    - FadeAllOut()                            Fades out all sound if previously faded in (doesn't affect other FX)
}
