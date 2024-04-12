using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;

    private Vector3 velocity;
    private Vector3 curTarget;

    private void Update()
    {
        var car = Globals.Get<Car>();

        curTarget = Vector3.SmoothDamp(curTarget, car.transform.position, ref velocity, 0.05f);
        curTarget = car.transform.position;
        var speedMul = 1.0f;//MathU.Remap(0.0f, 30.0f, 1.0f, 1.5f, car.velocity.magnitude, true);

        var targetPos = curTarget + new Vector3(speedMul * offset.x, offset.y, speedMul * offset.z);

        transform.position = targetPos;
        transform.LookAt(curTarget);
    }
}
