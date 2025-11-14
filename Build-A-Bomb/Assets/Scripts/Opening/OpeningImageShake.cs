using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningImageShake : MonoBehaviour
{
    [Header("Set the curve and length of the shaking")]
    [SerializeField] AnimationCurve shakeCurve;
    [SerializeField] float shakeLength;

    [HideInInspector] public bool shakeOnEnable = false;
    Coroutine imageShake;
    RectTransform rectTransform;
    Vector2 startPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (shakeOnEnable) BeginImageShake();
    }

    public void BeginImageShake()
    {
        startPos = rectTransform.anchoredPosition;
        if (imageShake != null) StopCoroutine(imageShake);
        imageShake = StartCoroutine(Shake());
    }


    IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeLength)
        {
            elapsedTime += Time.deltaTime;
            float strength = shakeCurve.Evaluate(elapsedTime / shakeLength);
            rectTransform.anchoredPosition = startPos + Random.insideUnitCircle * strength;
            yield return null;
        }
        rectTransform.anchoredPosition = startPos;
    }
}
