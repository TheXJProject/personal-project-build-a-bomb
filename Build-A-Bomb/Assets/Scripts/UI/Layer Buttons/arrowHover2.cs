using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowHover2 : MonoBehaviour
{
    public bool canShow = true;

    [SerializeField] GameObject arrow;
    public void showArrow()
    {
        if (canShow) arrow.SetActive(true);
    }

    public void hideArrow()
    {
        arrow.SetActive(false);
    }

    private void OnMouseEnter()
    {
        showArrow();
    }

    private void OnMouseExit()
    {
        hideArrow();
    }

    private void OnMouseDown()
    {
        hideArrow();
    }
}
