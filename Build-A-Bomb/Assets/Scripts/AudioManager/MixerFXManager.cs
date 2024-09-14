using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerFXManager : MonoBehaviour
{
    public static MixerFXManager instance;

    public AudioMixer audioMixer;

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

    //funcitons

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
            //do fade
        }
    }

    public void MuteMusic(bool mute)
    {
        //do mute/unmute
    }

    public void MuteSFX(bool mute)
    {
        //do mute/unmute
    }

    // The AudioManager functions affect the basic elements of the sounds.
    // They change only the most fundamental aspects such as, whether a sound
    // is playing, stopped or it's base volume.

    //Functions:
    //  Functions for mix.
    //    - FadeInMusicChannel(//channel)           Brings in
    //
    //  Functions for post mix.
    //    - FadeAllIn()                             Fades in all sound if previously faded out (doesn't affect other FX)
    //    - FadeAllOut()                            Fades out all sound if previously faded in (doesn't affect other FX)
    //    - MuteMusic(bool mute)                    Mutes all music (mute = true/ mute = false)
    //    - MuteSFX(bool mute)                      Mutes all SFX (mute = true/ mute = false)
}
