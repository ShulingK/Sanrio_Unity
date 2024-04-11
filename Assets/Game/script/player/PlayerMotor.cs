using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void CameraRotate(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            if(!rb.SweepTest(transform.forward, out RaycastHit hit, (velocity * Time.fixedDeltaTime).magnitude))
                rb.Move(rb.position + velocity * Time.fixedDeltaTime, rb.rotation);
        }
    }

    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if ((cam.transform.eulerAngles.x - cameraRotation.x) <= 90f || (cam.transform.eulerAngles.x - cameraRotation.x) >= 270f)
            cam.transform.Rotate(-cameraRotation);
    }
}
