using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MixerGroupsInfo
{
    public const string errorName = "Error!";

    [Tooltip("The variable 'Name' will be filled in automatically!\nDo not add manually.")]
    public string name;

    public AudioMixerGroup group;
    public MixerGroupExpoParameters parameters;
}

[System.Serializable]
public class MixerGroupExpoParameters
{
    public const string errorName = "Error!";
    public const string defaultName = "Empty";

    // Put all Exposed Parameters for a single channel here.
    public string volume;
    public string lowPassEQ;
}

