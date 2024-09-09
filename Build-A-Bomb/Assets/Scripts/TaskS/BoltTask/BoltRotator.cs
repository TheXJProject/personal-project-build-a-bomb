using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltRotator : MonoBehaviour
{
    public float rotationSpeed = 200f;  // Speed of the rotation
    public float screwDepthSpeed = 0.05f;  // Speed of moving the bolt down
    public float maxDepth = 1.5f;  // Maximum depth for the bolt to screw in

    private Vector3 initialPosition;  // Initial position of the bolt
    private float currentDepth = 0.0f;  // Tracks how deep the bolt is screwed

    void Start()
    {
        // Store the initial position of the bolt
        initialPosition = transform.position;
    }

    void Update()
    {
        // If the bolt has not reached the max depth
        if (currentDepth < maxDepth)
        {
            // Rotate the bolt around the Z-axis (2D rotation)
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // Move the bolt downward along the Y-axis
            transform.position -= new Vector3(0, screwDepthSpeed * Time.deltaTime, 0);

            // Update current depth
            currentDepth += screwDepthSpeed * Time.deltaTime;
        }
    }
}

