using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class materializeIn : MonoBehaviour
{
    private Coroutine fading;
    [SerializeField] float timeToFade;
    [SerializeField] float timeToFadeOut;
    [SerializeField] List<materializeIn> othersToFadeOut;
    [SerializeField] bool disableOnFade = false;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        color.a = 0;
        sprite.color = color;
    }

    private void OnEnable()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        if (fading != null)
            StopCoroutine(fading);
        fading = StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut()
    {
        if (fading != null)
            StopCoroutine(fading);
        fading = StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        foreach (materializeIn other in othersToFadeOut)
        {
            other.gameObject.SetActive(true);
        }

        Color color = sprite.color;
        float percentfadeIn = color.a;

        float elapsed = percentfadeIn * timeToFade;
        while (elapsed < timeToFade)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsed / timeToFade);
            color.a = alpha;
            sprite.color = color;

            yield return null;
        }
        color.a = 1;
        sprite.color = color;
    }

    IEnumerator FadeOutCoroutine()
    {
        foreach (materializeIn other in othersToFadeOut)
        {
            other.FadeOut();
        }

        Color color = sprite.color;
        float percentfadeOut = Mathf.Min(1 - color.a, 0);

        float elapsed = percentfadeOut * timeToFadeOut;
        while (elapsed < timeToFadeOut)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / timeToFadeOut);
            color.a = alpha;
            sprite.color = color;

            yield return null;
        }
        color.a = 0;
        sprite.color = color;
        if (disableOnFade) gameObject.SetActive(false);
    }
}
