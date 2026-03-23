using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fadeInTextUI : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 1f; // Time it takes to fade in
    private TextMeshProUGUI text;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // Reset alpha to 0 instantly when enabled
        Color c = text.color;
        c.a = 0f;
        text.color = c;

        // Start fade in
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color c = text.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            text.color = c;
            yield return null;
        }

        // Ensure fully visible at the end
        c.a = 1f;
        text.color = c;
    }
}
