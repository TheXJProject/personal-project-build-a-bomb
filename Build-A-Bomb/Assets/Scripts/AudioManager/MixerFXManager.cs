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
    [Header("_Main_")]
    public MainGroups mainGroups;
    [Header("_All Group_")]
    public MixerGroupsInfo[] groups;

    // Runtime Variables:
    public static MixerFXManager instance;
    private readonly Dictionary<(MixerGroupsInfo, EX_PARA), Coroutine> activeFades = new();

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

        // For each main group collection
        foreach (GROUP_OPTIONS option in System.Enum.GetValues(typeof(GROUP_OPTIONS)))
        {
            // Make sure at least one group is assigned
            if (mainGroups.GroupOptionToArray(option).Length == 0)
            {
                Debug.LogWarning("Error, main group " + option + " not filled in!");
                Debug.LogWarning("Please add component before running! Cheers :)");
            }
            else
            {
                // For each individual group in a collection
                foreach (AudioMixerGroup g in mainGroups.GroupOptionToArray(option))
                {
                    // Check it exsits in somewhere in the set of all groups
                    if (!Array.Exists(groups, gr => gr.group == g))
                    {
                        Debug.LogWarning("Error, main group " + g.name + " not in the main group collection!");
                    }
                }
            }
        }

        // Check main group array filled in
        if (groups.Length == 0)
        {
            Debug.LogWarning("Error, main group array empty!");
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
        float targetValue = 0;
        string expoParam = "";

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
        // If the paramater is already fading
        else if (activeFades.TryGetValue((groupToUse, param), out Coroutine exists))
        {
            // Stop the coroutine and remove it from active fades
            StopCoroutine(exists);
            activeFades.Remove((groupToUse, param));
        }

        // From the group to use, and the param type, get the correct exposed parameter and start value (on game loadup)
        expoParam = GetExposedParams(groupToUse, param).Item1;
        targetValue = GetExposedParams(groupToUse, param).Item2;

        // Get the current value of the exposed parameter
        if (!audioMixer.GetFloat(expoParam, out float currentValue))
        {
            // Throw error if this fails
            Debug.LogWarning("Error, failed to get current value for " + expoParam);
            return;
        }

        // Convert and clamp (linear values should be between 0 and 1)
        currentValue = ConvertType(param, false, currentValue);
        targetValue = Mathf.Clamp01(value ?? ConvertType(param, false, targetValue));

        // Finally, kick off a coroutine that fades the value
        activeFades[(groupToUse, param)] = StartCoroutine(Fader(expoParam, (groupToUse, param), duration, currentValue, targetValue));
    }

    public void SetAllMusicParam(EX_PARA param, float duration, float? value = null)
    {
        float targetValue = 0;
        string expoParam = "";

        // For each group in Music overall
        foreach (AudioMixerGroup g in mainGroups.musicOverall)
        {
            // Error check
            if (g == null)
            {
                Debug.LogWarning("Error, group in musicOverall is null.");
                continue;
            }

            // Then we check each element of our array of groups to find the right infomation
            MixerGroupsInfo groupToUse = Array.Find(groups, y => y.group == g);

            // If the paramater is already fading
            if (activeFades.TryGetValue((groupToUse, param), out Coroutine exists))
            {
                // Stop the coroutine and remove it from active fades
                StopCoroutine(exists);
                activeFades.Remove((groupToUse, param));
            }

            // From the group to use, and the param type, get the correct exposed parameter and start value (on game loadup)
            expoParam = GetExposedParams(groupToUse, param).Item1;
            targetValue = GetExposedParams(groupToUse, param).Item2;

            // Get the current value of the exposed parameter
            if (!audioMixer.GetFloat(expoParam, out float currentValue))
            {
                // Throw error if this fails
                Debug.LogWarning("Error, failed to get current value for " + expoParam);
                return;
            }

            // Convert and clamp (linear values should be between 0 and 1)
            currentValue = ConvertType(param, false, currentValue);
            targetValue = Mathf.Clamp01(value ?? ConvertType(param, false, targetValue));

            // Finally, kick off a coroutine that fades the value
            activeFades[(groupToUse, param)] = StartCoroutine(Fader(expoParam, (groupToUse, param), duration, currentValue, targetValue));
        }
    }

    public void SetAllSfxParam(EX_PARA param, float duration, float? value = null)
    {
        float targetValue = 0;
        string expoParam = "";

        // For each group in sfx overall
        foreach (AudioMixerGroup g in mainGroups.sfxOverall)
        {
            // Error check
            if (g == null)
            {
                Debug.LogWarning("Error, group in sfxOverall is null.");
                continue;
            }

            // Then we check each element of our array of groups to find the right infomation
            MixerGroupsInfo groupToUse = Array.Find(groups, y => y.group == g);

            // If the paramater is already fading
            if (activeFades.TryGetValue((groupToUse, param), out Coroutine exists))
            {
                // Stop the coroutine and remove it from active fades
                StopCoroutine(exists);
                activeFades.Remove((groupToUse, param));
            }

            // From the group to use, and the param type, get the correct exposed parameter and start value (on game loadup)
            expoParam = GetExposedParams(groupToUse, param).Item1;
            targetValue = GetExposedParams(groupToUse, param).Item2;

            // Get the current value of the exposed parameter
            if (!audioMixer.GetFloat(expoParam, out float currentValue))
            {
                // Throw error if this fails
                Debug.LogWarning("Error, failed to get current value for " + expoParam);
                return;
            }

            // Convert and clamp (linear values should be between 0 and 1)
            currentValue = ConvertType(param, false, currentValue);
            targetValue = Mathf.Clamp01(value ?? ConvertType(param, false, targetValue));

            // Finally, kick off a coroutine that fades the value
            activeFades[(groupToUse, param)] = StartCoroutine(Fader(expoParam, (groupToUse, param), duration, currentValue, targetValue));
        }
    }

    public void ForceSetParam(GROUP_OPTIONS collection, EX_PARA param, float? value = null)
    {
        // We will force one group (or collection of groups) to cancel all coroutines and
        // be set to "value" for one of the exposed parameter types
        foreach (AudioMixerGroup group in mainGroups.GroupOptionToArray(collection))
        {
            // Find the group from our group info array that matches the group from the collection
            MixerGroupsInfo groupToSet = Array.Find(groups, y => y.group.name == group.name);

            // Error check
            if (groupToSet == null)
            {
                Debug.LogWarning("Error, failed to find group from info array!");
            }
            else
            {
                // If the paramater is fading
                if (activeFades.TryGetValue((groupToSet, param), out Coroutine exists))
                {
                    // Stop the coroutine and remove it from active fades
                    StopCoroutine(exists);
                    activeFades.Remove((groupToSet, param));
                }

                // "value" is either the starting value of the parameter if nothing was passed
                float targetValue = GetExposedParams(groupToSet, param).Item2;
                targetValue = Mathf.Clamp01(value ?? ConvertType(param, false, targetValue));

                // Get the exposed parameter too
                string exParam = GetExposedParams(groupToSet, param).Item1;
                
                // Set the value of the exposed parameter to target
                if (!audioMixer.SetFloat(exParam, ConvertType(param, true, targetValue)))
                {
                    // Throw error if this fails
                    Debug.LogWarning("Error, failed to set target value for " + exParam);
                }
            }
        }

        if (Msg) Debug.Log("Force set: " + collection);
        if (Msg) Debug.Log("Parameter type: " + param);
        if (Msg) Debug.Log("Target value: " + (value.HasValue ? value : "default"));
    }

    IEnumerator Fader(string exposedParam, (MixerGroupsInfo, EX_PARA) key, float duration, float current, float target)
    {
        if (Msg) Debug.Log("Fading " + exposedParam + ", " + key.Item2 + " over duration " + duration + ".");
        if (Msg) Debug.Log("Current value (linear) is " + current + ".");
        if (Msg) Debug.Log("Target value (linear) is " + target + ".");

        float elapsed = 0f;

        // If the duration is zero, or the current value is the same as the target
        if ((duration <= 0) || Mathf.Approximately(current, target))
        {
            // Set the value of the exposed parameter to target
            if (!audioMixer.SetFloat(exposedParam, ConvertType(key.Item2, true, target)))
            {
                // Throw error if this fails
                Debug.LogWarning("Error, failed to set target value for " + exposedParam);
            }

            // We don't need to iterate
            yield break;
        }

        // While 
        while (elapsed < duration)
        {
            // Add time of frame
            elapsed += Time.deltaTime;

            // Find the next value for the exposed parameter
            float completePercentage = Mathf.Clamp01(elapsed / duration);
            float nextValue = Mathf.Lerp(current, target, completePercentage);

            // Set the value of the exposed parameter to target, converting it to the correct type
            if (!audioMixer.SetFloat(exposedParam, ConvertType(key.Item2, true, nextValue)))
            {
                // Throw error if this fails
                Debug.LogWarning("Error, failed to set target value for " + exposedParam);
            }

            // Repeat each frame
            yield return null;
        }

        // Set the value of the exposed parameter to target
        if (!audioMixer.SetFloat(exposedParam, ConvertType(key.Item2, true, target)))
        {
            // Throw error if this fails
            Debug.LogWarning("Error, failed to set target value for " + exposedParam);
        }

        // Remove this coroutine from active routines
        activeFades.Remove(key);
    }

    (string, float) GetExposedParams(MixerGroupsInfo group, EX_PARA type)
    {
        // Attempt to get the exposed parameter and default targetvalue (starting value), determined by the type of parameter
        switch (type)
        {
            case EX_PARA.VOLUME:
                return (group.parameters.volume, group.parameters.startVolume);
            case EX_PARA.LOW_PASS_EQ:
                return (group.parameters.lowPassEQ, group.parameters.startLowPassEQ);

            // ===== (EX_PARA: Add in extra parameters if added to!) =====
            default:
                Debug.LogWarning("Error, errrm.. okay? Somehow incorrect enum passed: " + type);
                return (null, 0f);
        }
    }

    float ConvertType(EX_PARA type, bool toType, float inputValue)
    {
        // Return value depending on type
        switch (type)
        {
            case EX_PARA.VOLUME:
                // If we want to convert to type
                if (toType)
                {
                    // Convert the linear inputted volume to dB which uses a logarithmic scale
                    // If input is less than or equal to 0 change to minimum value (-80dB = Mathf.Log10(0.0001f) * 20)
                    return Mathf.Log10(inputValue <= 0 ? 0.0001f : inputValue) * 20f;
                }
                // Otherwise, we want to convert from type
                else
                {
                    // Convert the volume in dB to use a linear scale
                    // (the inverse of converting to a logarithmic scale)
                    return Mathf.Pow(10f, inputValue / 20f);
                }
            case EX_PARA.LOW_PASS_EQ:
                // If we want to convert to type
                if (toType)
                {
                    return 0f;
                }
                // Otherwise, we want to convert from type
                else
                {
                    return 0f;
                }

            // ===== (EX_PARA: Add in extra parameters if added to!) =====
            default:
                return 0f;
        }
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
