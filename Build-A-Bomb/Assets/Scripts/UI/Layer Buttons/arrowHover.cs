using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowHover : MonoBehaviour
{
    [SerializeField] GameObject arrow;

    public void showArrow()
    {
        arrow.SetActive(true);
    }

    public void hideArrow()
    {
        arrow.SetActive(false);
    }
}
