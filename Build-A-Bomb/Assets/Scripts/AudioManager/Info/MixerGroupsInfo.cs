using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MixerGroupsInfo
{
    public const string errorName = "Error!";

    [Header("---- Group Info ----\n")]

    [Header("The variable 'Name' will be filled in \nautomatically! Do not add manually.")]
    public string name;

    public AudioMixerGroup group;
    public MixerGroupExpoParameters parameters;
    public LinkedAudioSources LinkedAudioSources;
}

[System.Serializable]
public class MixerGroupExpoParameters
{
    public const string errorName = "Error!";
    public const string defaultName = "Empty";

    [Header("---- Groups Exposed Parameters ----\n")]

    [Header("These variables should be named \nor left blank.")]
    
    [Header("---- ===== Volume ===== ----")]
    public string volume;
    public float startVolume;

    [Header("---- ===== LowPassEQ ===== ----")]
    public string lowPassEQ;
    public float startLowPassEQ;
    // Put all Exposed Parameters for a single channel here.
}

[System.Serializable]
public class LinkedAudioSources
{
    [Header("---- Linked Sources ----\n")]
    public AudioSource[] audioSources;
}

