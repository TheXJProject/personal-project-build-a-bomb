using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HammerNailMove : MonoBehaviour
{
    [SerializeField] float maxDegreesTurned = 8;
    [SerializeField] float minDegreesTurned = 2;

    bool lastDirectionNeg = false;

    public void setRandomRotation()
    {
        float rotateAmount = UnityEngine.Random.Range(0f, 1f) * (maxDegreesTurned- minDegreesTurned) + minDegreesTurned;

        if (lastDirectionNeg) lastDirectionNeg = false; // This ensures that the nail goes back and forth each hit
        else
        {
            rotateAmount *= -1;
            lastDirectionNeg = true;
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, rotateAmount);
    }
}
