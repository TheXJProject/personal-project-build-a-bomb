using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colourCycle : MonoBehaviour
{
    [SerializeField] int minValCol;
    [SerializeField] int maxValCol;
    [SerializeField] float colourCycleSpeed;
    [SerializeField] int opacity = 255;

    int diffValCol;
    int[] rgb = new int[3];
    bool[] rgbCurIncreasing = new bool[3];
    int currCol = 0;
    float alpha = 0;
    Image spriteToColour;

    private void Awake()
    {
        spriteToColour = gameObject.GetComponent<Image>();
        diffValCol = maxValCol - minValCol;
        for (int i = 0; i < rgb.Length; i++)
        {
            rgb[i] = ((i%2) == 0) ? minValCol : maxValCol;
            rgbCurIncreasing[i] = ((i%2) == 0) ? true : false;
        }
    }

    private void Update()
    {
        spriteToColour.color = new Color(rgb[0] / 255.0f, rgb[1] / 255.0f, rgb[2] / 255.0f, opacity/255.0f);

        alpha += Time.deltaTime * colourCycleSpeed;
        
        float alphaPlusMinus = rgbCurIncreasing[currCol] ? alpha : (1 - alpha);

        rgb[currCol] = (int)(alphaPlusMinus * diffValCol) + minValCol;
        if (rgb[currCol] >= maxValCol && rgbCurIncreasing[currCol]) rgb[currCol] = maxValCol;
        else if (rgb[currCol] <= minValCol && !rgbCurIncreasing[currCol]) rgb[currCol] = minValCol;
        else return;

        rgbCurIncreasing[currCol] = !rgbCurIncreasing[currCol];
        currCol++;
        currCol %= 3;

        alpha = 0;
    }
}
