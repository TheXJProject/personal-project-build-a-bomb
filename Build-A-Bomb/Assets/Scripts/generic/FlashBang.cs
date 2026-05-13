using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBang : MonoBehaviour
{
    [SerializeField] BombStatus bombStatus;
    [SerializeField] Image image;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float flashTime;

    private void OnEnable()
    {
        BombStatus.onLayerCreated += DoFlashBang;
    }

    private void OnDisable()
    {
        BombStatus.onLayerCreated -= DoFlashBang;
    }

    private void DoFlashBang(GameObject layer)
    {
        LayerStatus layerStatus = layer.GetComponent<LayerStatus>();
        if (layerStatus && layer.GetComponent<LayerStatus>().layer == bombStatus.finalLayer)
        {
            StartCoroutine(FlashBangRoutine());
        }
    }

    IEnumerator FlashBangRoutine()
    {
        Color color = image.color;

        float elapsed = 0;
        while (elapsed < flashTime)
        {
            elapsed += Time.deltaTime;
            float alpha = curve.Evaluate(elapsed / flashTime);
            color.a = alpha;
            image.color = color;

            yield return null;
        }
        color.a = 0;
        image.color = color;
    }
}
