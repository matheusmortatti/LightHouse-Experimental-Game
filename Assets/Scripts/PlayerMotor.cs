using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    private Camera _camera;

    public Camera camera
    {
        get { return _camera; }
    }

    private Vector3 velocity        = Vector3.zero;
    private Vector3 rotation        = Vector3.zero;
    private Vector3 cameraRotation  = Vector3.zero;

    private Rigidbody rb;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        velocity = Vector3.zero;
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        _camera.transform.Rotate(cameraRotation);

        Vector3 cameraAngle = _camera.transform.localEulerAngles;
        
        // Clamp angle value to -90 and 90
        if(cameraAngle == new Vector3(cameraAngle.x, 180, 180))
        {
            if(cameraAngle.x > 0 && cameraAngle.x <= 90)
            {
                cameraAngle = new Vector3(90, 0, 0);
            }
            else
            {
                cameraAngle = new Vector3(270, 0, 0);
            }
        }

        _camera.transform.localEulerAngles = cameraAngle;

        rotation = Vector3.zero;
        cameraRotation = Vector3.zero;
    }

    public void ApplyVelocity(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void ApplyRotation(Vector3 _rotation)
    {
        rotation += new Vector3(0, _rotation.y, 0);
        cameraRotation += new Vector3(_rotation.x, 0, 0);
    }

    public void LookAt(Vector3 _direction)
    {
        Vector3 rb_euler =  rb.rotation.eulerAngles;
        rb.MoveRotation(Quaternion.LookRotation(new Vector3(rb_euler.x, _direction.y, rb_euler.z)));
        //_camera.transform.rotation.x = Quaternion.Euler(new Vector3(_rotation.x, 0, 0)).x;

        Vector3 cameraAngle = _camera.transform.localEulerAngles;

        // Clamp angle value to -90 and 90
        if (cameraAngle == new Vector3(cameraAngle.x, 180, 180))
        {
            if (cameraAngle.x > 0 && cameraAngle.x <= 90)
            {
                cameraAngle = new Vector3(90, 0, 0);
            }
            else
            {
                cameraAngle = new Vector3(270, 0, 0);
            }
        }

        _camera.transform.localEulerAngles = cameraAngle;
    }
}
