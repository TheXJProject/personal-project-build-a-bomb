using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MixerGroupsInfo
{
    // Constant values:
    public const string errorName = "Error!";

    // Initialise In Inspector:
    [Header("---- Group Info ----\n")]
    [Header("The variable 'Name' will be filled in \nautomatically! Do not add manually.")]
    public string name;
    public AudioMixerGroup group;
    public MixerGroupExpoParameters parameters;
    public LinkedAudioSources linkedAudioSources;
}

[System.Serializable]
public class LinkedAudioSources
{
    // Initialise In Inspector:
    [Header("---- Linked Sources ----\n")]
    [Header(" This is for checking sources are linked \ncorrectly.")]
    public AudioSource[] audioSources;
}

[System.Serializable]
public class MixerGroupExpoParameters
{
    // Constant values:
    public const string errorName = "Error!";
    public const string defaultName = "Empty";

    // Initialise In Inspector:
    [Header("---- Groups Exposed Parameters ----\n")]
    [Header("These variables should be named \nor left blank.")]
    public string volume;
    public string lowPassEQ;
    
    // Runtime Variables:
    [HideInInspector] public float startVolume;
    [HideInInspector] public float startLowPassEQ;

    // (EX_PARA: Put all Exposed Parameters for a single channel here)
}

public enum EX_PARA
{
    VOLUME,
    LOW_PASS_EQ

    // (EX_PARA: Put all Exposed Parameters options here)
}