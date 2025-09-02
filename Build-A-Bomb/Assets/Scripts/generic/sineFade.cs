using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineFade : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [Range(0f, 1f)] public float minAlpha = 0f;
    [Range(0f, 1f)] public float maxAlpha = 1f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // Oscillates smoothly between -1 and 1
        float sine = Mathf.Sin(Time.time * speed);

        // Normalize sine to 0 - 1
        float normalized = (sine + 1f) * 0.5f;

        // Remap to minAlpha - maxAlpha
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, normalized);

        // Apply new alpha
        Color newColor = originalColor;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}
