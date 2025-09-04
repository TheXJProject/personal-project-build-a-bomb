using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class materializeIn : MonoBehaviour
{
    [SerializeField] float startAmount;
    [SerializeField] float timeToMaterialize;
    [SerializeField] Material materialize;
    [SerializeField] Material defaultMat;

    float a = 1.6f, f = 5.8f, b = 0.2f;

    private void OnEnable()
    {
        StartCoroutine(Materialize());
    }

    IEnumerator Materialize()
    {
        GetComponent<SpriteRenderer>().material = materialize;
        float elapsed = 0.0f;
        while (elapsed < timeToMaterialize)
        {
            elapsed += Time.deltaTime;
            materialize.SetFloat("_Fade", Mathf.Clamp01(myMathFunction(elapsed, timeToMaterialize) * (1.0f - startAmount)) + startAmount);
            yield return null;
        }
        materialize.SetFloat("_Fade", 1.0f);
        GetComponent<SpriteRenderer>().material = defaultMat;

    }

    float myMathFunction(float elapsed, float totalTime)
    {
        float x = elapsed / totalTime;
        return ((-Mathf.Exp(a - (f * x))) * b + 1);
    }

}
