using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatUI : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] float floatCycleDuration = 1f;
    [SerializeField] float floatRadius = 5f;

    float startX, startY;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startX = rectTransform.anchoredPosition.x;
        startY = rectTransform.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        float yDisFromStart = Mathf.Sin((Time.time * 2 * Mathf.PI) * floatCycleDuration) * floatRadius;
        rectTransform.anchoredPosition = new Vector2(startX, startY + yDisFromStart);
    }
}
