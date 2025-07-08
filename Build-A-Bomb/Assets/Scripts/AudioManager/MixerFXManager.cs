using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections.Generic;

public class MixerFXManager : MonoBehaviour
{
    // ==== For Debugging ====
    [SerializeField] bool Msg = false;

    // Initialise In Inspector:
    public AudioMixer audioMixer;
    [Header("---- Mixer Groups ----\n")]
    public MixerGroupsInfo[] groups;

    // Runtime Variables:
    public static MixerFXManager instance;
    private Dictionary<(MixerGroupsInfo, EX_PARA), Coroutine> activeVolumeFades = new();

    private void Awake()
    {
        // If we haven't already initialised an instance of the Audio manager
        if (instance == null)
        {
            // Make this instance a singleton
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            // Destroy this
            Destroy(gameObject);
        }
    }

    public void InitialiseMixer()
    {
        // (Yu Gui Oh fusion solution! We make sure that both the Audio Manager
        // and Mixer Manager are present.)
        if (GetComponent<AudioManager>() == null)
        {
            Debug.LogWarning("AudioManager component is missing!");
            Debug.LogWarning("Please add component before running!");
        }

        // From here, sets up audio channels.
        // Check if correct group info has been filled in and if it is valid.
        // For each group construct
        foreach (MixerGroupsInfo g in groups)
        {
            // If not assigned a group from the mixer
            if (g.group == null)
            {
                // Throw error
                Debug.LogWarning("Error, group reference missing in 'Mixer Groups'!");

                g.name = MixerGroupsInfo.errorName;
                g.parameters.volume = MixerGroupExpoParameters.errorName;
                g.parameters.lowPassEQ = MixerGroupExpoParameters.errorName;

                // ===== (EX_PARA: Add in extra parameters if added to!) =====
            }
            else
            {
                // Otherwise, apply the groups name automatically
                g.name = g.group.name;

                // Check if parameters have been filled in
                // If empty
                if (g.parameters.volume == "") // ===== Volume =====
                {
                    // Apply the default name (Some parameters won't be used so we don't throw an error)
                    g.parameters.volume = MixerGroupExpoParameters.defaultName;

                    if (Msg) Debug.Log("Volume on " + g.name + " not used.");
                }
                // If we have inputted a name for a potential exposed parameter, check it exists and if it does
                // Assign the value, it currently has, to our "start" variable
                else if (!audioMixer.GetFloat(g.parameters.volume, out g.parameters.startVolume))
                {
                    // If we don't find a match throw error
                    Debug.LogWarning("Error, parameter with name, " + ((g.parameters.volume == "") ? "none" : g.parameters.volume) + ", cannot be found in the mixer!");
                    g.parameters.volume = MixerGroupExpoParameters.errorName;
                }

                // If empty
                if (g.parameters.lowPassEQ == "") // ===== LowPassEQ =====
                {
                    // Apply the default name (Some parameters won't be used so we don't throw an error)
                    g.parameters.lowPassEQ = MixerGroupExpoParameters.defaultName;

                    if (Msg) Debug.Log("Low Pass EQ on " + g.name + " not used.");
                }
                // If we have inputted a name for a potential exposed parameter, check it exists and if it does
                // Assign the value, it currently has, to our "start" variable
                else if (!audioMixer.GetFloat(g.parameters.lowPassEQ, out g.parameters.startLowPassEQ))
                {
                    // If we don't find a match throw error
                    Debug.LogWarning("Error, parameter with name, " + ((g.parameters.lowPassEQ == "") ? "none" : g.parameters.lowPassEQ) + ", cannot be found in the mixer!");
                    g.parameters.lowPassEQ = MixerGroupExpoParameters.errorName;
                }

                // ===== (EX_PARA: Add in extra parameters if added to!) =====

                // Check if audiosources exist in Audio Manager.
                // For each audio source we have linked to the mixer
                foreach (AudioSource audioSource in g.linkedAudioSources.audioSources)
                {
                    // Find the source in the audio manager, it could be sfx or music
                    SoundSource sourceMusic = Array.Find(AudioManager.instance.musicSourceList, y => y.audioSource == audioSource);
                    SoundSource sourceSFX = Array.Find(AudioManager.instance.sfxSourceList, y => y.audioSource == audioSource);

                    // If both don't exist, throw error
                    if ((sourceMusic == null) && (sourceSFX == null))
                    {
                        Debug.LogWarning("Error, audiosource, " + ((audioSource == null) ? "none" : audioSource.name) + ", not found in Audio Manager!");
                    }

                    // Check if audiosources are linked correctly.
                    if (audioSource != null)
                    {
                        // If the group, the audiosource is assigned to, is not the same as the name of the group construct
                        // then the audiosource has been assigned to the wrong group. So throw error
                        if (audioSource.outputAudioMixerGroup.name != g.group.name)
                        {
                            Debug.LogWarning("Error, audiosource, " + ((audioSource == null) ? "none" : audioSource.name) + ", not outputting to this group, " + g.group.name + "!");
                        }
                    }
                }
            }
        }

        if (Msg) Debug.Log("Finished Audio Setup.");
    }

