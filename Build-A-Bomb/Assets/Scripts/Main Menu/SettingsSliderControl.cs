using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSliderControl : MonoBehaviour
{
    enum SETTING_OPTIONS
    {
        MASTER,
        MUSIC,
        SFX,
        NULL
    } 

    [SerializeField] SETTING_OPTIONS option;

    public void ChangeSetting(float level)
    {
        switch (option)
        {
            case SETTING_OPTIONS.MASTER: MixerFXManager.instance.SetPlayerMaster(level); break;
            case SETTING_OPTIONS.MUSIC: MixerFXManager.instance.SetPlayerMusic(level); break;
            case SETTING_OPTIONS.SFX: MixerFXManager.instance.SetPlayerSfx(level); break;
            default: Debug.LogWarning("Error, settings option not set"); break;
        }
    }
}
