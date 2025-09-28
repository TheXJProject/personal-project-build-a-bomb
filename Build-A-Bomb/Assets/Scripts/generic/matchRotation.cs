using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matchRotation : MonoBehaviour
{
    [SerializeField] Transform objectToMatch;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = objectToMatch.rotation;
    }
}
