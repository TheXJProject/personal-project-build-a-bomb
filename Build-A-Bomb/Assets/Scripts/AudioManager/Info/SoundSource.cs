using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSource
{
    public const string defaultName  = "Empty";

    [Header("---- Source Settings ----\n")]
    public string soundName = defaultName;
    public AudioSource audioSource;
    public bool soundIsSelected = false;
}
