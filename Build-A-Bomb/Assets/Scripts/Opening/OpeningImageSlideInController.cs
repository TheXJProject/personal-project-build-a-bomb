using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningImageSlideInController : MonoBehaviour
{
    [SerializeField] List<OpeningImageSlideIn> slidingImages = new List<OpeningImageSlideIn>();

    public void BeginSlidingImages()
    {
        foreach (OpeningImageSlideIn image in slidingImages)
        {
            image.BeginTransition();
        }
    }
}
