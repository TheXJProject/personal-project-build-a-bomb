using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OpeningImageSlideIn : MonoBehaviour
{
    [Header("Debugging configuration")]
    [SerializeField] bool showFinalPosition = true;

    [Header("Configure victory animatic slide in:")]
    [SerializeField] float timeBeforeSlideIn;
    [SerializeField] float timeToSlideIn;

    [Header("Configure the beginning and final position")]
    [SerializeField] Vector2 beginScreenPos;
    [SerializeField] Vector2 finalScreenPos;

    [Header("Do you want it to slow down as it reaches its final position")]
    [SerializeField] bool smoothEnding = false;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (Application.isPlaying) rectTransform.anchoredPosition = beginScreenPos;
        else rectTransform.anchoredPosition = finalScreenPos;
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            Vector2 newPos;
            if (showFinalPosition) newPos = finalScreenPos;
            else newPos = beginScreenPos;

            rectTransform.anchoredPosition = newPos;
        }
    }

    public void BeginTransition()
    {
        StartCoroutine(WaitBeforeTransition());
    }

    IEnumerator WaitBeforeTransition()
    {
        yield return new WaitForSeconds(timeBeforeSlideIn);
        StartCoroutine(MoveToPosition());
    }

    IEnumerator MoveToPosition()
    {
        float elapsed = timeToSlideIn;

        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            float percent = elapsed / timeToSlideIn;
            if (smoothEnding) percent *= percent * percent * percent;
            percent = 1f - percent;
            rectTransform.anchoredPosition = Vector2.Lerp(beginScreenPos, finalScreenPos, percent);
            yield return null;
        }

        rectTransform.anchoredPosition = finalScreenPos;
    }
}
