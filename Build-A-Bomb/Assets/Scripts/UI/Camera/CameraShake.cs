using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeDurationPercent;
    [SerializeField] AnimationCurve curve;
    [SerializeField] AnimationCurve curveExplode;
    [SerializeField] AnimationCurve curveVictory;
    [SerializeField] AnimationCurve defaultCamShake;
    Coroutine camShake;

    private void OnEnable()
    {
        DisableAndEnableForEndGame.onLightsOut += BeginCameraShakeLightsOut;
        CrazyColourBomb.onCrazyExplosionStarted += BeginCameraShakeExplode;
        CrazyColourBomb.onCrazyVictoryStarted += BeginCameraShakeVictory;
        FinishedEndGameAnimation.onEndGameFadeOutStart += SetCamStartSize;
    }

    private void OnDisable()
    {
        DisableAndEnableForEndGame.onLightsOut -= BeginCameraShakeLightsOut;
        CrazyColourBomb.onCrazyExplosionStarted -= BeginCameraShakeExplode;
        CrazyColourBomb.onCrazyVictoryStarted -= BeginCameraShakeVictory;
        FinishedEndGameAnimation.onEndGameFadeOutStart -= SetCamStartSize;
    }

    public void SetCamStartSize()
    {
        GetComponent<Camera>().orthographicSize = 3.5f; // I've given up trying to connect stuff up I just wanna be finsihed, Cadandra Cadumdra heres a magic number
    }

    public void BeginCameraShakeLightsOut(float duration)
    {
        if (camShake != null) StopCoroutine(camShake);
        camShake = StartCoroutine(CamShake(duration, curve));
    }

    public void BeginCameraShakeExplode(float duration)
    {
        if (camShake != null) StopCoroutine(camShake);
        camShake = StartCoroutine(CamShake(duration, curveExplode));
    }
    public void BeginCameraShakeVictory(float duration)
    {
        if (camShake != null) StopCoroutine(camShake);
        camShake = StartCoroutine(CamShake(duration, curveVictory));
    }


    IEnumerator CamShake(float duration, AnimationCurve inputCurve)
    {
        float scale = GetComponent<Camera>().orthographicSize / 3.5f; // yeah I can't be bothered to not make this a magic number, just need to reduce intensity a bit so the shake isn't too much
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float newDuration = duration * shakeDurationPercent;// Liturally no clue why I did this I Cry

        while (elapsedTime < newDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = inputCurve.Evaluate(elapsedTime / newDuration);
            transform.position = startPosition + (Vector3)Random.insideUnitCircle * strength * scale;
            yield return null;
        }
        transform.position = startPosition;
    }
}
