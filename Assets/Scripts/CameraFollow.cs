using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;

    private float cameraRatio;
    private Camera mainCam;
    private Vector3 smoothPos;
    public float smoothSpeed = 0.5f;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
    }
    
    void LateUpdate()
    {
        smoothPos = Vector3.Lerp(this.transform.position, new Vector3(followTransform.position.x, followTransform.position.y, this.transform.position.z), smoothSpeed);
        this.transform.position = smoothPos;
    }
}