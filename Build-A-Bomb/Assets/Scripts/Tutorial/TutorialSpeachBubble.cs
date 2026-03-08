using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSpeachBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image bubbleBackground;
    [SerializeField] private Transform overallTransform;

    private string textToShow;
    private List<char> charList = new List<char>();
    private string textShowing;
    private Coroutine showingBubble;
    private Coroutine hidingBubble;
    private AnimationCurve showCurve;
    private AnimationCurve hideCurve;
    private float timeForBubbleToEnter;
    private float timeForTextToEnter;
    private float timeForBubbleToLeave;
    private float bubbleWaitBeforeShowTime;
    private float bubbleScale;
    private bool showing = false;

    private void Awake()
    {
        textToShow = text.text;
        foreach (char character in textToShow)
        {
            charList.Add(character);
        }

        text.text = "";
        bubbleBackground.enabled = false;
        bubbleScale = overallTransform.localScale.x;
    }

    public void SetShowAnimationCurveParams(AnimationCurve curve, float timeToEnter)
    {
        showCurve = curve;
        timeForBubbleToEnter = timeToEnter;
    }

    public void SetHideAnimationCurveParams(AnimationCurve curve, float timeToLeave)
    {
        hideCurve = curve;
        timeForBubbleToLeave = timeToLeave;
    }

    public void SetTimeForTextToEnter(float timeToEnter)
    {
        timeForTextToEnter = timeToEnter;
    }

    public void SetTimeToBeginEnter(float timeToBeginEnter)
    {
        bubbleWaitBeforeShowTime = timeToBeginEnter;
    }

    public void ShowBubble()
    {
        if (showing) return;
        showing = true;

        if (showingBubble != null)
            StopCoroutine(showingBubble);
        if (hidingBubble != null)
            StopCoroutine(hidingBubble);
        showingBubble = StartCoroutine(ShowingBubble());
    }
    public void HideBubble()
    {
        if (!showing) return;
        showing = false;

        if (hidingBubble != null)
            StopCoroutine(hidingBubble);
        if (showingBubble != null)
            StopCoroutine(showingBubble);
        hidingBubble = StartCoroutine(HidingBubble());
    }

    private IEnumerator ShowingBubble()
    {
        yield return new WaitForSeconds(bubbleWaitBeforeShowTime);
        text.text = "";
        bubbleBackground.enabled = true;
        float timer = 0;
        float currentScale = 0;
        Vector3 newScale;
        float overallTime = Mathf.Max(timeForTextToEnter, timeForBubbleToEnter);
        int numCharsToShow = charList.Count;
        int numCharsShown = 0;
        while (timer < overallTime)
        {
            timer += Time.deltaTime;

            // Bubble size
            float percentThroughSize = Mathf.Min(1, timer / timeForBubbleToEnter);
            currentScale = showCurve.Evaluate(percentThroughSize) * bubbleScale;
            newScale = new Vector3(currentScale, currentScale, currentScale);
            overallTransform.localScale = newScale;

            // Bubble text
            float percentThroughText = Mathf.Min(1, timer / timeForTextToEnter);
            int numCharsToBeShown = (int)(numCharsToShow * percentThroughText);
            for (int i = numCharsShown; i < numCharsToBeShown; i++)
            {
                text.text += charList[i];
                ++numCharsShown;
            }

            yield return null;
        }

        // Final Bubble size
        newScale = new Vector3(currentScale, currentScale, 0);
        overallTransform.localScale = newScale;

        // Final Bubble text
        text.text = textToShow;
    }

    private IEnumerator HidingBubble()
    {
        bubbleBackground.enabled = true;
        float timer = 0;
        float currentScale = 0;
        Vector3 newScale;
        while (timer < timeForBubbleToEnter)
        {
            timer += Time.deltaTime;

            // Bubble size
            float percentThroughSize = timer / timeForBubbleToLeave;
            currentScale = hideCurve.Evaluate(percentThroughSize) * bubbleScale;
            newScale = new Vector3(currentScale, currentScale, currentScale);
            overallTransform.localScale = newScale;

            yield return null;
        }

        // Final Bubble size
        newScale = new Vector3(0, 0, 0);
        overallTransform.localScale = newScale;
        bubbleBackground.enabled = false;
    }
}
