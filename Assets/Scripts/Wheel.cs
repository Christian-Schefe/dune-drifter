using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Transform wheelModel;

    [HideInInspector] public WheelCollider WheelCollider;

    public bool steerable;
    public bool motorized;

    Vector3 position;
    Quaternion rotation;

    private void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    void Update()
    {
        WheelCollider.GetWorldPose(out position, out rotation);
        wheelModel.transform.SetPositionAndRotation(position, rotation);
    }
}
