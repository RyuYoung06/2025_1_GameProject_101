using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targret;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;


    private void LateUpdate()
    {
        Vector3 desiredPosiltion = targret.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosiltion, smoothSpeed);
        transform.position = smoothPosition;

        transform.LookAt(transform.position);
    }


}
