using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITweenable<T>
{
    public T Lerp(T to, float t);
    public T Offset(T by);
    public T Value { get; set; }
}

public class Tweenable<T> : ITweenable<T>
{
    public T Value { get; set; }
    public Tweenable(T value) => Value = value;
    protected virtual T LerpInternal(T to, float t) { throw new NotImplementedException(); }
    protected virtual T OffsetInternal(T to) { throw new NotImplementedException(); }

    public static implicit operator Tweenable<T>(T val) => new(val);
    public static implicit operator T(Tweenable<T> val) => val.Value;

    public T Lerp(T to, float t) => LerpInternal(to, t);
    public T Offset(T by) => OffsetInternal(by);

    public static T Lerp(T from, T to, float t) => ((Tweenable<T>)from).Lerp(to, t);
}
public class FloatTweenable : Tweenable<float>
{
    public FloatTweenable(float value) : base(value) { }
    protected override float LerpInternal(float to, float t) => Mathf.LerpUnclamped(Value, to, t);
    protected override float OffsetInternal(float to) => Value + to;
}
public class Vec2Tweenable : Tweenable<Vector2>
{
    public Vec2Tweenable(Vector2 value) : base(value) { }
    protected override Vector2 LerpInternal(Vector2 to, float t) => Vector2.LerpUnclamped(Value, to, t);
    protected override Vector2 OffsetInternal(Vector2 to) => Value + to;

}
public class Vec3Tweenable : Tweenable<Vector3>
{
    public Vec3Tweenable(Vector3 value) : base(value) { }
    protected override Vector3 LerpInternal(Vector3 to, float t) => Vector3.LerpUnclamped(Value, to, t);
    protected override Vector3 OffsetInternal(Vector3 to) => Value + to;
}
public class QuatTweenable : Tweenable<Quaternion>
{
    public QuatTweenable(Quaternion value) : base(value) { }
    protected override Quaternion LerpInternal(Quaternion to, float t) => Quaternion.SlerpUnclamped(Value, to, t);
    protected override Quaternion OffsetInternal(Quaternion to) => Value * to;
}
