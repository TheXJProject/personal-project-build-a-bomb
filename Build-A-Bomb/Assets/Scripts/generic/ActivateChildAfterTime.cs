using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChildAfterTime : MonoBehaviour
{
    [SerializeField] float timeBeforeFadeIn;
    [SerializeField] GameObject child;

    IEnumerator ActivateChild()
    {
        yield return new WaitForSeconds(timeBeforeFadeIn);
        child.SetActive(true);
    }
}
