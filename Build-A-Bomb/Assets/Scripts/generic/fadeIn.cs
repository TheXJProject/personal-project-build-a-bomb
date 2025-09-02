using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeIn : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Time it takes to fade in
    private SpriteRenderer spriteRenderer;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // Reset alpha to 0 instantly when enabled
        Color c = spriteRenderer.color;
        c.a = 0f;
        spriteRenderer.color = c;

        // Start fade in
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            spriteRenderer.color = c;
            yield return null;
        }

        // Ensure fully visible at the end
        c.a = 1f;
        spriteRenderer.color = c;
    }
}
