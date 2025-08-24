using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerButtonInfo : MonoBehaviour
{
    [SerializeField] LayerButtonAppearance app;
    [SerializeField] LayerButtonPress pre;
    [SerializeField] public Animator animator;
    [SerializeField] public Animator image;

    // Runtime Variables:
    public GameObject correspondingLayer;

    public void setLayer()
    {
        app.correspondingLayer = correspondingLayer;
        pre.correspondingLayer = correspondingLayer;
    }
}
