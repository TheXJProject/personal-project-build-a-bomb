using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class materializeIn : MonoBehaviour
{
    [SerializeField] float timeToFade;
    private SpriteRenderer sprite;

    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Materialize());
    }

    IEnumerator Materialize()
    {
        Color color = sprite.color;
        float elapsed = 0.0f;
        while (elapsed < timeToFade)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0,1,elapsed/timeToFade);
            color.a = alpha;
            sprite.color = color;

            yield return null;
        }
        color.a = 1;
        sprite.color = color;
    }
}
