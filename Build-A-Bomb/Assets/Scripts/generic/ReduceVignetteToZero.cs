using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ReduceVignetteToZero : MonoBehaviour
{
    [Header("Initialise in inspector")]
    [SerializeField] Volume volume;

    [Header("Set how long it takes for the Vignette to fade away")]
    [SerializeField] float fadeTime;

    Vignette vignette;
    float startingIntensity;

    private void Awake()
    {
        volume.profile.TryGet(out vignette);
        if ( vignette != null ) startingIntensity = vignette.intensity.value;
    }

    //private void OnEnable()
    //{
    //    GameStartCount.onCountdownFinished += StartVignetteFade;
    //}

    //private void OnDisable()
    //{
    //    GameStartCount.onCountdownFinished -= StartVignetteFade;

    //}

    private void Start()
    {
        StartVignetteFade();
    }

    void StartVignetteFade()
    {
        if (CheatLogic.cheatTool.GetNoCountdown())
        {
            vignette.intensity.value = 0;
            return;
        }
        StartCoroutine(VignetteFade());
    }

    IEnumerator VignetteFade()
    {
        if (vignette != null)
        {
            vignette.intensity.value = startingIntensity;
            float timer = fadeTime;
            do
            {
                vignette.intensity.value = timer/fadeTime;
                timer -= Time.deltaTime;
                yield return null;
            } while (timer > 0);
            vignette.intensity.value = 0;
        }
        else yield return null;
    }
}
