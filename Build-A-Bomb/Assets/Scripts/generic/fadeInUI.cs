using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeInUI : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Time it takes to fade in
    private Image image;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnEnable()
    {
        // Reset alpha to 0 instantly when enabled
        Color c = image.color;
        c.a = 0f;
        image.color = c;

        // Start fade in
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = image.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            image.color = c;
            yield return null;
        }

        // Ensure fully visible at the end
        c.a = 1f;
        image.color = c;
    }
}
