using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerFXManager : MonoBehaviour
{
    //          ++++++ Description at the bottom! ++++++

    public static MixerFXManager instance;

    public AudioMixer audioMixer;

    [Header("---- Mixer Groups ----")]
    public MixerGroupsInfo[] groups;

    private bool isFading = false;

private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (GetComponent<AudioManager>() == null) // (Yu Gui Oh fusion solution)
            {
                Debug.LogWarning("AudioManager component is missing!");
                Debug.LogWarning("Please add component before running!");
            }

            // From here, sets up audio channels.

            foreach (MixerGroupsInfo g in groups)
            {
                if (g.group == null)
                {
                    Debug.LogWarning("Error, group reference missing in 'Mixer Groups'!");
                    
                    g.name = MixerGroupsInfo.errorName;
                    g.parameters.volume = MixerGroupExpoParameters.errorName;
                    g.parameters.lowPassEQ = MixerGroupExpoParameters.errorName;
                    // (Add in extra parameters if added to!)
                }
                else
                {
                    g.name = g.group.name;

                    if (g.parameters.volume == null)
                    {
                        g.parameters.volume = MixerGroupExpoParameters.defaultName;
                    }
                    else //if (exists on group)
                    {

                    }
                }
                // rest
            }

            // Set up mixing channels:

            //foreach (var group in audioMixer.groups)
            //{
            //    group.audioMixer.SetFloat(group.name + "_Volume", 0);
            //    group.audioMixer.SetFloat(group.name + "_Mute", 0);
            //}

            // Set up post mix channels:

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
