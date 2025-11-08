using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CrazyColourBomb : MonoBehaviour
{
    public static event Action<float> onCrazyExplosionStarted;
    public static event Action<float> onCrazyVictoryStarted;

    [SerializeField] float crazyColoursLength;
    [SerializeField] Volume vol;

    [Header("Adjust the colour adjustment start and end values")]
    [SerializeField] float startExposure;
    [SerializeField] float endExposure;
    [SerializeField] float startHueShift;
    [SerializeField] float endHueShift;
    private ColorAdjustments coladjus;

    private void Awake()
    {
        vol.profile.TryGet(out coladjus);
    }

    Coroutine crazyColours;
    public void StartCrazyColours()
    {
        if (crazyColours != null) StopCoroutine(crazyColours);
        onCrazyExplosionStarted?.Invoke(crazyColoursLength);
        crazyColours = StartCoroutine(CrazyColours());
    }
    public void StartCrazyColoursVictory()
    {
        if (crazyColours != null) StopCoroutine(crazyColours);
        crazyColours = StartCoroutine(CrazyColours());
    }

    public void StartCameraShakeVictory()
    {
        onCrazyVictoryStarted?.Invoke(crazyColoursLength);
    }

    IEnumerator CrazyColours()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < crazyColoursLength)
        {
            elapsedTime += Time.deltaTime;
            float t = (elapsedTime / crazyColoursLength);
            coladjus.hueShift.value = Mathf.Lerp(startHueShift, endHueShift, t);
            coladjus.postExposure.value = Mathf.Lerp(startExposure, endExposure, t);
            yield return null;
        }

        coladjus.hueShift.value = endHueShift;
        coladjus.postExposure.value = endExposure;
    }
}