    public void SetMusicParam(string name, EX_PARA param, float duration, float? value = null)
    {
        float currentValue = 0;
        float targetValue = 0;
        bool failed1 = false;

        // We want to set a parameter in a music group that's playing track with "name".
        // We need to know what parameter, how long it will take, and the value we're setting it to.
        // First, we find the group and get it's current value for that parameter.
        // To do that, find the audio source outputting the track
        SoundSource source = Array.Find(AudioManager.instance.musicSourceList, y => y.soundName == name);

        // Then we find which group the source is playing to. We check each element of our array of groups.
        // In each element, we check our list of audiosources to see if the list contains the source we're looking for.
        MixerGroupsInfo groupToUse = Array.Find(groups, x => Array.Exists(x.linkedAudioSources.audioSources, z => z == source.audioSource));

        // Error check
        if (groupToUse == null)
        {
            Debug.LogWarning("Error, can't find an group with audio source playing " + name + ".");
            return;
        }

        // Depending what parameter we want to change
        switch (param)
        {
            case EX_PARA.VOLUME:
                // Attempt to get the currentvalue and targetvalue (convert current volume from log to linear)
                failed1 = audioMixer.GetFloat(groupToUse.parameters.volume, out currentValue);
                currentValue = logToLinearVolume(currentValue);
                targetValue = Mathf.Clamp01(value ?? logToLinearVolume(groupToUse.parameters.startVolume));
                break;
            case EX_PARA.LOW_PASS_EQ:
                // Attempt to get the currentvalue and targetvalue (convert current low pass EQ from log to linear)
                failed1 = audioMixer.GetFloat(groupToUse.parameters.lowPassEQ, out currentValue);
                currentValue = logToLinearLowPassEQ(currentValue);
                targetValue = Mathf.Clamp01(value ?? logToLinearLowPassEQ(groupToUse.parameters.startLowPassEQ));
                break;

            // ===== (EX_PARA: Add in extra parameters if added to!) =====
            default:
                Debug.LogWarning("Error, errrm.. okay? Somehow incorrect enum passed: " + param);
                break;
        }

        // Error checks
        if (failed1)
        {
            Debug.LogWarning("Error, failed to get current value for " + param);
            return;
        }

        // Next, if pass "value" parameter is null, we find what the starting value for that parameter was on game lauch.


        // After that, we use the Fade function set off the fade


    }

    public void SetAllMusicParam()
    {


    }

    public void SetAllSfxParam()
    {

    }

    IEnumerator Fader(string exposedParam, float duration, FADE_TYPE fadeType)
    {
        if (Msg) Debug.Log("Fading " + exposedParam + " over duration: " + duration);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
        }

        yield return null;
    }

    float LinearVolumeToLog(float linearVolume)
    {
        // Convert the linear inputted volume to dB that use a logarithmic scale
        // If input is less than or equal to 0 change to minimum value (-80dB = Mathf.Log10(0.0001f) * 20)
        return Mathf.Log10(linearVolume <= 0 ? 0.0001f : linearVolume) * 20f;
    }

    float logToLinearVolume(float logVolume)
    {
        // Convert the linear inputted volume to dB that use a logarithmic scale
        // If input is less than or equal to 0 change to minimum value (-80dB = Mathf.Log10(0.0001f) * 20)
        return Mathf.Log10(linearVolume <= 0 ? 0.0001f : linearVolume) * 20f;
    }

    float LinearLowPassEQToLog(float linearVolume)
    {
        // between 10hz and 22000hz
    }

    float logToLinearLowPassEQ(float logVolume)
    {
        // between 10hz and 22000hz
    }

    // TODO: write this up
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
