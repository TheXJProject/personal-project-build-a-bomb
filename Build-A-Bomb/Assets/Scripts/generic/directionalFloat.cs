using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionalFloat : MonoBehaviour
{
    [SerializeField] float floatDisScreenHeightPercent;
    [SerializeField] float angleDirOfFloat;
    [SerializeField] float floatSpeed;
    [SerializeField] float offsetStart;

    Vector3 StartPoint;
    Vector3 endPoint;

    Vector3 StartPointPercentOf;
    Vector3 endPointPercentOf;

    float alpha = 0;
    float timePassed = 0;

    Vector3 getFloatDirectionVector()
    {
        Quaternion rot = Quaternion.Euler(0, 0, angleDirOfFloat);
        Vector3 directionVector = (rot * Vector3.up);

        return directionVector;
    }
    private void OnDrawGizmos()
    {
        Vector3 pointsTo = transform.position + getFloatDirectionVector() * floatDisScreenHeightPercent * Screen.height;
        Gizmos.DrawLine(transform.position, pointsTo);
    }
    private void Awake()
    {
        StartPointPercentOf = transform.position / Screen.height;
        endPointPercentOf = ((transform.position + (getFloatDirectionVector() * floatDisScreenHeightPercent * Screen.height))) / Screen.height;
    }

    private void Update()
    {

        StartPoint = StartPointPercentOf * Screen.height;
        endPoint = endPointPercentOf * Screen.height;

        timePassed += Time.deltaTime;
        alpha = (Mathf.Sin((timePassed * floatSpeed) + (offsetStart * Mathf.PI)) + 1) / 2;

        transform.position = Vector3.Lerp(StartPoint, endPoint, alpha);
    }
}
