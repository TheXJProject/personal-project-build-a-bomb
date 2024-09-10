using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicSource
{
    [Header("---- Source Settings ----\n")]

    public string name = "Empty";
    public AudioSource musicSource;
    public bool playing = false;
}
