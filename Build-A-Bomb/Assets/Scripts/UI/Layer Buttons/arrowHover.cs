using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrowHover : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] Image button;
    [SerializeField] LayerButtonInfo info;

    [SerializeField] Color hoverColor;

    public void showArrow()
    {
        if (!info.correspondingLayer.GetComponent<LayerStatus>().isSelected)
        {
            arrow.SetActive(true);
            button.color = hoverColor;
        }
    }

    public void hideArrow()
    {
        arrow.SetActive(false);
        button.color = Color.white;
    }
}
