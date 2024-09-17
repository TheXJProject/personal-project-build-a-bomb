using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public const float defaultVolume = 1f;
    public const float defaultPitch = 1f;
    public const float defaultPanning = 0f;

    [Header("---- Sound Settings ----\n")]

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = defaultVolume;

    [Range(-3f, 3f)]
    public float pitch = defaultPitch;

    [Range(-1f, 1f)]
    public float panning = defaultPanning;
}

public struct TempSoundAspectsStorage
{
    public float volume;
    public float pitch;
    public float panning;
}