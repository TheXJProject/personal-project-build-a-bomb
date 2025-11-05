using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteAlways]
public class VictoryAnimaticDoTransition : MonoBehaviour
{
    public UnityEvent onFinishedTransition;

    [Header("Debugging configuration")]
    [SerializeField] bool showFinalPosition = true;

    [Header("Configure victory animatic slide in")]
    [SerializeField] float timeBeforeFloatingIn;
    [SerializeField] float timeToFloatIn;
    [SerializeField] Vector2 offScreenAnchorPosition;
    [SerializeField] Vector2 endAnimaticAnchorPosition;
    [SerializeField] bool smoothEnding = false;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (Application.isPlaying) rectTransform.anchoredPosition = offScreenAnchorPosition;
        else rectTransform.anchoredPosition = endAnimaticAnchorPosition;
    }

    private void OnEnable()
    {
        BeginVictoryScreenAnimatic.onVictoryScreenAnimaticStart += BeginTransition;
    }
    private void OnDisable()
    {
        BeginVictoryScreenAnimatic.onVictoryScreenAnimaticStart -= BeginTransition;
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            Vector2 newPos;
            if (showFinalPosition) newPos = endAnimaticAnchorPosition;
            else newPos = offScreenAnchorPosition;

            rectTransform.anchoredPosition = newPos;
        }
    }

    void BeginTransition()
    {
        StartCoroutine(WaitBeforeTransition());
    }

    IEnumerator WaitBeforeTransition()
    {
        yield return new WaitForSeconds(timeBeforeFloatingIn);
        StartCoroutine(MoveToPosition());
    }

    IEnumerator MoveToPosition()
    {
        float elapsed = timeToFloatIn;

        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            float percent = elapsed / timeToFloatIn;
            if (smoothEnding) percent *= percent;
            percent = 1f - percent;
            rectTransform.anchoredPosition = Vector2.Lerp(offScreenAnchorPosition, endAnimaticAnchorPosition, percent);
            yield return null;
        }

        rectTransform.anchoredPosition = endAnimaticAnchorPosition;
        onFinishedTransition.Invoke();
    }
}
