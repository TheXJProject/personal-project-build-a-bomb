using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeShowValue : MonoBehaviour
{
    [SerializeField] Slider slider;

    TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        text.text = ((int)(slider.value * 100f)).ToString();
    }

}
