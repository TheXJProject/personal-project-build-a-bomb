using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ReduceVignetteToZero : MonoBehaviour
{
    [Header("Initialise in inspector")]
    [SerializeField] Volume volume;

    [Header("Set the length of time for vignette going away\n " +
        "in two parts. The reason for this being to make\n" +
        "it go quicker when the timer finishes")]
    [SerializeField] float fadeTime;
    [SerializeField] float postStartFadeTime;

    [Header("Then set the percent fading it has done when it speeds up:")]
    [SerializeField][Range(0f, 1f)] float percDoneWhenSpeedUp;

    Vignette vignette;
    float startingIntensity;

    private void Awake()
    {
        volume.profile.TryGet(out vignette);
        if ( vignette != null ) startingIntensity = vignette.intensity.value;
    }


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
                vignette.intensity.value = Mathf.Lerp(startingIntensity, startingIntensity * (1f - percDoneWhenSpeedUp), 1f - (timer / fadeTime));
                timer -= Time.deltaTime;
                yield return null;
            } while (timer > 0);

            timer = postStartFadeTime;
            do
            {
                vignette.intensity.value = Mathf.Lerp(startingIntensity * (1f - percDoneWhenSpeedUp), 0, 1f - (timer / postStartFadeTime));
                timer -= Time.deltaTime;
                yield return null;
            } while (timer > 0);

            vignette.intensity.value = 0;
        }
        else yield return null;
    }
}
