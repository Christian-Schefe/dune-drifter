using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;

    private Vector3 velocity;

    private void Update()
    {
        var car = Globals.Get<Car>();

        var targetPos = car.transform.position + offset * MathU.Remap(0.0f, 30.0f, 1.0f, 1.5f, car.values.vel.magnitude, true);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.1f);
        transform.position = targetPos;
        transform.LookAt(car.transform.position);
    }
}
