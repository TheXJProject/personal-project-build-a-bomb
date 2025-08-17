using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinScript : MonoBehaviour
{
    [SerializeField] float spinSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its Y axis (you can change this to X or Z for other directions)
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
