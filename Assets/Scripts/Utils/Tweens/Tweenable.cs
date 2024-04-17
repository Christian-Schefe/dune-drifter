using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tweenable<T>
{
    public T Value { get; set; }
    public Tweenable(T value) => Value = value;

    public static explicit operator Tweenable<T>(T val)
    {
        return val switch
        {
            float f => new FloatTweenable(f) as Tweenable<T>,
            Vector2 v2 => new Vec2Tweenable(v2) as Tweenable<T>,
            Vector3 v3 => new Vec3Tweenable(v3) as Tweenable<T>,
            Quaternion q => new QuatTweenable(q) as Tweenable<T>,
            Color c => new ColorTweenable(c) as Tweenable<T>,
            RawTransform t => new TransformTweenable(t) as Tweenable<T>,
            _ => throw new NotImplementedException(),
        };
    }
    public static implicit operator T(Tweenable<T> val) => val.Value;

    public abstract T Lerp(T to, float t);
    public abstract T Offset(T by);
}
public class FloatTweenable : Tweenable<float>
{
    public FloatTweenable(float value) : base(value) { }
    public override float Lerp(float to, float t) => Mathf.LerpUnclamped(Value, to, t);
    public override float Offset(float by) => Value + by;
}
public class Vec2Tweenable : Tweenable<Vector2>
{
    public Vec2Tweenable(Vector2 value) : base(value) { }
    public override Vector2 Lerp(Vector2 to, float t) => Vector2.LerpUnclamped(Value, to, t);
    public override Vector2 Offset(Vector2 by) => Value + by;

}
public class Vec3Tweenable : Tweenable<Vector3>
{
    public Vec3Tweenable(Vector3 value) : base(value) { }
    public override Vector3 Lerp(Vector3 to, float t) => Vector3.LerpUnclamped(Value, to, t);
    public override Vector3 Offset(Vector3 by) => Value + by;
}
public class QuatTweenable : Tweenable<Quaternion>
{
    public QuatTweenable(Quaternion value) : base(value) { }
    public override Quaternion Lerp(Quaternion to, float t) => Quaternion.SlerpUnclamped(Value, to, t);
    public override Quaternion Offset(Quaternion by) => Value * by;
}
public class ColorTweenable : Tweenable<Color>
{
    public ColorTweenable(Color value) : base(value) { }
    public override Color Lerp(Color to, float t) => Color.LerpUnclamped(Value, to, t);
    public override Color Offset(Color by) => Value + by;
}
public class TransformTweenable : Tweenable<RawTransform>
{
    public TransformTweenable(RawTransform value) : base(value) { }
    public override RawTransform Lerp(RawTransform to, float t) => RawTransform.LerpUnclamped(Value, to, t);
    public override RawTransform Offset(RawTransform by) => new(Value.Position + by.Position, Value.Rotation * by.Rotation, Value.Scale + by.Scale);
}
