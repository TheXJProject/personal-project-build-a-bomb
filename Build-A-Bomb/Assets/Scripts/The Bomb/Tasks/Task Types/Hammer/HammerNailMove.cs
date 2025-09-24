using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HammerNailMove : MonoBehaviour
{
    [SerializeField] float maxDegreesTurned = 8;
    [SerializeField] float minDegreesTurned = 2;

    float previous = 0;

    public void setRandomRotation()
    {
        float rotateAmount = UnityEngine.Random.Range(0f, 1f) * (maxDegreesTurned- minDegreesTurned) + minDegreesTurned;

        if ((previous < 0 && rotateAmount < 0) || (previous > 0 && rotateAmount > 0))
        {
            bool randomBool = UnityEngine.Random.value > 0.15f;
            if (randomBool) rotateAmount *= -1;
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, rotateAmount);
        previous = rotateAmount;
    }
}
