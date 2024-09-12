using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicSource
{
    public const string defaultName  = "Empty";

    [Header("---- Source Settings ----\n")]
    public string songName;
    public AudioSource musicSource;
    public bool playing = false;

}
