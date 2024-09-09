using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueTaskLogic : MonoBehaviour
{
    Vector3 mousePos;
    public Camera cam;

    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;
        mousePos = cam.ScreenToWorldPoint(mousePos);

        float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, angle);
    }
}
