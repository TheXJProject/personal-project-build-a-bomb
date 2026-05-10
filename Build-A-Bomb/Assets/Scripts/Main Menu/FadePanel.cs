using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
    [SerializeField] float timeToFade;
    private SpriteRenderer sprite;
    private Coroutine fading;


    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
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
        Color color = sprite.color;
        float percentfadeOut = Mathf.Min(1 - color.a, 0);

        float elapsed = percentfadeOut * timeToFade;
        while (elapsed < timeToFade)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / timeToFade);
            color.a = alpha;
            sprite.color = color;

            yield return null;
        }
        color.a = 0;
        sprite.color = color;
    }
}
