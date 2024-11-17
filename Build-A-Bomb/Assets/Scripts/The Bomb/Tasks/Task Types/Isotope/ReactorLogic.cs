using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorLogic : MonoBehaviour
{
    // ==== For Debugging ====
    readonly bool Msg = false;

    // Inspector Adjustable Values:
    [SerializeField] [Range(0f, 5f)] float baseFanSpeed = 0;

    // Initialise In Inspector:
    [SerializeField] GameObject fan;

    // Runtime Variables:
    [HideInInspector] public bool canSpool;

    private void Awake()
    {
        canSpool = true;

        // Start fan at random angle
        fan.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }

    private void FixedUpdate()
    {
        if (canSpool)
        {
            fan.transform.Rotate(0, 0, baseFanSpeed);
        }
    }
}
