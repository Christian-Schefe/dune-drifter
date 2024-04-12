using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public ComponentCache<Collider> bounds;

    [SerializeField] private bool isFrontWheel;

    public float turnAngle;
    public float visualAngle;

    public List<Surface> touchedSurfaces = new();

    private void Update()
    {
        touchedSurfaces.Clear();
        var results = Physics.RaycastAll(transform.position, -transform.up, 2.0f);
        foreach (var hit in results)
        {
            if (hit.transform.gameObject.TryGetComponent<Surface>(out var surface))
            {
                touchedSurfaces.Add(surface);
            }
        }
    }

    public void SetTurnAngle(float angle)
    {
        if (!isFrontWheel) return;
        turnAngle = angle;
        var eulerAngle = MathU.Remap(-1.0f, 1.0f, -visualAngle, visualAngle, angle);
        var rot = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(rot.x, eulerAngle, rot.z);
    }

    public bool IsGrounded => touchedSurfaces.Count > 0;
    public float Grip => IsGrounded ? touchedSurfaces.Average(e => e.friction) : 0;

    public Vector3 Accel(Vector3 vel)
    {
        var counterVel = 0.1f * (1.0f - Mathf.Abs(Vector3.Dot(transform.forward, vel.normalized)));
        return Grip * transform.forward - Grip * counterVel * vel;
    }
    public Vector3 Torque => Grip * turnAngle * transform.up;
}
