using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedEndGameAnimation : MonoBehaviour
{
    public static event Action onEndGameFadeOutStart;

    [SerializeField] DisableAndEnableForEndGame endGameManager;
    [SerializeField] bool gameEndState = false;

    [SerializeField] float pauseBeforeFadeDuration = 2f;
    [SerializeField] float fadeDuration = 2f;
    [SerializeField] Image image;
    Color startColor;



    void Start()
    {
        startColor = image.color;
    }
    public void finishedAnimation()
    {
        endGameManager.loadEndGameScreen(gameEndState);
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        onEndGameFadeOutStart?.Invoke();
        yield return new WaitForSeconds(pauseBeforeFadeDuration);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, Mathf.Sqrt(elapsed / fadeDuration));
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        image.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        gameObject.SetActive(false);
    }
}
