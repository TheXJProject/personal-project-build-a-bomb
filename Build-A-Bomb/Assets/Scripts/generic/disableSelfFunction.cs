using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableSelfFunction : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
    public void disableThisGameObject()
    {
        gameObject.SetActive(false);
    }
}
