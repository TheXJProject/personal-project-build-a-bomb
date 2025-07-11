using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MixerGroupsInfo
{
    // Constant values:
    public const string errorName = "Error!";
    
    // Inspector Adjustable Values:
    public string name;

    // Initialise In Inspector:
    [Header("---- Group Info ----\n")]
    [Header("The variable 'Name' will be filled in \nautomatically! Manual additions will be replaced.")]
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

    // Inspector Adjustable Values:
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

public enum GROUP_OPTIONS
{
    SFX_OVERALL,
    MUSIC_OVERALL,
    MUSIC_COLLECTION
}

[System.Serializable]
public class MainGroups
{
    // Initialise In Inspector:
    public AudioMixerGroup[] sfxOverall;
    public AudioMixerGroup[] musicOverall;
    public AudioMixerGroup[] musicCollection;

    public AudioMixerGroup[] GroupOptionToArray(GROUP_OPTIONS group)
    {
        // Return the array that represents the passed group
        return group switch
        {
            GROUP_OPTIONS.SFX_OVERALL => sfxOverall,
            GROUP_OPTIONS.MUSIC_OVERALL => musicOverall,
            GROUP_OPTIONS.MUSIC_COLLECTION => musicCollection,
            _ => throw new System.NotImplementedException(),
        };
    }
}

[System.Serializable]
public class PlayerGroups
{
    // Initialise In Inspector:
    public AudioMixerGroup masterPlayer;
    public AudioMixerGroup musicPlayer;
    public AudioMixerGroup sfxPlayer;
}