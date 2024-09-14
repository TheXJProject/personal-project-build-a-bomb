using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerFXManager : MonoBehaviour
{
    public static MixerFXManager instance;

    public AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (GetComponent<AudioManager>() == null)
            {
                Debug.LogWarning("AudioManager component is missing!");
                Debug.LogWarning("Please add component before running!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Dosomething()
    {
        // beltch
    }
}
