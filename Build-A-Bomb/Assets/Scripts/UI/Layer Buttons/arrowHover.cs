using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowHover : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] LayerButtonInfo info;

    public void showArrow()
    {
        if (!info.correspondingLayer.GetComponent<LayerStatus>().isSelected)
        {
            arrow.SetActive(true);
        }
    }

    public void hideArrow()
    {
        arrow.SetActive(false);
    }
}
