using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement> {

    [SerializeField]
    bool isLocked;
    [SerializeField]
    Camera cam;
    [SerializeField]
    Transform player1, player2;

    [Header("Camera Active")]
    [SerializeField]
    float smoothSpeed = .125f;
    [SerializeField]
    float zoomFactor = 1;
    [SerializeField]
    Vector3 rotation, offset;

    void FixedUpdate() {
        if (!isLocked && GameManager.isPlaying) FixedCameraFollowSmooth();    
    }

    void NormalMovement() {
        transform.position = SmoothedPosition();
        
        if (isRotationNeeded())
            transform.rotation = SmoothedRotation();
    }
    void FixedCameraFollowSmooth() {
        Transform t1 = player1;
        Transform t2 = player2;

        float followTimeDelta = 0.8f;

        Vector3 midpoint = (t1.position + t2.position + offset) / 2f;
       
        float distance = (t1.position - t2.position).magnitude;

        Vector3 cameraDestination = midpoint - transform.forward * distance * zoomFactor;

        if (cam.orthographic)
            cam.orthographicSize = distance;
       
        transform.position = Vector3.Slerp(transform.position, cameraDestination, followTimeDelta);

        if ((cameraDestination - transform.position).magnitude <= 0.05f)
            transform.position = cameraDestination;

        if (isRotationNeeded())
            transform.rotation = SmoothedRotation();
    }

    bool isRotationNeeded() => transform.rotation.x != rotation.x;
   
    Vector3 SmoothedPosition() {
        return Vector3.Lerp(transform.position, DesiredPosition(), smoothSpeed);
    }
    Vector3 DesiredPosition() => player1.position + player2.position + offset;
   
    Quaternion SmoothedRotation() {
        return Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), smoothSpeed);
    }
}
   