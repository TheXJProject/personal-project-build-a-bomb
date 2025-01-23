using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSource
{
    // Constant values:
    public const string defaultName  = "Empty";

    // Initialise In Inspector:
    [Header("---- Source Settings ----\n")]
    public AudioSource audioSource;

    // Runtime Variables:
    //[System.NonSerialized] public string soundName = defaultName;
    //[System.NonSerialized] public bool soundIsSelected = false;

    [HideInInspector] public string soundName = defaultName;
    [HideInInspector] public bool soundIsSelected = false;

    public string Sound => soundName; // Read-only property
    public bool IsSelected => soundIsSelected; // Read-only property
}
