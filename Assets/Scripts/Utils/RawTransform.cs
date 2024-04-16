using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawTransform
{
    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;
    public Matrix4x4 worldToLocal;
    public Matrix4x4 localToWorld;

    public RawTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        UpdateMatrix();
    }

    public RawTransform() : this(Vector3.zero, Quaternion.identity, Vector3.one) { }

    public RawTransform(Transform transform, bool useLocal) : this(
        useLocal ? transform.localPosition : transform.position,
        useLocal ? transform.localRotation : transform.rotation,
        transform.localScale)
    { }

    public static implicit operator RawTransform(Transform transform) => new(transform, true);

    public void Transfer(Transform transform, bool useLocal)
    {
        if (useLocal) transform.SetLocalPositionAndRotation(Position, Rotation);
        else transform.SetPositionAndRotation(Position, Rotation);

        transform.localScale = Scale;
    }

    public Vector3 Position
    {
        get => position;
        set => SetPosition(value);
    }

    public Quaternion Rotation
    {
        get => rotation;
        set => SetRotation(value);
    }

    public Vector3 Scale
    {
        get => scale;
        set => SetScale(value);
    }

    public float PositionX
    {
        get => position.x;
        set => SetPosition(new Vector3(value, position.y, position.z));
    }

    public float PositionY
    {
        get => position.y;
        set => SetPosition(new Vector3(position.x, value, position.z));
    }

    public float PositionZ
    {
        get => position.z;
        set => SetPosition(new Vector3(position.x, position.y, value));
    }

    public float ScaleX
    {
        get => scale.x;
        set => SetScale(new Vector3(value, scale.y, scale.z));
    }

    public float ScaleY
    {
        get => scale.y;
        set => SetScale(new Vector3(scale.x, value, scale.z));
    }

    public float ScaleZ
    {
        get => scale.z;
        set => SetScale(new Vector3(scale.x, scale.y, value));
    }

    public Vector3 RotationEulerAngles
    {
        get => rotation.eulerAngles;
        set => SetRotation(Quaternion.Euler(value));
    }

    public float RotationX
    {
        get => rotation.eulerAngles.x;
        set => SetRotation(Quaternion.Euler(value, rotation.eulerAngles.y, rotation.eulerAngles.z));
    }

    public float RotationY
    {
        get => rotation.eulerAngles.y;
        set => SetRotation(Quaternion.Euler(rotation.eulerAngles.x, value, rotation.eulerAngles.z));
    }

    public float RotationZ
    {
        get => rotation.eulerAngles.z;
        set => SetRotation(Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, value));
    }

    public Matrix4x4 WorldToLocalMatrix => worldToLocal;
    public Vector3 WorldToLocal(Vector3 vec) => WorldToLocalMatrix * vec.Vec4With1();

    public Matrix4x4 LocalToWorldMatrix => localToWorld;
    public Vector3 LocalToWorld(Vector3 vec) => LocalToWorldMatrix * vec.Vec4With1();

    public Vector3 Forward => LocalToWorldMatrix * Vector3.forward;
    public Vector3 Up => LocalToWorldMatrix * Vector3.up;
    public Vector3 Right => LocalToWorldMatrix * Vector3.right;

    public void SetPosition(Vector3 position)
    {
        this.position = position;
        UpdateMatrix();
    }

    public void SetRotation(Quaternion rotation)
    {
        this.rotation = rotation;
        UpdateMatrix();
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
        UpdateMatrix();
    }

    public void SetScale(Vector3 scale)
    {
        this.scale = scale;
        UpdateMatrix();
    }

    public static RawTransform LerpUnclamped(RawTransform from, RawTransform to, float t)
    {
        return new(Vector3.LerpUnclamped(from.position, to.position, t), Quaternion.SlerpUnclamped(from.rotation, to.rotation, t), Vector3.LerpUnclamped(from.scale, to.scale, t));
    }

    private void UpdateMatrix()
    {
        localToWorld = Matrix4x4.TRS(position, rotation, Scale);
        worldToLocal = localToWorld.inverse;
    }
}
