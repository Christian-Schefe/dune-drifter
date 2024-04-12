using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct CarParams
{
    public float breakStrength;
    public float accelStrength;
    public float torqueStrength;
}

public class Car : MonoBehaviour
{
    public ComponentCache<Rigidbody> rb = new();
    public ChildrenCache<Wheel> wheels = new();

    public float turnAngle;
    public float throttle;
    public float breaking;

    public int wheelsOnGround;

    public Vector3 velocity;
    public Vector3 wheelAcceleration;

    public CarParams settings;

    public Vector3 respawnPos;
    public Quaternion respawnRot;

    private void Awake()
    {
        respawnPos = transform.position;
        respawnRot = transform.rotation;
    }

    private void Update()
    {
        UseInput();
        var rigidbody = rb.Get(gameObject);
        velocity = rigidbody.velocity;

        var acc = throttle * settings.accelStrength - breaking * settings.breakStrength;
        var torq = settings.torqueStrength * MathU.Remap(0.0f, 1.0f, 0.0f, 1.0f, velocity.magnitude, true);

        var wheelsArr = wheels.Get(gameObject);
        foreach (var wheel in wheelsArr)
        {
            wheel.SetTurnAngle(turnAngle);

            rigidbody.AddForce(wheel.Accel(velocity) * acc, ForceMode.Acceleration);
            rigidbody.AddTorque(wheel.Torque * torq, ForceMode.Acceleration);
        }

        //rigidbody.velocity = velocity;

        wheelsOnGround = wheels.Get(gameObject).Where(e => e.IsGrounded).Count();
    }

    private void UseInput()
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");

        turnAngle = horizontal;
        throttle = Mathf.Max(0.0f, vertical);
        breaking = Mathf.Max(0.0f, -vertical);

        if (Input.GetKeyUp(KeyCode.R))
        {
            transform.SetPositionAndRotation(respawnPos, respawnRot);
            rb.Get(gameObject).velocity = Vector3.zero;
            rb.Get(gameObject).angularVelocity = Vector3.zero;
        }
    }
}
