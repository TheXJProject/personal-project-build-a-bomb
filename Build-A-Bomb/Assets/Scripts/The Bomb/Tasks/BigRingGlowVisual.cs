using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRingGlowVisual : MonoBehaviour
{
    // Initialise in Inspector
    public Transform glowTransform;

    // Inspector Adjustable Values
    [Range(0f, 1f)] public float minFlickerHeight = 0.5f;
    [Range(0f, 1f)] public float maxFlickerHeight = 0.5f;
    [Range(0.2f, 10f)] public float flickerSharpness = 0.5f;
    [Range(0f, 15f)] public float flickerDensity = 0.5f;
    [Range(0f, 3f)] public float flickerTime = 1.5f;
    [Range(0f, 10f)] public float flickerInterval = 2f;
    [Range(0f, 3f)] public float flickerIntervalRandomness = 0.5f;

    // Runtime Variables
    float startSize;
    float secondsTillNextFlicker = 0f;
    float heightOfNextFlicker;
    float currentSize;
    float currentTime;
    float t;
    bool flickering = false;
    System.Random rnd = new System.Random();

    private void Awake()
    {
        startSize = glowTransform.localScale.x;
        DetermineNextFlicker();
        resetFlicker();
    }

    private void Update()
    {
        if (flickering)
        {
            // Calculates flicker using bell curve of following equation (paste into desmos): y(x)=a\left(\exp(-\frac{(x)^{2d}}{0.5^{c}})\right)+1
            float exponent = Mathf.Pow((0.5f-t), 2 * flickerSharpness);
            exponent /= Mathf.Pow(0.5f, flickerDensity);
            currentSize = startSize * (heightOfNextFlicker * Mathf.Exp(-exponent) + 1f);
            glowTransform.localScale = new Vector3(currentSize, currentSize, 1);

            // Set time in transition
            currentTime += Time.deltaTime;
            t = currentTime / flickerTime;
            if (t > 0.5f) t = 1 - t;
            else if (t < 0) resetFlicker();
        }

        secondsTillNextFlicker -= Time.deltaTime;
        if (secondsTillNextFlicker < 0f)
        {
            TriggerFlicker();
            DetermineNextFlicker();
        }
    }

    void resetFlicker()
    {
        currentTime = 0;
        t = currentTime / flickerTime;
        currentSize = startSize;
        flickering = false;
    }

    void TriggerFlicker()
    {
        resetFlicker();
        flickering = true;
    }

    void DetermineNextFlicker()
    {
        int minTime = (int)((flickerInterval - flickerIntervalRandomness) * 100f);
        int maxTime = (int)((flickerInterval + flickerIntervalRandomness) * 100f);
        int minSize = (int)(minFlickerHeight * 100);
        int maxSize = (int)(maxFlickerHeight * 100);
        secondsTillNextFlicker = rnd.Next(minTime, maxTime + 1);
        heightOfNextFlicker = rnd.Next(minSize, maxSize + 1);
        secondsTillNextFlicker /= 100f;
        heightOfNextFlicker /= 100f;
    }
}
