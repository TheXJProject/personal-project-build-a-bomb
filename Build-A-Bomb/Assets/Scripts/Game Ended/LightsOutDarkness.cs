using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightsOutDarkness : MonoBehaviour
{
    [SerializeField] GameObject darkness;
    [SerializeField] Volume vol;
    private ColorCurves colCur;

    private void Awake()
    {
        vol.profile.TryGet(out colCur);
    }

    private void OnEnable()
    {
        Death.onGameOver += EnableDarkness;
    }

    private void OnDisable()
    {
        Death.onGameOver -= EnableDarkness;
    }

    void EnableDarkness()
    {
        colCur.active = true;
        darkness.SetActive(true);
    }
}
