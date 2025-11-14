using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningImageFade : MonoBehaviour
{
    public enum FadeType
    {
        NONE,
        TOBLACK,
        FROMBLACK
    }

    [Header("Initialise in inspector:")]
    [SerializeField] Image fade;

    [Header("Set the following details about the fade")]
    [Header("fadeLength doesn't affect time of fade to/from black for the prev/next slide")]
    [SerializeField] float fadeLength;

    [HideInInspector] public FadeType fadeState { get; private set; } = FadeType.NONE;
    [HideInInspector] public bool fadeFromBlackOnEnable = false;
    Coroutine imageFade;

    private void OnEnable()
    {
        if (fadeFromBlackOnEnable) BeginImageFade(FadeType.FROMBLACK);
        else BeginImageFade(FadeType.NONE);
    }

    /// <summary>
    /// Starts the image fade, going either from transparent to black or visa-versa
    /// </summary>
    public void BeginImageFade(FadeType fadeType)
    {
        if (imageFade != null) StopCoroutine(imageFade);
        imageFade = StartCoroutine(Fade(fadeType));
    }

    /// <summary>
    /// Stops fading if it currently is, and sets the current fade to transparent
    /// </summary>
    public void SkipFade()
    {
        if (imageFade != null) StopCoroutine(imageFade);

        Color fadeColor = fade.color;
        fadeColor.a = 0;
        fade.color = fadeColor;

        fadeState = FadeType.NONE;
    }


    IEnumerator Fade(FadeType fadeType)
    {
        float elapsedTime = 0f;
        Color fadeColor = fade.color;
        bool proceed = true;

        if (fadeType == FadeType.TOBLACK) fadeColor.a = 0f;
        else if (fadeType == FadeType.FROMBLACK) fadeColor.a = 1f;
        else
        {
            fadeColor.a = 0f;
            fade.color = fadeColor;
            proceed = false;
            yield return null;
        }

        if(proceed)
        {
            fadeState = fadeType;

            while (elapsedTime < fadeLength)
            {
                elapsedTime += Time.deltaTime;

                fadeColor.a = elapsedTime / fadeLength;
                if (fadeType == FadeType.FROMBLACK) fadeColor.a = 1f - fadeColor.a;
                fade.color = fadeColor;
                yield return null;
            }

            fadeColor.a = 1f;
            if (fadeType == FadeType.FROMBLACK) fadeColor.a = 0f;
            fade.color = fadeColor;

            fadeState = FadeType.NONE;
        }
    }
}
