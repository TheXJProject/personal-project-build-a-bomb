using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class PingRing : MonoBehaviour
{
    [Header("Set the end scale percent and the time it takes to complete the ping:")]
    [SerializeField] float endScale = 2f;
    [SerializeField] float pingDuration = 0.8f;

    [Header("Initialise in inspector:")]
    [SerializeField] SpriteRenderer image;

    Vector2 startScale;
    [SerializeField] Color debugColor = Color.yellow;
    [SerializeField] bool debug = false;
    Color startColor;
    Coroutine ping;
    [HideInInspector] public bool goingWrong = false;

    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void OnEnable()
    {
        InitiatePing();
    }

    private void OnDisable()
    {
        goingWrong = false;
    }

    void InitiatePing()
    {
        if (ping != null) StopCoroutine(ping);
        if (goingWrong && debug) startColor = debugColor;
        else if (goingWrong) startColor = Color.yellow;
        else startColor = Color.white;
        ping = StartCoroutine(DoPing());
    }

    IEnumerator DoPing()
    {
        transform.localScale = startScale;
        image.color = startColor;

        Color curCol = startColor;
        float timer = pingDuration;

        do
        {
            timer -= Time.deltaTime;
            float incPerc = 1 - (timer / pingDuration);
            float decPerc = timer / pingDuration;

            transform.localScale = ((startScale * endScale) * incPerc) + (startScale * decPerc);

            curCol.a = decPerc;
            image.color = curCol;

            yield return null;
        } 
        while (timer > 0f);

        transform.localScale = startScale * endScale;
        curCol.a = 0f;
        image.color = curCol;

        if (goingWrong) InitiatePing();
    }
}
