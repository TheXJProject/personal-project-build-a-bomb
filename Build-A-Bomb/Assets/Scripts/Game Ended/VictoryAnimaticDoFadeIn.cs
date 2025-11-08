using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryAnimaticDoFadeIn : MonoBehaviour
{
    [Header("Configure victory animatic slide in")]
    [SerializeField] float timeBeforeFadingIn;
    [SerializeField] float timeToFadeIn;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.raycastTarget = false;
        Color startColour = image.color;
        startColour.a = 0f;
        image.color = startColour;
    }
    private void OnEnable()
    {
        BeginVictoryScreenAnimatic.onVictoryScreenButtonsAppear += BeginTransition;
    }
    private void OnDisable()
    {
        BeginVictoryScreenAnimatic.onVictoryScreenButtonsAppear -= BeginTransition;
    }

    void BeginTransition()
    {
        StartCoroutine(WaitBeforeTransition());
    }

    IEnumerator WaitBeforeTransition()
    {
        yield return new WaitForSeconds(timeBeforeFadingIn);
        StartCoroutine(FadeInPosition());
    }

    IEnumerator FadeInPosition()
    {
        image.raycastTarget = true;
        float elapsed = 0f;

        Color startColour = image.color;

        while (elapsed < timeToFadeIn)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / timeToFadeIn;
            startColour.a = Mathf.Lerp(0f, 1f, percent);
            image.color = startColour;
            yield return null;
        }
        startColour.a = 1f;
        image.color = startColour;
    }

}
