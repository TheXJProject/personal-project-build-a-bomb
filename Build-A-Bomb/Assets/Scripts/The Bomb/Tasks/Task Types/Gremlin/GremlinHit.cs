using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GremlinHit : MonoBehaviour
{
    // The gremlin starts not hit
    public GameObject mallet;
    public GameObject shadow;
    [SerializeField] Sprite[] gremlinSprites;
    public Sprite splatSprite;
    [SerializeField] float growInTime = 0.1f;

    bool hit = false;
    Vector2 startSize;
    Coroutine growInRoutine;
    bool grownIn = false;

    private void Awake()
    {
        int randomIndex = UnityEngine.Random.Range(0, gremlinSprites.Length);

        // Set the sprite
        GetComponent<Image>().sprite = gremlinSprites[randomIndex];
        startSize = GetComponent<RectTransform>().sizeDelta;
    }
    private void OnEnable()
    {
        if (!hit)
        {
            if (growInRoutine != null) StopCoroutine(growInRoutine);
            growInRoutine = StartCoroutine(GrowIn());
        }
    }
    public void GremlinGotHit(BaseEventData data)
    {
        // If this gremlin is not hit yet
        if (!hit)
        {
            PointerEventData newData = (PointerEventData)data;
            if (newData.button.Equals(PointerEventData.InputButton.Left))
            {
                // If gremlin clicked on tell main script
                GremlinTaskLogic parentScript = GetComponentInParent<GremlinTaskLogic>();
                parentScript.GremlinHit();
                
                // The gremlin is now hit
                hit = true;
            }
        }
    }

    IEnumerator GrowIn()
    {
        float elapsed = 0f;
        
        Vector2 endSize = Vector2.zero;

        while (elapsed < growInTime)
        {
            elapsed += Time.deltaTime;
            if (!grownIn)
            {
                GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(endSize, startSize, elapsed / growInTime);
            }
            yield return null;
        }
        GetComponent<RectTransform>().sizeDelta = startSize;
        grownIn = true;
    }
}