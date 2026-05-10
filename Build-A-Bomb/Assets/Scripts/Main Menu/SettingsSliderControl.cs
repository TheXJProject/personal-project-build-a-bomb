using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        float initialValue = 0;
        switch (option)
        {
            case SETTING_OPTIONS.MASTER: initialValue = MixerFXManager.instance.GetPlayerMaster(); break;
            case SETTING_OPTIONS.MUSIC: initialValue = MixerFXManager.instance.GetPlayerMusic(); break;
            case SETTING_OPTIONS.SFX: initialValue = MixerFXManager.instance.GetPlayerSfx(); break;
            default: Debug.LogWarning("Error, settings option not set"); break;
        }
        gameObject.GetComponent<Slider>().value = initialValue;
    }

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
